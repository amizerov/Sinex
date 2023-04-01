using amLogger;
using Bittrex.Net.SymbolOrderBooks;
using Bittrex.Net.Clients;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;

namespace CaExch;
public class CaBittrex : AnExchange
{
    public override int ID => 4;
    public override string Name => "Bittrex";

    public override ISymbolOrderBook OrderBook => new BittrexSymbolOrderBook(_symbol);
    string _symbol = "";

    public override List<string> Intervals => new List<string>() 
        { "1m", "5m", "1h", "1d" };

    BittrexClient restClient = new();
    BittrexSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        try
        {
            var r = restClient.SpotApi.CommonSpotClient
                .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter))).Result;

            if (r.Success)
            {
                klines = r.Data.ToList();
                Log.Info(ID, $"GetKlines({symbol})", $"{klines.Count} klines loaded");
            }
            else
            {
                Log.Error(ID, $"GetKlines({symbol})", "Error: " + r.Error?.Message);
            }
        }
        catch(Exception ex)
        {
            Log.Error(ID, $"GetKlines({symbol})", "Exception: " + ex.Message);
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
                BittrexKline k = msg.Data.Delta;

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
