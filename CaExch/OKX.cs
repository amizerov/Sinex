using amLogger;
using OKX.Net.Clients;
using OKX.Net.Enums;
using OKX.Net.Interfaces;
using OKX.Net.Objects;
using OKX.Net.SymbolOrderBooks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CaSecrets;
using CryptoExchange.Net.Authentication;
using OKX.Net.Objects.Account;
using OKX.Net.Objects.Market;

namespace CaExch;
public class CaOKX : AnExchange
{
    public override int ID => 8;
    public override string Name => "OKX";

    public override ISymbolOrderBook OrderBook => new OKXSymbolOrderBook(_symbol);
    string _symbol = "";

    public override List<string> Intervals => new List<string>() 
        { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "12h", "1d", "1w", "1M" };

    OKXRestClient restClient = new();
    OKXSocketClient socketClient = new();

    public override async Task<bool> CheckApiKey()
    {
        bool res = false;
        try
        {
            string apiKey = Secrets.OKXApiKey;
            string apiSecret = Secrets.OKXApiSecret;

            restClient = new OKXRestClient(options =>
                {
                    options.ApiCredentials = new OKXApiCredentials(apiKey, apiSecret, "API-PASSPHRASE");
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
        var res = await restClient.UnifiedApi.Account.GetAccountBalanceAsync();
        if (res.Success)
        {
            OKXAccountBalance balance = res.Data;
            var bals = balance.Details.ToList();
            foreach (var b in bals)
            {
                balances.Add(new Balance() { Asset = b.Asset, Available = b.AvailableBalance, Total = b.CashBalance });
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
        var r = await restClient.UnifiedApi.ExchangeData.GetTickerAsync(symbol);
        Ticker t = new();
        t.HighPrice = r.Data.HighPrice;
        t.LowPrice = r.Data.LowPrice;
        t.LastPrice = r.Data.LastPrice;
        t.Symbol = r.Data.Symbol;
        t.Volume = r.Data.Volume;
        return t;
    }

    public async override Task<List<Kline>> GetKlines(string symbol, string inter, int count)
    {
        _symbol = symbol;
        List<Kline> klines = new();

        OKXPeriod period = OKXPeriodFromInterval(inter);

        if(count > 300) count = 300;

        var r = await restClient.UnifiedApi.ExchangeData
            .GetKlinesAsync(symbol, period, limit: count);

        if (r.Success)
        {
            var ks = r.Data.ToList();
            foreach (var k in ks)
            {
                Kline kline = new() { 
                    HighPrice = k.HighPrice, 
                    LowPrice = k.LowPrice, 
                    OpenPrice = k.OpenPrice, 
                    ClosePrice = k.ClosePrice, 
                    Volume = k.Volume, 
                    OpenTime = k.Time 
                };

                klines.Add(kline);
            }
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
        OKXPeriod period = OKXPeriodFromInterval(inter);
        var r = await socketClient.UnifiedApi.ExchangeData.
            SubscribeToKlineUpdatesAsync(symbol, period,
            msg => 
            {
                OKXCandlestick k = msg;

                Kline kline = new Kline();
                kline.HighPrice = k.HighPrice;
                kline.LowPrice = k.LowPrice;
                kline.OpenPrice = k.OpenPrice;
                kline.ClosePrice = k.ClosePrice;
                kline.Volume = k.Volume;
                kline.OpenTime = k.Time;

                KlineUpdated(symbol, kline);
                Log.Info(ID, "qqq", $"{symbol} {k.Time} {k.ClosePrice}");
            });

        return r;
    }
    OKXPeriod OKXPeriodFromInterval(string interval)
    {
        OKXPeriod per = OKXPeriod.OneMinute;
        switch(interval)
        {
            case "1m": per = OKXPeriod.OneMinute; break;
            case "3m": per = OKXPeriod.ThreeMinutes; break;
            case "5m": per = OKXPeriod.FiveMinutes; break;
            case "15m": per = OKXPeriod.FifteenMinutes; break;
            case "30m": per = OKXPeriod.ThirtyMinutes; break;
            case "1h": per = OKXPeriod.OneHour; break;
            case "2h": per = OKXPeriod.TwoHours; break;
            case "4h": per = OKXPeriod.FourHours; break;
            case "6h": per = OKXPeriod.SixHours; break;
            case "12h": per = OKXPeriod.TwelveHours; break;
            case "1d": per = OKXPeriod.OneDay; break;
            case "1w": per = OKXPeriod.OneWeek; break;
            case "1M": per = OKXPeriod.OneMonth; break;
        }

        return per;
    }
    public async override Task<int> SubsсribeToTicker(string symbol)
    {
        var res = await socketClient.UnifiedApi.ExchangeData.
            SubscribeToTickerUpdatesAsync(symbol,
            tiker => {
                Ticker t = new()
                {
                    HighPrice = tiker.HighPrice,
                    LowPrice = tiker.LowPrice,
                    LastPrice = tiker.LastPrice,
                    Symbol = tiker.Symbol,
                    Volume = tiker.Volume
                };
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
        var res = await restClient.UnifiedApi.Trading.PlaceOrderAsync(
                symbol!,
                OKXOrderSide.Buy,
                OKXOrderType.MarketOrder, quantity
            );
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
        var res = await restClient.UnifiedApi.Trading.PlaceOrderAsync(
            symbol,
            OKXOrderSide.Sell,
            OKXOrderType.MarketOrder, quantity);
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
        string listenKey = "";
        //var res = spotMarg ? restClient.UnifiedApi.Account.StartUserStreamAsync().Result
        //                   : restClient.UnifiedApi.Account.StartMarginUserStreamAsync().Result;
        //if (res.Success)
        //{
        //    listenKey = res.Data;
        //}
        //else
        //{
        //    Log.Error(Name, $"Listen Key failed - {res.Error?.Message}");
        //}
        return listenKey;
    }

    public async override Task<Order> GetLastSpotOrder(string symbol)
    {
        Order order = new() { Symbol = symbol };
        var res = await restClient.UnifiedApi.Trading.GetOrdersAsync(OKXInstrumentType.Spot, symbol);
        if (res.Success)
        {
            var ord = res.Data
                .Where(o => o.OrderState == OKXOrderState.Filled && o.OrderSide == OKXOrderSide.Buy)
                .MaxBy(o => o.CreateTime);
            if (ord != null)
            {
                order.Status = (CommonOrderStatus)ord.OrderState;
                order.QuantityFilled = ord.QuantityFilled;
                order.Quantity = ord.Quantity;
                order.Price = ord.Price;
                order.Side = (CommonOrderSide)ord.OrderSide;
                order.Timestamp = ord.CreateTime;
            }
        }
        else
        {
            Log.Error(Name, $"GetSpotOrders failed - {res.Error?.Message}");
        }
        return order;
    }

    public async override void SubscribeToSpotAccountUpdates()
    {
        //string listenKey = GetListenKey();
        var res = await socketClient.UnifiedApi.Account.
            SubscribeToAccountUpdatesAsync(
                _symbol, true,
                onData => { 
                    onData.Details.ToList().ForEach(d =>
                    {
                        Log.Info(Name, $"AccountUpdate {d.Asset} {d.Twap} {d.AvailableBalance}");
                    });
                }
            );
        if (!res.Success)
            Log.Error(Name, $"Error in SubscribeToSpotAccSocket: {res.Error?.Message}");
    }

}
