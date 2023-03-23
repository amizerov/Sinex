using amLogger;
using CryptoExchange.Net.CommonObjects;

namespace CaExch;

public abstract class AnExchange
{
    public static event Action<string, Kline>? OnKline;
    protected void SendKline(string s, Kline k) => OnKline?.Invoke(s, k);

    public abstract int ID { get; }
    public abstract string Name { get; }
    
    public abstract List<Kline> GetKlines(string symbol, string inter);
    public abstract void SocketSubscribe(string symbol, string inter);
    protected TimeSpan GetIntervalFromString(string inter)
    {
        TimeSpan ts = new(0, 1, 0);

        switch (inter)
        {
            case "1m":
                ts = new(0, 1, 0);
                break;
            case "3m":
                ts = new(0, 3, 0);
                break;
            case "5m":
                ts = new(0, 5, 0);
                break;
            case "15m":
                ts = new(0, 15, 0);
                break;
            case "30m":
                ts = new(0, 30, 0);
                break;
            case "1h":
                ts = new(1, 0, 0);
                break;
            case "2h":
                ts = new(2, 0, 0);
                break;
            case "4h":
                ts = new(4, 0, 0);
                break;
            case "6h":
                ts = new(6, 0, 0);
                break;
            case "8h":
                ts = new(8, 0, 0);
                break;
            case "12h":
                ts = new(12, 0, 0);
                break;
            case "1d":
                ts = new(1, 0, 0, 0);
                break;
            default:
                break;
        }

        return ts;
    }
}