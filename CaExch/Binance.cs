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
    
    BinanceClient client = new();
    BinanceSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        List<Kline> klines = new();
        TimeSpan klInterval = GetIntervalFromString(inter);

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval).Result;

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
        await socketClient.UnsubscribeAllAsync();

        TimeSpan ts = GetIntervalFromString(inter);
        KlineInterval interval = (KlineInterval)(
            ts.Days * 24 * 60 * 60 + 
            ts.Hours * 60 * 60 +
            ts.Minutes * 60 +
            ts.Seconds);
        
        var r = await socketClient.SpotStreams.
            SubscribeToKlineUpdatesAsync(symbol, interval, 
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
            Log.Info(ID, "SocketSubscribe", $"subscribed to {symbol}, {inter}");
        }
        else
        {
            Log.Error(ID, $"SocketSubscribe({symbol})", ""+r.Error?.Message);
        }
    }
}
