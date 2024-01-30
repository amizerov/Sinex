using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace CaExch2;

public class CaMexc : AnExchange
{
    public override int ID => 10;
    public override string Name => "Mexc";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + quoteAsset;
    }
    public override CaOrderBook GetOrderBook(string symbol)
    {
        CaOrderBook orderBook = new(symbol);
        using HttpClient c = new();
        var r = c.GetAsync($"https://api.mexc.com/api/v3/depth?symbol={symbol}").Result;
        var s = r.Content.ReadAsStringAsync().Result;
        JsonDocument j = JsonDocument.Parse(s);
        JsonElement e = j.RootElement;
        var asks = e.GetProperty("asks");
        var bids = e.GetProperty("bids");
        foreach (var a in asks.EnumerateArray())
        {
            decimal p = sd(a[0]);
            decimal q = sd(a[1]);
            orderBook.Asks.Add(new OrderBookEntry() { Price = p, Quantity = q });
        }
        foreach (var b in bids.EnumerateArray())
        {
            decimal p = sd(b[0]);
            decimal q = sd(b[1]);
            orderBook.Bids.Add(new OrderBookEntry() { Price = p, Quantity = q });
        }
  
        return orderBook;
    }
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
}
