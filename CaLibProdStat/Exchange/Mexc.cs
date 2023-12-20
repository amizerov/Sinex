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
        return new List<Kline>();
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
        }

        return products;
    }

    protected override Product ToProduct(object p)
    {
        throw new NotImplementedException();
    }
}
