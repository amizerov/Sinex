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

    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        symbol = symbol + "USDT_SPBL";
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
