using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using static System.Text.Json.JsonElement;
using System.Net;
using System.Text.Json;
using amLogger;

namespace CaExch2;

public class CaBitMart : AnExchange
{
    public override int ID => 12;
    public const string BASE_URL = "https://api-cloud.bitmart.com";
    public override string Name => "BitMart";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + "_" + quoteAsset;
    }
    public override async Task<CaOrderBook> GetOrderBook(string symbol)
    {
        CaOrderBook orderBook = new(symbol);
        using HttpClient c = new();
        var r = await c.GetAsync($"{BASE_URL}/spot/quotation/v3/books?symbol={symbol}");
        var s = await r.Content.ReadAsStringAsync();

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
    { "1m", "3m", "5m", "15m", "30m", "45m", "1h", "2h", "3h", "4h", "1d", "7d", "30d" };

    string _symbol = "";

    public override Task<bool> CheckApiKey()
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

    public override Task<List<Balance>> GetBalances()
    {
        throw new NotImplementedException();
    }

    public override Task<List<Kline>> GetKlines(string symbol, string inter, int count = 0)
    {
        throw new NotImplementedException();
    }

    public override Task<Order> GetLastSpotOrder(string symbol)
    {
        throw new NotImplementedException();
    }

    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        Ticker t = new();
        using HttpClient c = new();
        
        var res = await c.GetAsync($"https://api-cloud.bitmart.com/spot/quotation/v3/ticker?symbol={symbol}");

        if (res.StatusCode == HttpStatusCode.OK)
        {
            var s = res.Content.ReadAsStringAsync().Result;
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement r = j.RootElement.GetProperty("data");

            try
            {
                t.Symbol = r.GetProperty("symbol").GetString()!;

                decimal last = sd(r.GetProperty("last"));

                decimal bid_sz = sd(r.GetProperty("bid_sz"));
                decimal bid_px = sd(r.GetProperty("bid_px"));

                decimal ask_sz = sd(r.GetProperty("ask_sz"));
                decimal ask_px = sd(r.GetProperty("ask_px"));

                t.HighPrice = bid_px;
                t.LowPrice = ask_px;
                t.LastPrice = last;
                t.Volume = (ask_sz + bid_sz) * last / 2;
            }
            catch (Exception ex)
            {
                Log.Error(ID, "GetTicker", ex.Message);
            }
        }
        
        return t;
    }

    public override Task<bool> PlaceSpotOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> PlaceSpotOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void SubscribeToSpotAccountUpdates()
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

    public override void UnsubKlineSocket(int subscriptionId)
    {
        throw new NotImplementedException();
    }

    protected override Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter)
    {
        throw new NotImplementedException();
    }
}
