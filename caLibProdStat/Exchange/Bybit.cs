using Bybit.Net.Clients;
using Bybit.Net.Enums;
using Bybit.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using Bybit.Net.Objects.Models.Spot.v3;

namespace caLibProdStat;

public class Bybit : AnExchange
{
    public override int ID => 5;
    public override string Name => "Bybit";

    BybitRestClient client = new();

    protected override Product ToProduct(object p)
    {
        BybitSpotSymbolV3 bittProd = (BybitSpotSymbolV3)p;

        Product product = new();
        product.symbol = bittProd.Name;
        product.exchange = ID;
        product.baseasset = bittProd.BaseAsset;
        product.quoteasset = bittProd.QuoteAsset;

        product.IsTradingEnabled = bittProd.ShowStatus == "1";

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();

        var r = client.SpotApiV3.ExchangeData.GetSymbolsAsync().Result;
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

        var r = client.SpotApiV3.CommonSpotClient
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
