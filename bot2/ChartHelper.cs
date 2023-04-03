using System.Windows.Forms.DataVisualization.Charting;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CaExch;
using Skender.Stock.Indicators;

namespace bot2;

public class Charty
{
    Chart       _ch;
    ChartArea   _cha;
    List<Kline> _klines = new();

    int         _zoom = 100;
    double      _volumeRate = 0;
    double      _yMax = 0;
    double      _yMin = 0;
    string      _indy = "";
    string      _symbol = "";
    string      _interval = "";

    int         _klineSubscriptionId = 0;

    public int Zoom
    {
        get { return _zoom; }
        set
        {
            _zoom = value;
            if (_zoom < 10) _zoom = 10;
            if (_zoom > _klines.Count) _zoom = _klines.Count;
        }
    }
    public string Symbol
    {
        get { return _symbol; }
        set
        {
            if (_symbol != value)
            {
                _symbol = value;
            }
        }
    }
    public async void SetInterval(string value)
    {
        {
            if (_interval != value)
            {
                _interval = value;

                if(_klineSubscriptionId > 0)
                    Exchange.UnsubKlineSocket(_klineSubscriptionId);

                if (_symbol.Length > 0 && _interval.Length > 0)
                {
                    _klineSubscriptionId = await Exchange.SubscribeToSocket(_symbol, _interval);
                }
            }
        }
    }

    public event Action<Kline>? OnLastKline;
    public AnExchange Exchange; 

    public Charty(Chart chart, AnExchange ech, string symbo)
    {
        Exchange = ech;
        _symbol = symbo;

        _ch = chart;
        _ch.Series.Clear();
        _ch.Legends.Clear();

        Series sKlines = new Series("Klines");
        Series sVolume = new Series("Volume");
        Series sIndica = new Series("Indica");
        _ch.Series.Add(sKlines);
        _ch.Series.Add(sVolume);
        _ch.Series.Add(sIndica);

        sKlines.ChartType = SeriesChartType.Candlestick;
        sKlines["OpenCloseStyle"] = "Triangle";
        sKlines["ShowOpenClose"] = "Both";
        sKlines["PointWidth"] = "1.0";
        sKlines["PriceUpColor"] = "Green";
        sKlines["PriceDownColor"] = "Red";
        sKlines.YAxisType = AxisType.Secondary;

        sVolume.ChartType = SeriesChartType.Column;
        sVolume.Color = Color.FromArgb(80, Color.Blue);
        sVolume.YAxisType = AxisType.Secondary;

        AnExchange.OnKline += OnKline;
    }
    public void populate() 
    {
        try
        {
            Series sKlines = _ch.Series["Klines"];
            Series sVolume = _ch.Series["Volume"];
            sKlines.Points.Clear();
            sVolume.Points.Clear();

            _cha = _ch.ChartAreas[0];
            _cha.AxisY2.ScrollBar.Enabled = false;
            _cha.AxisY2.Enabled = AxisEnabled.True;
            _cha.AxisY2.IsStartedFromZero = _ch.ChartAreas[0].AxisY.IsStartedFromZero;
            _cha.AxisX.LabelStyle.Format = "dd.MM.yy hh:mm";

            List<Kline> ks = new(_klines.Skip(_klines.Count - _zoom));

            _yMax = Convert.ToDouble(ks.Max(k => k.HighPrice));
            _yMin = Convert.ToDouble(ks.Min(k => k.LowPrice));
            _yMin = _yMin - 0.1 * (_yMax - _yMin);

            _cha.AxisY2.ScaleView.Zoom(_yMin, _yMax);
            
            double maxVolume = Convert.ToDouble(ks.Max(k => k.Volume));
            _volumeRate = 0.3 * (_yMax - _yMin) / maxVolume;

            Log.Trace(Exchange.ID, "Charty.populate statred", $"symbol {_symbol} zoom {_zoom}");

            foreach (var k in ks)
            {
                sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

                decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
                int n = sVolume.Points.AddXY(k.OpenTime, vol);
                if (n > 0)
                {
                    if ((double)vol! < sVolume.Points[n - 1].YValues[0])
                    {
                        sVolume.Points[n].Color = Color.Red;
                    }
                }
            }

            DrawIndicator(_indy);

            Log.Trace(Exchange.ID, "Charty.populated", $"symbol {_symbol} zoom {_zoom}");
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "Charty.populate", "Error: " + ex.Message);
        }
    }
    void UpdateKline(Kline k)
    {
        Series sKlines, sVolume;
        try { 
            if(_klines.Count == 0) return;         if(_ch.Series.Count < 2) return;
            sKlines = _ch.Series["Klines"]; if (sKlines.Points.Count == 0) return;
            sVolume = _ch.Series["Volume"]; if (sVolume.Points.Count == 0) return;
        }
        catch { return; }

        var lk = _klines.Last();
        if (lk.OpenTime < k.OpenTime)
        {
            _klines.Add(k);
            _klines.Remove(_klines.First());
            populate();
        }
        else
        {
            sKlines.Points.Remove(sKlines.Points.Last());
            sVolume.Points.Remove(sVolume.Points.Last());
            sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);
            decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
            sVolume.Points.AddXY(k.OpenTime, vol);
        }

        Log.Info(Exchange.ID, "Charty.UpdateKline", $"deal -> {k.ClosePrice} / {k.Volume}");
    }

    public async Task GetKlines()
    {
        _klines = await Exchange.GetKlines(_symbol, _interval);
    }

    void OnKline(int id, string s, Kline k)
    {
        if (Exchange.ID == id && _symbol == s)
        {
            OnLastKline?.Invoke(k);
            UpdateKline(k);
        }
    }
    public void UnsubKlineSocket()
    {
        if (_klineSubscriptionId > 0)
            Exchange.UnsubKlineSocket(_klineSubscriptionId);
    }
    public void DrawIndicator(string indica)
    {
        _indy = indica;
        if (_indy == "sma") DrawSma();
        if (_indy == "rsi") DrawRsi();
        if (_indy == "vfi") DrawSma();
    }
    void DrawSma()
    {
        Series k = _ch.Series["Klines"];
        Series s = _ch.Series["Indica"];
        s.ChartType = SeriesChartType.FastLine;
        s.YAxisType = AxisType.Secondary;
        s.Color = Color.Red;

        int lookbackPeriods = _zoom < 50 ? 5 : _zoom / 10;
        List<SmaResult> sma = Indica.GetSma(_klines, lookbackPeriods);

        List<Kline> ks = _klines.Skip(_klines.Count - _zoom).ToList();

        try
        {
            List<SmaResult> smas = new(sma.Where(p => p.Date >= ks.First().OpenTime));
            s.Points.Clear();
            foreach (var v in smas)
            {
                s.Points.AddXY(v.Date, v.Sma);
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
