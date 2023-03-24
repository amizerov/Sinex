using Bybit.Net.Clients;
using Bybit.Net.Enums;
using Bybit.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using amLogger;

namespace caLibProdStat;

public class Bybit : AnExchange
{
    public override int ID => 5;
    public override string Name => "Bybit";

    BybitClient client = new();

    protected override Product ToProduct(object p)
    {
        BybitSymbol bittProd = (BybitSymbol)p;

        Product product = new();
        product.symbol = bittProd.Name;
        product.exchange = ID;
        product.baseasset = bittProd.BaseCurrency;
        product.quoteasset = bittProd.QuoteCurrency;

        product.IsTradingEnabled = bittProd.Status == SymbolStatus.Trading;

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
}
