using CoinEx.Net.Clients;
using CoinEx.Net.Enums;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CoinEx.Net.Objects.Models;
using CryptoExchange.Net;

namespace caLibProdStat;

public class CoinEx : AnExchange
{
    public override int ID => 9;
    public override string Name => "CoinEx";

    CoinExRestClient client = new();

    protected override Product ToProduct(object p)
    {
        KeyValuePair<string, CoinExSymbol> coinExProd = (KeyValuePair<string, CoinExSymbol>)p;

        Product product = new();
        product.symbol = coinExProd.Key;
        product.exchange = ID;
        product.baseasset = coinExProd.Value.TradingName;
        product.quoteasset = coinExProd.Value.Name.Replace(product.baseasset, "");

        product.IsTradingEnabled = true; // coinExProd.Value. == CoinExInstrumentState.Live;

        return product;
    }
    protected override List<Product> GetProducts()
    {
        List<Product> products = new List<Product>();
        
        var r = client.SpotApi.ExchangeData.GetSymbolInfoAsync().Result;
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

        var r = client.SpotApi.ExchangeData
            .GetKlinesAsync(symbol, KlineInterval.OneMinute).Result;

        if (r.Success)
        {
            foreach (var p in r.Data)
            {
                Kline k = new();
                
                k.OpenTime = p.OpenTime;
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

    public override CoinDetails GetCoinDetails(string baseAsset)
    {
        CoinDetails cd = new();
        return cd;
    }
}
