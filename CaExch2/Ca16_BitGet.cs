using amLogger;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace CaExch2;

public class CaBitGet : AnExchange
{
    public override int ID => 16;
    public const string BASE_URL = "https://api.bitget.com";
    public override string Name => "BitGet";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + quoteAsset + "_SPBL";
    }
    public override async Task<CaOrderBook> GetOrderBook(string symbol)
    {
        CaOrderBook orderBook = new(symbol);
        using HttpClient c = new();
        var r = await c.GetAsync($"{BASE_URL}/api/spot/v1/market/depth?symbol={symbol}&type=step0");
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
            var res = await c.GetAsync(
                $"{BASE_URL}/api/spot/v1/market/ticker?symbol={symbol}");

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var s = res.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement r = j.RootElement.GetProperty("data");

                t.Symbol = r.GetProperty("symbol").GetString()!;

                decimal last = sd(r.GetProperty("close"));

                decimal usdtVol = sd(r.GetProperty("usdtVol"));
                decimal lowPrice = sd(r.GetProperty("low24h"));
                decimal highPrice = sd(r.GetProperty("high24h"));

                t.HighPrice = highPrice;
                t.LowPrice = lowPrice;
                t.LastPrice = last;
                t.Volume = usdtVol;
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
