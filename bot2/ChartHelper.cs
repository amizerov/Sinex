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
    List<Kline> klines = new();
    int         _excha = 0;
    int         _zoom = 50;
    double      _volumeRate = 0;
    double      _yMax = 0;
    double      _yMin = 0;
    string      _indy = "";
    string      _symbol = "";
    string      _interval = "";

    public int Zoom
    {
        get { return _zoom; }
        set
        {
            _zoom = value;
            if (_zoom < 10) _zoom = 10;
            if (_zoom > klines.Count) _zoom = klines.Count;
        }
    }
    public int Exchange { get { return _excha; } 
        set {
            if (_excha != value)
            {
                var excha = Exchanges.FirstOrDefault(e => e.ID == _excha);
                excha?.Unsub();
                _symbol = "";
            }
            _excha = value; 
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

                var excha = Exchanges.FirstOrDefault(e => e.ID == _excha);
                excha?.Unsub();

                if(_symbol.Length > 0)
                    excha?.SubscribeToSocket(_symbol, _interval);
            }
        }
    }
    public string Interval
    {
        get { return _interval; }
        set
        {
            if (_interval != value)
            {
                _interval = value;
                
                var excha = Exchanges.FirstOrDefault(e => e.ID == _excha);
                excha?.Unsub();
                excha?.SubscribeToSocket(_symbol, _interval);
            }
        }
    }

    public event Action<Kline>? OnLastKline;
    public List<AnExchange> Exchanges = new(){
        new CaBinance(),
        new CaKucoin(),
        new CaHuobi(),
        new CaBittrex(),
        new CaBybit()
    };

    public Charty(Chart chart)
    {
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

        _cha = _ch.ChartAreas[0];
        _cha.AxisY2.ScrollBar.Enabled = false;
        _cha.AxisY2.Enabled = AxisEnabled.True;
        _cha.AxisY2.IsStartedFromZero = _ch.ChartAreas[0].AxisY.IsStartedFromZero;
        _cha.AxisX.LabelStyle.Format = "yy-MM-dd hh:mm";

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

            List<Kline> ks = new(klines.Skip(klines.Count - _zoom));

            _yMax = Convert.ToDouble(ks.Max(k => k.HighPrice));
            _yMin = Convert.ToDouble(ks.Min(k => k.LowPrice));
            _yMin = _yMin - 0.1 * (_yMax - _yMin);

            _cha.AxisY2.ScaleView.Zoom(_yMin, _yMax);

            double maxVolume = Convert.ToDouble(ks.Max(k => k.Volume));
            _volumeRate = 0.3 * (_yMax - _yMin) / maxVolume;
            foreach (var k in ks)
            {
                sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

                decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
                sVolume.Points.AddXY(k.OpenTime, vol);
            }

            DrawIndicator(_indy);
        }
        catch (Exception ex)
        {
            Log.Error(_excha, "populate", "Error: " + ex.Message);
        }
    }
    void UpdateKline(Kline k)
    {
        if(klines.Count == 0) return;

        var lk = klines.Last();
        if (lk.OpenTime == k.OpenTime)
        {
            Series sKlines = _ch.Series["Klines"];
            sKlines.Points.Remove(sKlines.Points.Last());
            sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

            Series sVolume = _ch.Series["Volume"];
            sVolume.Points.RemoveAt(sVolume.Points.Count - 1);

            decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
            sVolume.Points.AddXY(k.OpenTime, vol);

            Log.Info(_excha, "UpdateKline", $"deal -> {k.ClosePrice} / {k.Volume}");
        }
        else
        {
            Log.Info(_excha, "UpdateKline", "New Kline starts GetKlines");

            GetKlines();
            try{
                populate();
            }catch (Exception ex) { Log.Error(_excha, "populate chart", ex.Message); }
        }
    }

    public void GetKlines()
    {
        var excha = Exchanges.FirstOrDefault(e => e.ID == Exchange);
        if (excha != null)
            klines = excha.GetKlines(Symbol, Interval);
    }

    void OnKline(int id, string s, Kline k)
    {
        OnLastKline?.Invoke(k);
        UpdateKline(k);
    }

    public void DrawIndicator(string indica)
    {
        _indy = indica;
        if (_indy == "sma") DrawSma();
    }
    void DrawSma()
    {
        Series k = _ch.Series["Klines"];
        Series s = _ch.Series["Indica"];
        s.ChartType = SeriesChartType.FastLine;
        s.YAxisType = AxisType.Secondary;
        s.Color = Color.Red;

        int lookbackPeriods = _zoom < 50 ? 5 : _zoom / 10;
        List<SmaResult> sma = Indica.GetSma(klines, lookbackPeriods);

        List<Kline> ks = klines.Skip(klines.Count - _zoom).ToList();

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
            Log.Error(_excha, "DrawSma", "Error: " + ex.Message);
        }
    }
}
