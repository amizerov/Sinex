﻿using amLogger;
using Kucoin.Net.Clients;
using CryptoExchange.Net.CommonObjects;
using Kucoin.Net.Enums;
using Kucoin.Net.Objects.Models.Spot;
using Kucoin.Net.SymbolOrderBooks;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Authentication;
using CaSecrets;
using Kucoin.Net.Objects;

namespace CaExch2;
public class CaKucoin : AnExchange
{
    public override int ID => 2;
    public override string Name => "Kucoin";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + "-" + quoteAsset;
    }
    public override async Task<CaOrderBook> GetOrderBook(string symbol) 
    {
        CaOrderBook orderBook = new(symbol);
        var ob = new KucoinSpotSymbolOrderBook(symbol, (options) =>
        {
            options.ApiCredentials = new KucoinApiCredentials(
                Secrets.KucoinApiKey, Secrets.KucoinApiSecret, Secrets.KucoinPassPhrase
            );
        });
        var r = await ob.StartAsync();
        if(!r.Success) return orderBook;

        foreach (var b in ob.Asks)
        {
            orderBook.Asks.Add(new OrderBookEntry() { Price = b.Price, Quantity = b.Quantity });
        }
        foreach (var b in ob.Bids)
        {
            orderBook.Bids.Add(new OrderBookEntry() { Price = b.Price, Quantity = b.Quantity });
        }

        return orderBook;
    }
    string _symbol = "";
    public override List<string> Intervals => new List<string>()
        { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d", "1w" };

    KucoinRestClient restClient = new();
    KucoinSocketClient socketClient = new();

    public override async Task<bool> CheckApiKey()
    {
        await Task.Delay(10);
        return false;
    }
    public override async Task<List<Balance>> GetBalances()
    {
        List<Balance> balances = new();
        var res = await restClient.SpotApi.Account.GetAccountsAsync();
        if (res.Success)
        {
            var bals = res.Data.ToList();
            foreach (var b in bals)
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

    public async override Task<List<Kline>> GetKlines(string symbol, string inter, int count = 0)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        var r = await restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter)));

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
    protected async override Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter)
    {
        var r = await socketClient.SpotApi.
            SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)IntervalInSeconds(inter),
                msg =>
                {
                    KucoinKline k = msg.Data.Candles;

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
    public override Task<bool> PlaceSpotOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> PlaceSpotOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> FutuOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override Task<bool> FutuOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();
    }

    public override void SubscribeToSpotAccountUpdates()
    {
        throw new NotImplementedException();
    }

    public override Task<int> SubsсribeToTicker(string symbol)
    {
        throw new NotImplementedException();
    }

    public override Task<Order> GetLastSpotOrder(string symbol)
    {
        throw new NotImplementedException();
    }

    public override void UnSubFromTicker(int subsId)
    {
        throw new NotImplementedException();
    }
}
