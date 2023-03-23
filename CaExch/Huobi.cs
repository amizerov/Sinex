using amLogger;
using Huobi.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using Huobi.Net.Enums;
using Huobi.Net.Objects.Models;

namespace CaExch;
public class Huobi : AnExchange
{
    public override int ID => 3;
    public override string Name => "Huobi";

    HuobiClient client = new();
    HuobiSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        List<Kline> klines = new();
        TimeSpan klInterval = GetIntervalFromString(inter);

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval).Result;

        if (r.Success)
        {
            klines = r.Data.ToList(); klines.Reverse();
            Log.Info(ID, $"GetKlines({symbol})", $"{klines.Count} klines loaded");

            SocketSubscribe(symbol, inter);
        }
        else
        {
            Log.Error(ID, $"GetKlines({symbol})", "" + r.Error?.Message);
        }
        return klines;
    }

    async public override void SocketSubscribe(string symbol, string inter)
    {
        await socketClient.UnsubscribeAllAsync();
        KlineInterval interval = (KlineInterval)GetIntervalFromString(inter).Seconds;
        var r = await socketClient.SpotStreams.
            SubscribeToKlineUpdatesAsync(symbol, interval,
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
            Log.Info(ID, "SocketSubscribe", $"subscribed to {symbol}, {inter}");
        }
        else
        {
            Log.Error(ID, $"SocketSubscribe({symbol})", "" + r.Error?.Message);
        }
    }
}
