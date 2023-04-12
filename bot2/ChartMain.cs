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
    ChartPoint _chaPoint;

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

    public event Action? NeedToRepopulate;
    public event Action<Kline>? OnKlineUpdated;
    public AnExchange Exchange; 

    public Charty(Chart chart, AnExchange ech, string symbo)
    {
        Exchange = ech;
        _symbol = symbo;

        _ch = chart;
        _chaPoint = new(_ch);

        _ch.Series.Clear();
        _ch.Legends.Clear();
        _ch.BackColor = Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
        _ch.BackGradientStyle = GradientStyle.TopBottom;
        _ch.BackSecondaryColor = Color.White;
        _ch.BorderlineColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        _ch.BorderlineDashStyle = ChartDashStyle.Solid;
        _ch.BorderlineWidth = 2;
        _ch.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

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

        _cha = _ch.ChartAreas[0];
        _cha.AxisY2.ScrollBar.Enabled = false;
        _cha.AxisY2.Enabled = AxisEnabled.True;
        _cha.AxisY2.IsStartedFromZero = _ch.ChartAreas[0].AxisY.IsStartedFromZero;
        _cha.BackColor = Color.Gray;
        _cha.BackGradientStyle = GradientStyle.LeftRight;
        _cha.BackSecondaryColor = Color.White;
        _cha.ShadowOffset = 5;
        _cha.Position.Auto = false;
        _cha.Position.Width = 92;
        _cha.Position.Height = 90;
        _cha.Position.X = 1.5F;
        _cha.Position.Y = 7;

        var cx = _cha.CursorX;
        cx.IsUserEnabled = true;
        cx.IsUserSelectionEnabled = true;
        cx.LineDashStyle = ChartDashStyle.Dash;

        var cy = _cha.CursorY;
        cy.IsUserEnabled = true;
        cy.IsUserSelectionEnabled = true;
        cy.LineDashStyle = ChartDashStyle.Dash;
        cy.AxisType = AxisType.Secondary;

        Exchange.OnKlineUpdate += OnPriceUpdate;
    }
    string DL(DateTime d)
    {
        List<Kline> ks = new(_klines.Skip(_klines.Count - _zoom));
        DateTime xMin = ks.Min(k => k.OpenTime);
        DateTime xMax = ks.Max(k => k.OpenTime);
        if(_interval == "1s")
            if(xMax.Hour == xMin.Hour)
                return d.ToString("mm:ss");
            else
                return d.ToString("hh:mm:ss");
        else if (xMax.Day == xMin.Day)
            return d.ToString("hh:mm");
        else if(xMax.Year == xMin.Year)
            return d.ToString("dd.MM hh:mm");
        else
            return d.ToString("dd.MM.yy hh:mm");
    }
    public void populate() 
    {
        try
        {
            Series sKlines = _ch.Series["Klines"];
            Series sVolume = _ch.Series["Volume"];
            sKlines.Points.Clear();
            sVolume.Points.Clear();

            List<Kline> ks = new(_klines.Skip(_klines.Count - _zoom));

            _yMax = Convert.ToDouble(ks.Max(k => k.HighPrice));
            _yMin = Convert.ToDouble(ks.Min(k => k.LowPrice));
            _yMin = _yMin - 0.1 * (_yMax - _yMin);

            _cha.AxisY2.ScaleView.Zoom(_yMin, _yMax);
            
            double maxVolume = Convert.ToDouble(ks.Max(k => k.Volume));
            _volumeRate = 0.3 * (_yMax - _yMin) / maxVolume;

            foreach (var k in ks)
            {
                sKlines.Points.AddXY(DL(k.OpenTime), k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

                decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
                int n = sVolume.Points.AddXY(DL(k.OpenTime), vol);
                if (n > 0)
                {
                    if ((double)vol! < sVolume.Points[n - 1].YValues[0])
                    {
                        sVolume.Points[n].Color = Color.Red;
                    }
                }
            }

            DrawIndicator(_indy);
        }
        catch (Exception ex)
        {
            Log.Error(Exchange.ID, "Charty.populate", "Error: " + ex.Message);
        }
    }

    async void UpdatePrice(Kline k)
    {
        Series sKlines, sVolume;
        try 
        { 
            if(_klines.Count == 0) return;  if(_ch.Series.Count < 2) return;
            sKlines = _ch.Series["Klines"]; if (sKlines.Points.Count == 0) return;
            sVolume = _ch.Series["Volume"]; if (sVolume.Points.Count == 0) return;

            var lk = _klines.Last();
            if (lk.OpenTime < k.OpenTime)
            {
                await GetKlines();
                NeedToRepopulate?.Invoke();
                return;
            }
            sKlines.Points.Remove(sKlines.Points.Last());
            sVolume.Points.Remove(sVolume.Points.Last());
            sKlines.Points.AddXY(DL(k.OpenTime), k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);
            decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
            sVolume.Points.AddXY(DL(k.OpenTime), vol);

        }
        catch(Exception ex) 
        {
            Log.Error(Exchange.ID, "Charty.UpdateKline", "Error: " + ex.Message);
        }
    }

    public async Task GetKlines()
    {
        _klines = await Exchange.GetKlines(_symbol, _interval);
    }

    void OnPriceUpdate(string s, Kline k)
    {
        if (_symbol == s)
        {
            OnKlineUpdated?.Invoke(k);
            UpdatePrice(k);
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
