using amLogger;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace CaExch2;

public class CaAscendEx : AnExchange
{
    public override int ID => 13;
    public const string BASE_URL = "https://ascendex.com";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + "/" + quoteAsset;
    }
    public override async Task<CaOrderBook> GetOrderBook(string symbol)
    {
        CaOrderBook orderBook = new(symbol);
        using HttpClient c = new();
        var r = await c.GetAsync($"{BASE_URL}/api/pro/v1/depth?symbol={symbol}");
        var s = await r.Content.ReadAsStringAsync();

        JsonDocument j = JsonDocument.Parse(s);
        JsonElement e = j.RootElement;
        var data = e.GetProperty("data");
        var asks = data.GetProperty("asks");
        var bids = data.GetProperty("bids");
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
    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        Ticker t = new();
        using (HttpClient c = new())
        {
            var res = await c.GetAsync($"{BASE_URL}/api/pro/v1/spot/ticker?symbol={symbol}");

            if (!res.IsSuccessStatusCode) return t;
            try
            {
                var s = res.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement r = j.RootElement.GetProperty("data");

                t.Symbol = r.GetProperty("symbol").GetString()!;

                decimal last = sd(r.GetProperty("close"));

                decimal bid_sz = sd(r.GetProperty("bid")[1]);
                decimal bid_px = sd(r.GetProperty("bid")[0]);

                decimal ask_sz = sd(r.GetProperty("ask")[1]);
                decimal ask_px = sd(r.GetProperty("ask")[0]);

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

    public override Task<Order> GetLastSpotOrder(string symbol)
    {
        throw new NotImplementedException();
    }

    public override void SubscribeToSpotAccountUpdates()
    {
        throw new NotImplementedException();
    }
}
