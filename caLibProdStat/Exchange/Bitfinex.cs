using Bitfinex.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using Bitfinex.Net.Objects.Models;
using Bitfinex.Net.Enums;

namespace caLibProdStat;

public class Bitfinex : AnExchange
{
    public override int ID => 6;
    public override string Name => "Bitfinex";

    BitfinexRestClient client = new();

    protected override Product ToProduct(object p)
    {
        string bitfProd = (string)p;
        //BitfinexSymbolOverview pp = new();

        Product product = new();
        string s = product.symbol = bitfProd;
        product.exchange = ID;
        product.baseasset = s.Remove(s.Length - 3);
        product.quoteasset = s.Remove(0, s.Length - 3);

        product.IsTradingEnabled = true; //bitfProd.Status == SymbolStatus.Online;

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        var r = client.SpotApi.ExchangeData.GetSymbolNamesAsync(SymbolType.Exchange).Result;
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
        Task.Delay(3000).Wait();

        List<Kline> klines = new List<Kline>();
        symbol = "t" + symbol.ToUpper();

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromMinutes(CaInfo.KlineInterval)).Result;

        if (r.Success)
        {
            klines = r.Data.ToList();
        }
        else
        {
            Log.Error(ID, $"GetProductStat({symbol})", r.Error!.Message);
        }
        return klines;
    }

    public override CoinDetails GetCoinDetails(string baseAsset)
    {
        CoinDetails cd = new();
        return cd;
    }
}
