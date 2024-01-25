using amLogger;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace CaExch2;

public abstract class AnExchange
{
    public event Action<List<Balance>>? OnAccPositionUpdate;
    protected void AccPositionUpdated(List<Balance> balances) => OnAccPositionUpdate?.Invoke(balances);

    public event Action<string, decimal>? OnAccBalanceUpdate;
    protected void AccBalanceUpdated(string asset, decimal delta) => OnAccBalanceUpdate?.Invoke(asset, delta);

    public event Action<string, Kline>? OnKlineUpdate;
    protected void KlineUpdated(string s, Kline k) => OnKlineUpdate?.Invoke(s, k);
    
    public event Action<Ticker>? OnTickerUpdate;
    protected void TickerUpdated(Ticker t) => OnTickerUpdate?.Invoke(t);

    public abstract int ID { get; }
    public abstract string Name { get; }

    public abstract ISymbolOrderBook OrderBook { get; }
    public abstract Task<bool> CheckApiKey();
    public abstract Task<List<Balance>> GetBalances();
    public abstract Task<Ticker> GetTickerAsync(string symbol);
    public abstract Task<List<Kline>> GetKlines(string symbol, string inter, int count = 0);
    public abstract List<string> Intervals { get; }

    public abstract Task<int> SubsсribeToTicker(string symbol);
    public abstract void UnSubFromTicker(int subsId);

    protected abstract Task<CallResult<UpdateSubscription>> SubsToSock(string symbol, string inter);
    public async Task<int> SubscribeToSocket(string symbol, string inter)
    {
        int subscriptionId = 0;
        Log.Trace(ID, "SubscribeToSocket", $"Begin subscribe {symbol}, Interval = {inter}");
        try
        {
            var r = await SubsToSock(symbol, inter);
            subscriptionId = r.Data.Id;
            if (r.Success)
            {
                Log.Trace(ID, $"SocketSubscribe({symbol}, {subscriptionId})", $"interval {inter}");
            }
            else
            {
                Log.Error(ID, $"SocketSubscribe({symbol})", "Error: " + r.Error?.Message);
            }
        }
        catch (Exception e)
        {
            Log.Error(ID, $"SocketSubscribe({symbol})", "Exception: " + e.Message);

        }
        return subscriptionId;
    }
    public abstract void UnsubKlineSocket(int subscriptionId);

    protected int IntervalInSeconds(string inter)
    {
        int seconds = 0;
        switch (inter)
        {
            case "1s":
                seconds = 1;
                break;
            case "1m":
                seconds = 60;
                break;
            case "3m":
                seconds = 3 * 60;
                break;
            case "5m":
                seconds = 5 * 60;
                break;
            case "15m":
                seconds = 15 * 60;
                break;
            case "30m":
                seconds = 30 * 60;
                break;
            case "1h":
                seconds = 60 * 60;
                break;
            case "2h":
                seconds = 2 * 60 * 60;
                break;
            case "4h":
                seconds = 4 * 60 * 60;
                break;
            case "6h":
                seconds = 6 * 60 * 60;
                break;
            case "8h":
                seconds = 8 * 60 * 60;
                break;
            case "12h":
                seconds = 12 * 60 * 60;
                break;
            case "1d":
                seconds = 24 * 60 * 60;
                break;
            case "3d":
                seconds = 3 * 24 * 60 * 60;
                break;
            case "1w":
                seconds = 7 * 24 * 60 * 60;
                break;
            case "1M":
                seconds = 30 * 24 * 60 * 60;
                break;
            default:
                break;
        }

        return seconds;
    }

    /*
        Trading
     */
    public abstract Task<bool> PlaceSpotOrderBuy(string symbol, decimal quantity);
    public abstract Task<bool> PlaceSpotOrderSell(string symbol, decimal quantity);
    public abstract Task<bool> FutuOrderBuy(string symbol, decimal quantity);
    public abstract Task<bool> FutuOrderSell(string symbol, decimal quantity);

    /*
     *  Account 
     */
    public abstract Task<Order> GetLastSpotOrder(string symbol);
    public abstract void SubscribeToSpotAccountUpdates();
}