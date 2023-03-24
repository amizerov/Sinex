using amLogger;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using CryptoExchange.Net.CommonObjects;

namespace CaExch;
public class Binance : AnExchange
{
    public override int ID => 1;
    public override string Name => "Binance";
    
    BinanceClient restClient = new();
    BinanceSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        List<Kline> klines = new();

        var r = restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter))).Result;

        if (r.Success)
        {
            klines = r.Data.ToList();
            Log.Info(ID, $"GetKlines({symbol})", $"{klines.Count} klines loaded");

            SocketSubscribe(symbol, inter);
        }
        else
        {
            Log.Error(ID, $"GetKlines({symbol})", ""+r.Error?.Message);
        }
        return klines;
    }

    async public override void SocketSubscribe(string symbol, string inter)
    {
        int c1 = socketClient.CurrentSubscriptions;
        await socketClient.UnsubscribeAllAsync();
        int c2 = socketClient.CurrentSubscriptions;

        var r = await socketClient.SpotStreams.
            SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)IntervalInSeconds(inter),
            msg => 
            {
                IBinanceStreamKline k = msg.Data.Data;

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
            Log.Info(ID, "SocketSubscribe", $"subscribed to {symbol}, {inter}, {c1}, {c2}, {c3}");
        }
        else
        {
            Log.Error(ID, $"SocketSubscribe({symbol})", ""+r.Error?.Message);
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
