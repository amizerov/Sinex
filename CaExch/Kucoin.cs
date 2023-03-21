using amLogger;
using Kucoin.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using Kucoin.Net.Enums;

namespace CaExch;
public class Kucoin : AnExchange
{
    public override int ID => 2;
    public override string Name => "Kucoin";

    KucoinClient client = new();
    KucoinSocketClient socketClient = new();

    public override List<Kline> GetKlines(string symbol, string inter)
    {
        List<Kline> klines = new();
        TimeSpan klInterval = GetIntervalFromString(inter);

        var r = client.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, klInterval).Result;

        if (r.Success)
        {
            klines = r.Data.ToList(); klines.Reverse();
            Logger.Write(new Log() { msg = $"{Name}({symbol}) {klines.Count} klines loaded" });
        }
        else
        {
            Logger.Write(new Log() { msg = $"{Name}({symbol}) Error: {r.Error?.Message}" });
        }
        return klines;
    }

    async public override void SocketSubscribe(string symbol, string inter)
    {
        KlineInterval interval = (KlineInterval)GetKlineIntervalFromString(inter);
        var r = await socketClient.SpotStreams.
            SubscribeToKlineUpdatesAsync(symbol, interval,
                msg =>
                {

                });
    }
}
