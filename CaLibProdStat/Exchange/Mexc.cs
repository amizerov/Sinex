using CryptoExchange.Net.CommonObjects;
using System.Net;

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
                //System.Text.Json.
            }
        }

        return products;
    }

    protected override Product ToProduct(object p)
    {
        throw new NotImplementedException();
    }
}
