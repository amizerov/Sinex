using Huobi.Net.Enums;
using Huobi.Net.Clients;
using Huobi.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Huobi : AnExchange
{
    public override int ID => 3;
    public override string Name => "Huobi";

    HuobiRestClient client = new();

    protected override Product ToProduct(object p)
    {
        HuobiSymbol huobProd = (HuobiSymbol)p;

        Product product = new();
        product.symbol = huobProd.Name;
        product.exchange = ID;
        product.baseasset = huobProd.BaseAsset;
        product.quoteasset = huobProd.QuoteAsset;

        product.IsTradingEnabled = huobProd.State == SymbolState.Online;

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        var r = client.SpotApi.ExchangeData.GetSymbolsAsync().Result;
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
            Log.Info(ID, "GetProducts", "got " + products.Count);
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
}
