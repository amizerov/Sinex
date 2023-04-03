using amLogger;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.SymbolOrderBooks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace CaExch;
public class CaBinance : AnExchange
{
    public override int ID => 1;
    public override string Name => "Binance";

    public override ISymbolOrderBook OrderBook => new BinanceSpotSymbolOrderBook(_symbol);
    string _symbol = "";

    public override List<string> Intervals => new List<string>() 
        { "1s", "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d", "3d", "1w", "1M" };

    BinanceClient restClient = new();
    BinanceSocketClient socketClient = new();

    public async override Task<List<Kline>> GetKlines(string symbol, string inter)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        var r = await restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter)));

        if (r.Success)
        {
            klines = r.Data.ToList();
            Log.Info(ID, $"GetKlines({symbol})", $"{klines.Count} klines loaded");
        }
        else
        {
            Log.Error(ID, $"GetKlines({symbol})", ""+r.Error?.Message);
        }
        return klines;
    }

    protected async override Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter)
    {
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

        return r;
    }
    public async override void UnsubKlineSocket(int subscriptionId)
    {
        int c1 = socketClient.CurrentSubscriptions;
        await socketClient.UnsubscribeAsync(subscriptionId);
        int c2 = socketClient.CurrentSubscriptions;
        Log.Info(ID, $"Unsubscribe({_symbol}, {subscriptionId})", $"before unsubed {c1}, left after {c2}");
    }
}
