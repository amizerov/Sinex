using amLogger;
using Binance.Net.Clients;
using Binance.Net.Enums;
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
