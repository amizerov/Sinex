using OKX.Net.Clients;
using OKX.Net.Enums;
using OKX.Net.Objects.Public;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using System.Net;
using System.Text.Json;

namespace caLibProdStat;

public class OKX : AnExchange
{
    public override int ID => 8;
    public override string Name => "OKX";

    OKXRestClient client = new();

    protected override Product ToProduct(object p)
    {
        OKXInstrument okxProd = (OKXInstrument)p;

        Product product = new();
        product.symbol = okxProd.Symbol;
        product.exchange = ID;
        product.baseasset = okxProd.BaseAsset;
        product.quoteasset = okxProd.QuoteAsset;

        product.IsTradingEnabled = okxProd.state == OKXInstrumentState.Live;

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        var r = client.UnifiedApi.ExchangeData.GetSymbolsAsync(OKXInstrumentType.Spot).Result;
        if (r.Success)
        {
            Log.Info(ID, "GetProducts", "start");
            foreach (var p in r.Data)
            {
                Product product = ToProduct(p);
                if (product.IsTradingEnabled)
                {
                    products.Add(product);
                }
            }
            Log.Info(ID, $"GetProducts({Name})", "got " + products.Count);
        }
        else
        {
            string err = r.Error!.Message;
            Log.Error(ID, $"GetProducts)", err);
        }
        return products;
    }
    protected override List<Kline> GetLastKlines(string symbol)
    {
        List<Kline> klines = new List<Kline>();

        var r = client.UnifiedApi.ExchangeData
            .GetKlinesAsync(symbol, OKXPeriod.OneMinute).Result;

        if (r.Success)
        {
            foreach (var p in r.Data)
            {
                Kline k = new();

                k.OpenTime = p.Time;
                k.OpenPrice = p.OpenPrice;
                k.HighPrice = p.HighPrice;
                k.LowPrice = p.LowPrice;
                k.ClosePrice = p.ClosePrice;
                k.Volume = p.Volume;

                klines.Add(k);
            }
        }
        else
        {
            Log.Error(ID, $"GetProductStat({symbol})", r.Error!.Message);
        }
        return klines;
    }

    public override CoinDetails GetCoinDetails(string asset)
    {
        CoinDetails cd = new();
        /*using (HttpClient c = new())
        {
            string uri = $"https://www.okx.com/api/v5/asset/currencies?ccy={asset}";
            var r = new HttpRequestMessage(HttpMethod.Get, uri);
            r.Headers.Add()

            var r = c.SendAsync(r); 
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;

            }
        }*/
        return cd;
    }
}