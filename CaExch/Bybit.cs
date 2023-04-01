using amLogger;
using Bybit.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using Bybit.Net.Enums;
using Bybit.Net.Objects.Models.Socket.Spot;
using Bybit.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces;

namespace CaExch;
public class CaBybit : AnExchange
{
    public override int ID => 5;
    public override string Name => "Bybit";

    public override ISymbolOrderBook OrderBook => new BybitSpotSymbolOrderBook(_symbol);
    string _symbol = "";
    public override List<string> Intervals => new List<string>()
        { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "12h", "1d", "1w", "1M" };

    BybitClient restClient = new();
    BybitSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        var r = restClient.SpotApiV3.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter))).Result;

        if (r.Success)
        {
            klines = r.Data.ToList(); //klines.Reverse();
            Log.Info(ID, $"GetKlines({symbol})", $"{klines.Count} klines loaded");
        }
        else
        {
            Log.Error(ID, $"GetKlines({symbol})", "" + r.Error?.Message);
        }
        return klines;
    }

    async public override void SubscribeToSocket(string symbol, string inter)
    {
        Log.Info(ID, "SubscribeToSocket", $"Begin subscribe {symbol}, Interval = {inter}");

        var r = await socketClient.SpotStreamsV3.
            SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)IntervalInSeconds(inter),
                msg =>
                {
                    BybitSpotKlineUpdate k = msg.Data;

                    Kline kline = new Kline();
                    kline.HighPrice = k.HighPrice;
                    kline.LowPrice = k.LowPrice;
                    kline.OpenPrice = k.OpenPrice;
                    kline.ClosePrice = k.ClosePrice;
                    kline.Volume = k.Volume;
                    kline.OpenTime = k.OpenTime;

                    SendKline(ID, symbol, kline);
                    Log.Info(ID, "qqq", $"{symbol} {k.OpenTime} {k.ClosePrice}");
                });

        if (r.Success)
        {
            int c3 = socketClient.CurrentSubscriptions;
            Log.Info(ID, "SocketSubscribe", $"subscribed to {symbol}, {inter}, {c3}");
        }
        else
        {
            Log.Error(ID, $"SocketSubscribe({symbol})", "" + r.Error?.Message);
        }
    }
    public override async void Unsub()
    {
        int c1 = socketClient.CurrentSubscriptions;
        await socketClient.UnsubscribeAllAsync();
        int c2 = socketClient.CurrentSubscriptions;
        Log.Info(ID, "UnsubscribeAllAsync", $"{c1} unsubed, left {c2}");
    }
}
