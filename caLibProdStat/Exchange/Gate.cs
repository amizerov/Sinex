using amLogger;
using CryptoExchange.Net.CommonObjects;
using System;
using System.Net;
using System.Text.Json;

namespace caLibProdStat;

public class Gate : AnExchange
{
    public override int ID => 11;

    public override string Name => "Gate";

    protected override List<Kline> GetLastKlines(string symbol)
    {
        List<Kline> klines = new();

        using (HttpClient c = new())
        {
            var r = c.GetAsync($"https://api.gateio.ws/api/v4/spot/candlesticks?currency_pair={symbol}&interval=1m").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                foreach (var p in e.EnumerateArray())
                {
                    Kline k = new();

                    k.OpenTime = UnixTimeStampToDateTime(p[0]);
                    k.Volume = sd(p[1]);
                    k.ClosePrice = sd(p[2]);
                    k.HighPrice = sd(p[3]);
                    k.LowPrice = sd(p[4]);
                    k.OpenPrice = sd(p[5]);

                    klines.Add(k);
                }
            }
            else
            {
                Log.Error(ID, $"GetLastKlines({symbol})", r.StatusCode.ToString());
            }
        }
        return klines;
    }

    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (HttpClient c = new())
        {
            var r = c.GetAsync("https://api.gateio.ws/api/v4/spot/currency_pairs").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                foreach (var p in e.EnumerateArray())
                {
                    Product product = new();
                    product.symbol = p.GetProperty("id").GetString() + "";
                    product.exchange = ID;
                    product.baseasset = p.GetProperty("base").GetString() + "";
                    product.quoteasset = p.GetProperty("quote").GetString() + "";

                    product.IsTradingEnabled =
                        p.GetProperty("trade_status").GetString() == "tradable";

                    products.Add(product);
                }
            }
            else
            {
                Log.Error(ID, $"GetProduct()", r.StatusCode.ToString());
            }
        }

        return products;
    }

    protected override Product ToProduct(object p)
    {
        throw new NotImplementedException();
    }

    decimal sd(JsonElement j)
    {
        decimal d = 0;
        string s = j.GetString()!;
        if (s.Contains("E"))
        {
            string[] p = s.Split("E");
            d = Decimal.Parse(p[0].Replace(".", ",")) * (decimal)Math.Pow(10, int.Parse(p[1]));
        }
        else
        {
            d = Decimal.Parse(s.Replace(".", ","));
        }
        return d;
    }
    public static DateTime UnixTimeStampToDateTime(JsonElement j)
    {
        double unixTimeStamp = double.Parse(j.GetString()!);
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    string baseAssetLast = "";
    public override Coin GetCoinDetails(string baseAsset)
    {
        if (baseAssetLast == baseAsset) return new Coin();
        baseAssetLast = baseAsset;

        var host = "https://api.gateio.ws";
        var prefix = "/api/v4";
        var url = "/wallet/currency_chains";

        using HttpClient clt = new();

        string uri = $"{host}{prefix}{url}?currency={baseAsset}";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        Coin cd = new();

        var r = clt.SendAsync(req).Result;
        if (r.IsSuccessStatusCode)
        {
            var s = r.Content.ReadAsStringAsync().Result;
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;

            cd.exchId = ID;
            cd.asset = baseAsset;
            int i = 0;
            foreach (var p in e.EnumerateArray()) 
            {
                cd.network = p.GetProperty("chain").GetString() + "";
                cd.contract = p.GetProperty("contract_address").GetString() + "";
                cd.longName = (++i) + "";
            }
            cd.Save().Wait();

            //Thread.Sleep(200);
        }

        return cd;
    }
}
