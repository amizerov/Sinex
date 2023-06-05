using amLogger;
using Bittrex.Net.SymbolOrderBooks;
using Bittrex.Net.Clients;
using Bittrex.Net.Enums;
using Bittrex.Net.Objects.Models;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Objects;
using Binance.Net.Objects.Models.Spot;

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

    public override async Task<bool> CheckApiKey()
    {
        await Task.Delay(10);
        return false;
    }
    public override async Task<List<Balance>> GetBalances()
    {
        List<Balance> balances = new();
        var res = await restClient.SpotApi.Account.GetBalancesAsync();
        if (res.Success)
        {
            var bals = res.Data.ToList();
            foreach(var b in bals)
            {
                balances.Add(new Balance() { Asset = b.Asset, Available = b.Available, Total = b.Total });
            }
        }
        else
        {
            Console.WriteLine("GetBalances - " +
                $"Error GetAccountInfoAsync: {res.Error?.Message}");
        }
        return balances;
    }

    public override async Task<Ticker> GetTickerAsync(string symbol)
    {
        var r = await restClient.SpotApi.CommonSpotClient.GetTickerAsync(symbol);
        return r.Data;
    }

    public async override Task<List<Kline>> GetKlines(string symbol, string inter)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        try
        {
            var r = await restClient.SpotApi.CommonSpotClient
                .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter)));

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

    protected async override Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter)
    {
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

                KlineUpdated(symbol, kline);
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

    /*
     * Trading
     */
    public override void SpotOrderBuy(decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void SpotOrderSell(decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void FutuOrderBuy(decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void FutuOrderSell(decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void SubscribeToSpotAccountUpdates()
    {
        throw new NotImplementedException();
    }

    public override void SubsсribeToTicker(string symbol)
    {
        throw new NotImplementedException();
    }

    public override Task<Order> GetLastSpotOrder(string symbol)
    {
        throw new NotImplementedException();
    }
}
