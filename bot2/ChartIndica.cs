using amLogger;
using CryptoExchange.Net.CommonObjects;
using Skender.Stock.Indicators;
using System.Windows.Forms.DataVisualization.Charting;

namespace bot2;

public partial class Charty
{
    public Task DrawIndicators(List<JIndica>? indicatorsList = null)
    {
        if (indicatorsList != null) IndicatorsList = indicatorsList;
        if (IndicatorsList.Count == 0) return Task.CompletedTask;

        var sIn = _ch.Series.Where(s => s.Name.StartsWith("Indica_"));
        int cnt = sIn.Count();
        for (int i = 0; i < cnt; i++)
        {
            var s = sIn.FirstOrDefault();
            _ch.Series.Remove(s);
        }

        foreach (var ind in IndicatorsList.Where(i => i.IsChecked))
        {
            foreach (string s in ind.Settings)
            {
                string[] a = s.Split(";");
                if (a.Length == 3)
                {
                    int lp = int.Parse(a[0]);
                    int lw = int.Parse(a[1]);
                    int lc = int.Parse(a[2]);

                    Series ser = _ch.Series.Add("Indica_" + ind.Name + s.Replace(";", ""));
                    ser.ChartType = SeriesChartType.FastLine;
                    ser.YAxisType = AxisType.Secondary;
                    ser.Color = Color.FromArgb(lc);
                    ser.BorderWidth = lw;

                    if (ind.Name == "SMA") DrawSma(lp, ser);
                    if (ind.Name == "SMMA") DrawSmma(lp, ser);
                    if (ind.Name == "EMA") DrawEma(lp, ser);
                    if (ind.Name == "WMA") DrawWma(lp, ser);
                    if (ind.Name == "EPMA") DrawEpma(lp, ser);
                }
            }
        }
        return Task.CompletedTask;
    }
    void DrawEma(int lookbackPeriods, Series sIndica)
    {
        List<EmaResult> ema = Indica.GetEma(_klines, lookbackPeriods);
        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<EmaResult> emas = new(ema.Where(p => p.Date >= ks.First().OpenTime));
            sIndica.Points.Clear();
            foreach (var v in emas)
            {
                sIndica.Points.AddXY(DL(v.Date), v.Ema);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawEma", "Error: " + ex.Message);
        }
    }
    void DrawSma(int lookbackPeriods, Series sIndica)
    {
        List<SmaResult> sma = Indica.GetSma(_klines, lookbackPeriods);
        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<SmaResult> smas = new(sma.Where(p => p.Date >= ks.First().OpenTime));
            sIndica.Points.Clear();
            foreach (var v in smas)
            {
                sIndica.Points.AddXY(DL(v.Date), v.Sma);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawSma", "Error: " + ex.Message);
        }
    }
    void DrawSmma(int lookbackPeriods, Series sIndica)
    {
        List<SmmaResult> smma = Indica.GetSmma(_klines, lookbackPeriods);
        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<SmmaResult> smmas = new(smma.Where(p => p.Date >= ks.First().OpenTime));
            sIndica.Points.Clear();
            foreach (var v in smmas)
            {
                sIndica.Points.AddXY(DL(v.Date), v.Smma);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawSma", "Error: " + ex.Message);
        }
    }
    void DrawWma(int lookbackPeriods, Series sIndica)
    {
        List<WmaResult> wma = Indica.GetWma(_klines, lookbackPeriods);
        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<WmaResult> wmas = new(wma.Where(p => p.Date >= ks.First().OpenTime));
            sIndica.Points.Clear();
            foreach (var v in wmas)
            {
                sIndica.Points.AddXY(DL(v.Date), v.Wma);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawSma", "Error: " + ex.Message);
        }
    }
    void DrawEpma(int lookbackPeriods, Series sIndica)
    {
        List<EpmaResult> epma = Indica.GetEpma(_klines, lookbackPeriods);
        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<EpmaResult> epmas = new(epma.Where(p => p.Date >= ks.First().OpenTime));
            sIndica.Points.Clear();
            foreach (var v in epmas)
            {
                sIndica.Points.AddXY(DL(v.Date), v.Epma);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawSma", "Error: " + ex.Message);
        }
    }
    void DrawRsi()
    {
        Series k = _ch.Series["Klines"];
        Series s = _ch.Series["Indica"];
        s.ChartType = SeriesChartType.FastLine;
        s.YAxisType = AxisType.Secondary;
        s.Color = Color.Red;

        int lookbackPeriods = _zoom < 50 ? 5 : _zoom / 10;
        List<RsiResult> rsi = Indica.GetRsi(_klines, lookbackPeriods);

        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<RsiResult> rsis = new(rsi.Where(p => p.Date >= ks.First().OpenTime));

            double rsiMax = Convert.ToDouble(rsis.Max(r => r.Rsi));
            double rsiMin = Convert.ToDouble(rsis.Min(r => r.Rsi));
            double rsiRate = 0.3 * (_yMax - _yMin) / (rsiMax - rsiMin);

            s.Points.Clear();
            foreach (var r in rsis)
            {
                s.Points.AddXY(r.Date, (r.Rsi - rsiMin) * rsiRate + _yMin);
            }
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "DrawSma", "Error: " + ex.Message);
        }
    }
}

public class JIndica
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Settings { get; set; }
    public bool IsChecked { get; set; }
    public JIndica() { Name = ""; Description = ""; Settings = new(); IsChecked = false; }

    public static List<JIndica> InitList()
    {
        List<JIndica> res = new();

        res.Add(new() { Name = "SMA", Description = "Simple Moving Average (SMA)" });
        res.Add(new() { Name = "SMMA", Description = "Smoothed Moving Average (SMMA)" });
        res.Add(new() { Name = "EMA", Description = "Exponential Moving Average (EMA)" });
        res.Add(new() { Name = "WMA", Description = "Weighted Moving Average (WMA)" });
        res.Add(new() { Name = "EPMA", Description = "Endpoint Moving Average (EPMA)" });

        return res;
    }
}