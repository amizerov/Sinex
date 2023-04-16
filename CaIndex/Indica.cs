using CryptoExchange.Net.CommonObjects;
using Skender.Stock.Indicators;

public class Indica
{
    public static List<EmaResult> GetEma(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<EmaResult> ema = quotes.GetEma(lookbackPeriods);

        return ema.ToList();
    }

    public static List<SmaResult> GetSma(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<SmaResult> sma = quotes.GetSma(lookbackPeriods);

        return sma.ToList();
    }

    public static List<SmmaResult> GetSmma(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<SmmaResult> smma = quotes.GetSmma(lookbackPeriods);

        return smma.ToList();
    }

    public static List<RsiResult> GetRsi(List<Kline> klines, int lookbackPeriods)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<RsiResult> rsi = quotes.GetRsi(lookbackPeriods);

        return rsi.ToList();
    }
    public static List<BollingerBandsResult> GetMfi(List<Kline> klines, int lookbackPeriods, double standardDeviations)
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines(klines);
        IEnumerable<BollingerBandsResult> bol = quotes.GetBollingerBands(lookbackPeriods, standardDeviations);

        return bol.ToList();
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