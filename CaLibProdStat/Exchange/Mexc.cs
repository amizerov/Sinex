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
                    k.OpenPrice = Decimal.Parse(p[1].GetString()!.Replace(".", ","));
                    k.HighPrice = Decimal.Parse(p[2].GetString()!.Replace(".", ","));
                    k.LowPrice = Decimal.Parse(p[3].GetString()!.Replace(".", ","));
                    k.ClosePrice = Decimal.Parse(p[4].GetString()!.Replace(".", ","));
                    k.Volume = Decimal.Parse(p[5].GetString()!.Replace(".", ","));

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
}
