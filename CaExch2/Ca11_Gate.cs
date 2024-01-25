using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;
using static System.Text.Json.JsonElement;

namespace CaExch2;

public class CaGate : AnExchange
{
    public override int ID => 11;
    public override string Name => "Gate";

    public override ISymbolOrderBook OrderBook => throw new NotImplementedException();

    public override List<string> Intervals => new List<string>()
    { "10s", "1m", "5m", "15m", "30m", "1h", "4h", "8h", "1d", "7d", "30d" };

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
            var res = await c.GetAsync($"https://api.gateio.ws/api/v4/spot/tickers?currency_pair={symbol}");

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var s = res.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement r = j.RootElement;
                ArrayEnumerator a = r.EnumerateArray();
                a.MoveNext();
                JsonElement e = a.Current;

                t.Symbol = e.GetProperty("currency_pair").GetString()!;
                decimal p1 = sd(e.GetProperty("lowest_ask"));
                decimal p2 = sd(e.GetProperty("highest_bid"));
                decimal v = sd(e.GetProperty("quote_volume"));

                t.HighPrice = p1;
                t.LowPrice = p2;
                t.LastPrice = (p1 + p2) / 2;
                t.Volume = v;
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
