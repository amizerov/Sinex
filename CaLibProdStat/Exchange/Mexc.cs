using amLogger;
using CryptoExchange.Net.CommonObjects;
using System.Net;
using System.Text.Json;

namespace caLibProdStat;

public class Mexc : AnExchange
{
    public override int ID => 10;

    public override string Name => "Mexc";

    protected override List<Kline> GetLastKlines(string symbol)
    {
        List<Kline> klines = new();

        using (HttpClient c = new())
        {
            var r = c.GetAsync($"https://api.mexc.com/api/v3/klines?symbol={symbol}&interval=1m").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                foreach (var p in e.EnumerateArray())
                {
                    Kline k = new();

                    double t = Math.Round(p[0].GetInt64() / 1000d);
                    k.OpenTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(t).ToLocalTime();
                    k.OpenPrice = sd(p[1]);
                    k.HighPrice = sd(p[2]);
                    k.LowPrice = sd(p[3]);
                    k.ClosePrice = sd(p[4]);
                    k.Volume = sd(p[5]);

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
            var r = c.GetAsync("https://api.mexc.com/api/v3/exchangeInfo").Result;
            if(r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement.GetProperty("symbols");
                foreach (var p in e.EnumerateArray())
                {
                    Product product = new();
                    product.symbol = p.GetProperty("symbol").GetString() + "";
                    product.exchange = ID;
                    product.baseasset = p.GetProperty("baseAsset").GetString() + "";
                    product.quoteasset = p.GetProperty("quoteAsset").GetString() + "";

                    product.IsTradingEnabled = 
                        p.GetProperty("status").GetString() == "ENABLED" &&
                        p.GetProperty("isSpotTradingAllowed").GetBoolean();

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
}
