using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace CaExch;

public class CaMexc : AnExchange
{
    public override int ID => 10;
    public override string Name => "Mexc";

    public override ISymbolOrderBook OrderBook => throw new NotImplementedException();

    public override List<string> Intervals => new List<string>()
    { "1m", "5m", "15m", "30m", "60m", "4h", "1d", "1M" };

    string _symbol = "";

    public override Task<bool> CheckApiKey()
    {
        throw new NotImplementedException();
    }

    public override Task<List<Balance>> GetBalances()
    {
        throw new NotImplementedException();
    }

    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        Ticker t = new();
        using (HttpClient c = new())
        {
            var r = await c.GetAsync($"https://api.mexc.com/api/v3/ticker/bookTicker?symbol={symbol}");

            if(r.StatusCode == HttpStatusCode.OK)
            {
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;

                t.Symbol = e.GetProperty("symbol").GetString()!;
                decimal p1 = sd(e.GetProperty("bidPrice"));
                decimal p2 = sd(e.GetProperty("askPrice"));
                decimal v1 = sd(e.GetProperty("bidQty"));
                decimal v2 = sd(e.GetProperty("askQty"));

                t.HighPrice = p1;
                t.LowPrice = p2;
                t.LastPrice = (p1 + p2)/2;
                t.Volume = (v1 + v2) / 2;
            }
        }
        return t;
    }

    public override Task<List<Kline>> GetKlines(string symbol, string inter, int count = 0)
    {
        throw new NotImplementedException();
    }

    public override Task<int> SubsсribeToTicker(string symbol)
    {
        throw new NotImplementedException();
    }

    public override void UnSubFromTicker(int subsId)
    {
        throw new NotImplementedException();
    }

    protected override Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter)
    {
        throw new NotImplementedException();
    }

    public override void UnsubKlineSocket(int subscriptionId)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> PlaceSpotOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> PlaceSpotOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> FutuOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> FutuOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<Order> GetLastSpotOrder(string symbol)
    {
        throw new NotImplementedException();
    }

    public override void SubscribeToSpotAccountUpdates()
    {
        throw new NotImplementedException();
    }
    decimal sd(JsonElement j)
    {
        decimal d = 0;
        string s = j.GetString()!;
        if (s.Contains("E"))
        {
            string[] p = s.Split("E");
            d = Decimal.Parse(p[0].Replace(".", ",")) * (decimal)Math.Pow(10, int.Parse(p[1]));
        }
        else
        {
            d = Decimal.Parse(s.Replace(".", ","));
        }
        return d;
    }
}
