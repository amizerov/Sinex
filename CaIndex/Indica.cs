using CryptoExchange.Net.CommonObjects;
using Skender.Stock.Indicators;

public class Indica
{
    List<Kline> klines;

    public Indica(List<Kline> klines)
    {
        this.klines = klines;
    }

    public IEnumerable<SmaResult> GetSma()
    {
        IEnumerable<Quote> quotes = GetQuotesFromKlines();
        IEnumerable<SmaResult> sma = quotes.GetSma(50);

        return sma;
    }

    IEnumerable<Quote> GetQuotesFromKlines()
    {
        List<Quote> res = new List<Quote>();
        foreach (var k in klines)
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