using amLogger;
using Huobi.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using Huobi.Net.Enums;
using Huobi.Net.Objects.Models;
using Huobi.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces;

namespace CaExch;
public class CaHuobi : AnExchange
{
    public override int ID => 3;
    public override string Name => "Huobi";

    public override ISymbolOrderBook OrderBook => new HuobiSpotSymbolOrderBook(_symbol);
    string _symbol = "";
    public override List<string> Intervals => new List<string>()
        { "1m", "5m", "15m", "30m", "1h", "2h", "4h", "1w", "1M", "1y" };

    HuobiClient restClient = new();
    HuobiSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        var r = restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter))).Result;

        if (r.Success)
        {
            klines = r.Data.ToList(); klines.Reverse();
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

        var r = await socketClient.SpotStreams.
            SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)IntervalInSeconds(inter),
                msg =>
                {
                    HuobiKline k = msg.Data;

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
