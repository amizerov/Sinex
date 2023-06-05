using amLogger;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using Binance.Net.Objects;
using Binance.Net.SymbolOrderBooks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CaSecrets;

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

    public override async Task<bool> CheckApiKey()
    {
        bool res = false;
        try
        {
            string apiKey = Secrets.BinanceApiKey;
            string apiSecret = Secrets.BinanceApiSecret;

            restClient = new BinanceClient(
                new BinanceClientOptions()
                {
                    ApiCredentials = new BinanceApiCredentials(apiKey, apiSecret)
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
        var res = await restClient.SpotApi.Account.GetAccountInfoAsync();
        if (res.Success)
        {
            var bals = res.Data.Balances.ToList();
            foreach( var b in bals )
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
        CancellationToken cancellationToken = new CancellationToken();
        var r = await restClient.SpotApi.CommonSpotClient
            .GetKlinesAsync(symbol, TimeSpan.FromSeconds(IntervalInSeconds(inter)),
            null, null, 1000, cancellationToken);

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

                KlineUpdated(symbol, kline);
                Log.Info(ID, "qqq", $"{symbol} {k.OpenTime} {k.ClosePrice}");
            });

        return r;
    }

    public async override void SubsсribeToTicker(string symbol)
    {
        var res = await socketClient.SpotStreams.SubscribeToTickerUpdatesAsync(symbol,
            onMessage => {
                Ticker t = new();
                var d = onMessage.Data;
                t.HighPrice = d.HighPrice;
                t.LowPrice = d.LowPrice;
                t.LastPrice = d.LastPrice;
                t.Symbol = d.Symbol;
                t.Volume = d.Volume;
                t.Price24H = d.PrevDayClosePrice;
                TickerUpdated(t);
            });
        if (!res.Success)
        {
            Log.Error(Name, $"Error in SubsсribeToTicker: {res.Error?.Message}");
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
    public async override void SpotOrderBuy(decimal quantity)
    {
        Console.WriteLine("spot buy " + _symbol);
        var res = await restClient.SpotApi.Trading.PlaceOrderAsync(
            _symbol!,
            OrderSide.Buy,
            SpotOrderType.Market, quantity);
        if (!res.Success)
        {
            Log.Error(Name, $"Error in SpotOrderBuy: {res.Error?.Message}");
        }
    }
    public async override void SpotOrderSell(decimal quantity)
    {
        var res = await restClient.SpotApi.Trading.PlaceOrderAsync(
            _symbol!,
            OrderSide.Sell,
            SpotOrderType.Market, quantity);
        if(!res.Success)
        {
            Log.Error(Name, $"Error in SpotOrderSell: {res.Error?.Message}");
        }
    }
    public async override void FutuOrderBuy(decimal quantity)
    {
        Log.Info(Name, "futu buy " + _symbol);

        var res = await restClient.UsdFuturesApi.Trading.PlaceOrderAsync(
        //_restClient.CoinFuturesApi.Trading.PlaceOrderAsync(
            _symbol!,
            OrderSide.Buy,
            FuturesOrderType.Market,
            quantity);
    }
    public async override void FutuOrderSell(decimal quantity)
    {
        var res = await restClient.CoinFuturesApi.Trading.PlaceOrderAsync(
            _symbol!,
            OrderSide.Sell,
            FuturesOrderType.Market,
            quantity);
    }

    /*
     *  Account
     */

    string GetListenKey(bool spotMarg = true)
    {
        string listenKey = "";
        var res = spotMarg ? restClient.SpotApi.Account.StartUserStreamAsync().Result
                           : restClient.SpotApi.Account.StartMarginUserStreamAsync().Result;
        if (res.Success)
        {
            listenKey = res.Data;
        }
        else
        {
            Log.Error(Name, $"Listen Key failed - {res.Error?.Message}");
        }
        return listenKey;
    }

    public async override Task<Order> GetLastSpotOrder(string symbol)
    {
        Order? order = new() { Symbol = symbol };
        var res = await restClient.SpotApi.Trading.GetOrdersAsync(symbol);
        if (res.Success)
        {
            var ord = res.Data
                .Where(o => o.Status == OrderStatus.Filled && o.Side == OrderSide.Buy)
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

    public async override void SubscribeToSpotAccountUpdates()
    {
        string listenKey = GetListenKey();
        var res = await socketClient.SpotStreams
            .SubscribeToUserDataUpdatesAsync(
                listenKey,
                order => { 
                    int a = 1;
                    a++;
                },
                ocoOrder => { },
                accPosition => {
                    var d = accPosition.Data;
                    List<Balance> balances = new List<Balance>();
                    foreach(var bal in d.Balances)
                    {
                        Balance balance = new Balance();
                        balance.Asset = bal.Asset;
                        balance.Available = bal.Available;
                        balance.Total = bal.Total;
                        balances.Add(balance);
                    }
                    AccPositionUpdated(balances);
                },
                accBalance => {
                    string asset = accBalance.Data.Asset;
                    decimal delta = accBalance.Data.BalanceDelta;
                    AccBalanceUpdated(asset, delta);
                }
            );
        if (!res.Success)
            Log.Error(Name, $"Error in SubscribeToSpotAccSocket: {res.Error?.Message}");
    }

}
