using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Binance : AnExchange
{
    public override int ID => 1;
    public override string Name => "Binance";

    BinanceClient client = new();

    protected override Product ToProduct(object p)
    {
        BinanceProduct binaProd = (BinanceProduct)p;

        Product product = new();
        product.exchange = ID;
        product.symbol = binaProd.Symbol.ToUpper().Replace("-", "");
        product.baseasset = binaProd.BaseAsset.ToUpper();
        product.quoteasset = binaProd.QuoteAsset.ToUpper();

        product.IsTradingEnabled = binaProd.Status == "TRADING";

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        
        var r = client.SpotApi.ExchangeData.GetProductsAsync().Result;
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
