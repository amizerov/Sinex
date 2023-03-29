using CryptoExchange.Net.CommonObjects;
using Skender.Stock.Indicators;

public class Indica
{
    public static List<SmaResult> GetSma(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<SmaResult> sma = quotes.GetSma(lookbackPeriods);

        return sma.ToList();
    }

    public static List<RsiResult> GetRsi(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<RsiResult> rsi = quotes.GetRsi();

        return rsi.ToList();
    }
    public static List<MfiResult> GetMfi(List<Kline> klines)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<MfiResult> mfi = quotes.GetMfi();

        return mfi.ToList();
    }
    static IEnumerable<Quote> GetQuotesFromKlines(List<Kline> klines)
    {
        List<Kline> ks = new(klines);
        List<Quote> res = new List<Quote>();
        foreach (var k in ks)
        {
            Quote q = new Quote();
            q.Date = k.OpenTime;
            q.Volume = (decimal)k.Volume!;
            q.High = (decimal)k.HighPrice!;
            q.Low = (decimal)k.LowPrice!;
            q.Close = (decimal)k.ClosePrice!;
            q.Open = (decimal)k.OpenPrice!;
            res.Add(q);
        }
        return res;
    }

}