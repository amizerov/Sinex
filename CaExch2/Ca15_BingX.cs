using amLogger;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using System.Net;
using System.Text.Json;

namespace CaExch2;

public class CaBingX : AnExchange
{
    public override int ID => 15;
    public const string BASE_URL = "https://open-api.bingx.com";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + "-" + quoteAsset;
    }
    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        Ticker t = new();
        using (HttpClient c = new())
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var res = await c.GetAsync(
                $"{BASE_URL}/openApi/swap/v2/quote/ticker?symbol={symbol}&timestamp={timestamp}");

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var s = res.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement r = j.RootElement.GetProperty("data");

                t.Symbol = r.GetProperty("symbol").GetString()!;

                decimal last = sd(r.GetProperty("lastPrice"));

                decimal lastQty = sd(r.GetProperty("lastQty"));
                decimal lowPrice = sd(r.GetProperty("lowPrice"));
                decimal highPrice = sd(r.GetProperty("highPrice"));

                t.HighPrice = highPrice;
                t.LowPrice = lowPrice;
                t.LastPrice = last;
                t.Volume = lastQty;
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
