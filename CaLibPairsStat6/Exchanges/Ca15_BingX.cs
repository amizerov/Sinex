using amLogger;
using CryptoExchange.Net.CommonObjects;
using System.Net;
using System.Text.Json;

namespace caLibPairsStat6;

public class BingX : AnExchange
{
    public override int ID => 15;
    public const string BASE_URL = "https://open-api.bingx.com";

    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        using (HttpClient c = new())
        {
            var r = c.GetAsync($"{BASE_URL}/openApi/spot/v1/common/symbols").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement data = j.RootElement.GetProperty("data");
                JsonElement symbols = data.GetProperty("symbols");
                foreach (var p in symbols.EnumerateArray())
                {
                    try
                    {
                        Product product = new();
                        product.exchange = ID;

                        string sym = p.GetProperty("symbol") + "";
                        string[] baseQuote = sym.Split('-');

                        product.symbol = sym;
                        product.baseasset = baseQuote[0];
                        product.quoteasset = baseQuote[1];
                        product.IsTradingEnabled = p.GetProperty("status").ToString() == "1";

                        products.Add(product);
                    }
                    catch (Exception e)
                    {
                        Log.Error(ID, $"{Name} - GetProduct", $"Error: {e.Message}");
                    }
                }
            }
            else
            {
                Log.Error(ID, "GetProduct", $"Request failed whith code: {r.StatusCode}");
            }
        }

        return products;
    }

    protected override List<Kline> GetLastKlines(string symbol)
    {
        List<Kline> klines = new();

        using (HttpClient c = new())
        {
            long timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var r = c.GetAsync($"{BASE_URL}/openApi/spot/v2/market/kline?symbol={symbol}&interval=1m&timestamp={timestamp}").Result;
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement.GetProperty("data");
                foreach (var p in e.EnumerateArray())
                {
                    try
                    {
                        Kline k = new();

                        k.OpenTime = UnixTimeStampToDateTime(p[0]);
                        k.OpenPrice = sd(p[1]);
                        k.HighPrice = sd(p[2]);
                        k.LowPrice = sd(p[3]);
                        k.ClosePrice = sd(p[4]);
                        k.Volume = sd(p[7]);

                        klines.Add(k);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ID, $"{Name} - GetLastKlines({symbol})", $"Error: {ex.Message}");
                    }
                }
            }
            else
            {
                Log.Error(ID, $"{Name} - GetLastKlines({symbol})", $"Request failed with code: {r.StatusCode}");
            }
        }
        return klines;
    }

    decimal sd(JsonElement j)
    {
        decimal d = 0;
        string s = j+"";
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
    public DateTime UnixTimeStampToDateTime(JsonElement j)
    {
        DateTime dateTime = new DateTime();
        try
        {
            double unixTimeStamp = double.Parse(j + "") / 1000;
            // Unix timestamp is seconds past epoch
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }
        catch (Exception e)
        {
            Log.Error(ID, "UnixTimeStampToDateTime", $"Error: {e.Message}");
        }
        return dateTime;
    }
}
