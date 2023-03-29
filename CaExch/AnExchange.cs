using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;

namespace CaExch;

public abstract class AnExchange
{
    public static event Action<int, string, Kline>? OnKline;
    protected void SendKline(int ID, string s, Kline k) => OnKline?.Invoke(ID, s, k);

    public abstract int ID { get; }
    public abstract string Name { get; }
    public abstract ISymbolOrderBook OrderBook { get; }
    public abstract List<Kline> GetKlines(string symbol, string inter);
    public abstract void SubscribeToSocket(string symbol, string inter);
    public abstract void Unsub();
    protected int IntervalInSeconds(string inter)
    {
        int seconds = 0;
        switch (inter)
        {
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
            default:
                break;
        }

        return seconds;
    }
}