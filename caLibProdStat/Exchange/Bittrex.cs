using Bittrex.Net.Clients;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Bittrex : AnExchange
{
    public override int ID => 4;
    public override string Name => "Bittrex";

    BittrexClient client = new();

    protected override Product ToProduct(object p)
    {
        BittrexSymbol bittProd = (BittrexSymbol)p;

        Product product = new();
        product.symbol = bittProd.Name;
        product.exchange = ID;
        product.baseasset = bittProd.BaseAsset;
        product.quoteasset = bittProd.QuoteAsset;

        product.IsTradingEnabled = bittProd.Status == SymbolStatus.Online;

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

        int m = CaInfo.KlineInterval % 60;
        int h = (CaInfo.KlineInterval - m) / 60;
        TimeSpan klInterval = new TimeSpan(h, m, 0);

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval).Result;

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
