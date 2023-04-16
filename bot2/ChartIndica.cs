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

        foreach (var ind in IndicatorsList)
        {
            foreach (string s in ind.Settings)
            {
                Series ser = _ch.Series.Add("Indica_" + ind.Name + s.Replace(";", ""));

                if (ind.Name == "SMA")
                {
                    string[] a = s.Split(";");
                    if (a.Length == 3)
                    {
                        int lp = int.Parse(a[0]);
                        int lw = int.Parse(a[1]);
                        int lc = int.Parse(a[2]);
                        DrawSma(lp, lw, lc, ser);
                    }
                }
                if (ind.Name == "SMMA")
                {
                    string[] a = s.Split(";");
                    if (a.Length == 3)
                    {
                        int lp = int.Parse(a[0]);
                        int lw = int.Parse(a[1]);
                        int lc = int.Parse(a[2]);
                        DrawSmma(lp, lw, lc, ser);
                    }
                }
                if (ind.Name == "EMA")
                {
                    string[] a = s.Split(";");
                    if (a.Length == 3)
                    {
                        int lp = int.Parse(a[0]);
                        int lw = int.Parse(a[1]);
                        int lc = int.Parse(a[2]);
                        DrawEma(lp, lw, lc, ser);
                    }
                }
            }
        }
        return Task.CompletedTask;
    }
    void DrawEma(int lookbackPeriods, int lineWidth, int lineColor, Series sIndica)
    {
        sIndica.ChartType = SeriesChartType.FastLine;
        sIndica.YAxisType = AxisType.Secondary;
        sIndica.Color = Color.FromArgb(lineColor);
        sIndica.BorderWidth = lineWidth;

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
    void DrawSma(int lookbackPeriods, int lineWidth, int lineColor, Series sIndica)
    {
        sIndica.ChartType = SeriesChartType.FastLine;
        sIndica.YAxisType = AxisType.Secondary;
        sIndica.Color = Color.FromArgb(lineColor);
        sIndica.BorderWidth = lineWidth;

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
    void DrawSmma(int lookbackPeriods, int lineWidth, int lineColor, Series sIndica)
    {
        sIndica.ChartType = SeriesChartType.FastLine;
        sIndica.YAxisType = AxisType.Secondary;
        sIndica.Color = Color.FromArgb(lineColor);
        sIndica.BorderWidth = lineWidth;

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
