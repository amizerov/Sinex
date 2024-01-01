using CoinEx.Net.Clients;
using CoinEx.Net.Enums;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CoinEx.Net.Objects.Models;
using CryptoExchange.Net;
using System.Text.Json;

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

    string baseAssetLast = "";
    public override Coin GetCoinDetails(string baseAsset)
    {
        if (baseAssetLast == baseAsset) return new Coin();
        baseAssetLast = baseAsset;

        var host = "https://api.coinex.com";
        var prefix = "/v1";
        var url = "/common/asset/config";

        using HttpClient clt = new();

        string uri = $"{host}{prefix}{url}?coin_type={baseAsset}";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        Coin cd = new();

        var r = clt.SendAsync(req).Result;
        if (r.IsSuccessStatusCode)
        {
            var s = r.Content.ReadAsStringAsync().Result;
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            var d = e.GetProperty("data");

            cd.exchId = ID;
            cd.asset = baseAsset;
               
            var p = d.GetProperty(baseAsset);
            cd.network = p.GetProperty("chain").GetString() + "";

            cd.Save().Wait();

            //Thread.Sleep(200);
        }

        return cd;
    }
}
