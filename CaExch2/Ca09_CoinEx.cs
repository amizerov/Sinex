using amLogger;
using CoinEx.Net.Clients;
using CoinEx.Net.Enums;
using CoinEx.Net.Interfaces;
using CoinEx.Net.Objects;
using CoinEx.Net.SymbolOrderBooks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CaSecrets;
using CryptoExchange.Net.Authentication;
using CoinEx.Net.Objects.Models;

namespace CaExch2;
public class CaCoinEx : AnExchange
{
    public override int ID => 9;
    public override string Name => "CoinEx";
    public override string ValidateSymbol(string baseAsset, string quoteAsset)
    {
        return baseAsset + quoteAsset;
    }
    public override async Task<CaOrderBook> GetOrderBook(string symbol)
    {
        CaOrderBook orderBook = new(symbol);
        var ob = new CoinExSpotSymbolOrderBook(_symbol);
        await ob.StartAsync();

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
        { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "12h", "1d", "3d", "1w" };

    CoinExRestClient restClient = new();
    CoinExSocketClient socketClient = new();

    public override async Task<bool> CheckApiKey()
    {
        bool res = false;
        try
        {
            string apiKey = Secrets.CoinExApiKey;
            string apiSecret = Secrets.CoinExApiSecret;

            restClient = new CoinExRestClient(options =>
                {
                    options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
                });

            // Если получен доступ к балансам, ключ считается рабочим
            List<Balance> bs = await GetBalances();
            res = bs.Count > 0;

            Console.WriteLine($"CheckApiKey - Key.IsWorking: {res}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Init api key - Error: {ex.Message}");
        }
        return res;
    }
    public override async Task<List<Balance>> GetBalances()
    {
        List<Balance> balances = new();
        var res = await restClient.SpotApi.Account.GetBalancesAsync();
        if (res.Success)
        {
            var bals = res.Data.ToList();
            foreach( var b in bals )
            {
                balances.Add(new Balance() { Asset = b.Value.Asset, Available = b.Value.Available, Total = b.Value.Available + b.Value.Frozen });
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
        try
        {
            var r = await restClient.SpotApi.CommonSpotClient.GetTickerAsync(symbol);
            return r.Data;
        }
        catch (Exception ex)
        {
            Log.Error(ID, $"GetTicker({symbol})", ex.Message);
            return new Ticker();
        }
    }

    public async override Task<List<Kline>> GetKlines(string symbol, string inter, int count)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        var r = await restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter)), limit: count);

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
        var r = await socketClient.SpotApi.
            SubscribeToKlineUpdatesAsync(symbol, (KlineInterval)IntervalInSeconds(inter),
            msg => 
            {
                foreach (CoinExKline k in msg.Data)
                {

                    Kline kline = new Kline();
                    kline.HighPrice = k.HighPrice;
                    kline.LowPrice = k.LowPrice;
                    kline.OpenPrice = k.OpenPrice;
                    kline.ClosePrice = k.ClosePrice;
                    kline.Volume = k.Volume;
                    kline.OpenTime = k.OpenTime;

                    KlineUpdated(symbol, kline);
                    Log.Info(ID, "qqq", $"{symbol} {k.OpenTime} {k.ClosePrice}");
                }
            });

        return r;
    }

    public async override Task<int> SubsсribeToTicker(string symbol)
    {
        var res = await socketClient.SpotApi.
            SubscribeToTickerUpdatesAsync(symbol,
            onMessage => {
                Ticker t = new();
                var d = onMessage.Data;
                t.HighPrice = d.HighPrice;
                t.LowPrice = d.LowPrice;
                t.LastPrice = d.LastPrice;
                t.Symbol = d.Symbol;
                t.Volume = d.Volume;
                //t.Price24H = d.;
                TickerUpdated(t);
            });
        if (!res.Success)
        {
            Log.Error(Name, $"Error in SubsсribeToTicker: {res.Error?.Message}");
            return 0;
        }
        return res.Data.Id;
    }
    public async override void UnSubFromTicker(int subsId)
    {
        int c1 = socketClient.CurrentSubscriptions;
        await socketClient.UnsubscribeAsync(subsId);
        int c2 = socketClient.CurrentSubscriptions;

        if (c1 == c2)
        {
            Log.Error(Name, $"Error in UnSubFromTicker - не отписался");
        }
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
    public async override Task<bool> PlaceSpotOrderBuy(string symbol, decimal quantity)
    {
        Log.Trace(Name, "spot buy " + symbol);
        var res = await restClient.SpotApi.Trading.PlaceOrderAsync(
            symbol!,
            OrderSide.Buy,
            OrderType.Market, quantity);
        if (!res.Success)
        {
            Log.Error(Name, $"Error in SpotOrderBuy: {res.Error?.Message}");
            return false;
        }
        return true;
    }
    public async override Task<bool> PlaceSpotOrderSell(string symbol, decimal quantity)
    {
        Log.Trace(Name, "spot sell " + symbol);
        var res = await restClient.SpotApi.Trading.PlaceOrderAsync(
            symbol,
            OrderSide.Sell,
            OrderType.Market, quantity);
        if(!res.Success)
        {
            Log.Error(Name, $"Error in SpotOrderSell: {res.Error?.Message}");
            return false;
        }
        return true;
    }
    public override Task<bool> FutuOrderBuy(string symbol, decimal quantity)
    {
        throw new NotImplementedException();

    }
    public override Task<bool> FutuOrderSell(string symbol, decimal quantity)
    {
        throw new NotImplementedException();

    }

    /*
     *  Account
     */

    string GetListenKey(bool spotMarg = true)
    {
        throw new NotImplementedException();
    }

    public async override Task<Order> GetLastSpotOrder(string symbol)
    {
        Order? order = new() { Symbol = symbol };
        var res = await restClient.SpotApi.Trading.GetOpenOrdersAsync(symbol);
        if (res.Success)
        {
            var ord = res.Data.Data
                .Where(o => o.Status == OrderStatus.Executed && o.Side == OrderSide.Buy)
                .MaxBy(o => o.CreateTime);
            if (ord != null)
            {
                order.Status = (CommonOrderStatus)ord.Status;
                order.QuantityFilled = ord.QuantityFilled;
                order.Quantity = ord.Quantity;
                order.Price = ord.Price;
                order.Side = (CommonOrderSide)ord.Side;
                order.Timestamp = ord.CreateTime;
            }
        }
        else
        {
            Log.Error(Name, $"GetSpotOrders failed - {res.Error?.Message}");
        }
        return order;
    }

    public override void SubscribeToSpotAccountUpdates()
    {
        throw new NotImplementedException();

    }
}
