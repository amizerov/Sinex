using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using static System.Text.Json.JsonElement;
using System.Net;
using System.Text.Json;

namespace CaExch2;

public class CaBitMart : AnExchange
{
    public override int ID => 12;
    public override string Name => "BitMart";

    public override ISymbolOrderBook OrderBook => throw new NotImplementedException();

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
        using (HttpClient c = new())
        {
            var res = await c.GetAsync($"https://api-cloud.bitmart.com/spot/quotation/v3/ticker?symbol={symbol}");

            if (res.StatusCode == HttpStatusCode.OK)
            {
                var s = res.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement r = j.RootElement.GetProperty("data");

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
