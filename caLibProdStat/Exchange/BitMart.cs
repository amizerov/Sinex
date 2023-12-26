using amLogger;
using CryptoExchange.Net.CommonObjects;
using System.Net;
using System.Text.Json;

namespace caLibProdStat;

public class BitMart : AnExchange
{
    const string BASE_URL = "https://api-cloud.bitmart.com";

    public override int ID => 12;

    public override string Name => "BitMart";

    protected override List<Kline> GetLastKlines(string symbol)
    {
        List<Kline> klines = new();

        using (HttpClient c = new())
        {
            var r = c.GetAsync($"{BASE_URL}/spot/quotation/v3/lite-klines?symbol={symbol}&step=1").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement.GetProperty("data");
                foreach (var p in e.EnumerateArray())
                {
                    Kline k = new();

                    k.OpenTime = UnixTimeStampToDateTime(p[0]);
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
            var r = c.GetAsync($"{BASE_URL}/spot/v1/symbols").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement.GetProperty("data");
                JsonElement symbols = e.GetProperty("symbols");
                foreach (var p in symbols.EnumerateArray())
                {
                    Product product = new();
                    product.exchange = ID;

                    string sym = p.GetString() + "";
                    string[] baseQuote = sym.Split('_');

                    product.symbol = sym;
                    product.baseasset = baseQuote[0];
                    product.quoteasset = baseQuote[1];
                    product.IsTradingEnabled = true;

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

    public override CoinDetails GetCoinDetails(string baseAsset)
    {
        CoinDetails cd = new();
        using (HttpClient c = new())
        {
            string uri = $"{BASE_URL}/account/v1/currencies";
            var req = new HttpRequestMessage(HttpMethod.Get, uri);

            var r = c.SendAsync(req).Result; 
            if (r.IsSuccessStatusCode)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                JsonElement data = e.GetProperty("data");
                JsonElement ccs = data.GetProperty("currencies");
                foreach (var p in ccs.EnumerateArray())
                {
                    cd.asset = p.GetProperty("currency").GetString() + "";
                    cd.exchId = ID;
                    cd.network = p.GetProperty("network").GetString() + "";
                    cd.address = p.GetProperty("contract_address").GetString() + "";
                    cd.longName = p.GetProperty("name").GetString() + "";
                }
            }
        }
        return cd;
    }
}
