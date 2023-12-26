using Kraken.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using Kraken.Net.Objects.Models;

namespace caLibProdStat;

public class Kraken : AnExchange
{
    public override int ID => 7;
    public override string Name => "Kraken";

    KrakenRestClient client = new();

    protected override Product ToProduct(object p)
    {
        KeyValuePair<string, KrakenSymbol> krakenProd = (KeyValuePair<string, KrakenSymbol>)p;

        Product product = new();
        product.symbol = krakenProd.Key;
        product.exchange = ID;
        product.baseasset = krakenProd.Value.BaseAsset;
        product.quoteasset = krakenProd.Key.Replace(product.baseasset, "");

        product.IsTradingEnabled = krakenProd.Value.Status == "online";

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
