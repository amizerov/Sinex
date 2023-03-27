using System.Windows.Forms.DataVisualization.Charting;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CaExch;

namespace bot2;

public class Charty
{
    public event Action<Kline>? OnLastKline;

    int _excha = 0;
    public int Exchange { get { return _excha; } 
        set {
            if (_excha != value)
            {
                var excha = Exchanges.FirstOrDefault(e => e.ID == _excha);
                excha?.Unsub();
            }
            _excha = value; 
        } 
    }
    public string Symbol = "";
    public string Interval = "";

    Chart _ch;
    ChartArea _cha;
    List<Kline> klines = new();
    
    public List<AnExchange> Exchanges = new(){
        new CaBinance(),
        new CaKucoin(),
        new CaHuobi(),
        new CaBittrex(),
        new CaBybit()
    };

    int _zoom = 50;
    public int Zoom { get { return _zoom; }
        set {
            _zoom = value;
            if(_zoom < 10) _zoom = 10;
            if (_zoom > klines.Count) _zoom = klines.Count;
        }
    }

    double _volumeRate = 0;
    double _yMax = 0;
    double _yMin = 0;

    public Charty(Chart chart)
    {
        _ch = chart;
        _ch.Series.Clear();
        _ch.Legends.Clear();

        Series sKlines = new Series("Klines");
        Series sVolume = new Series("Volume");
        _ch.Series.Add(sKlines);
        _ch.Series.Add(sVolume);

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
        List<Kline> ks = klines.Skip(klines.Count - _zoom).ToList();

        Series sKlines = _ch.Series["Klines"];
        Series sVolume = _ch.Series["Volume"];
        sKlines.Points.Clear();
        sVolume.Points.Clear();

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
    }
    void UpdateKline(Kline k)
    {
        if(klines.Count == 0) return;

        var lk = klines.Last();
        if (lk.OpenTime == k.OpenTime)
        {
            Series sKlines = _ch.Series["Klines"];
            sKlines.Points.RemoveAt(sKlines.Points.Count - 1);
            sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

            Series sVolume = _ch.Series["Volume"];
            sVolume.Points.RemoveAt(sVolume.Points.Count - 1);

            decimal? vol = k.Volume * (decimal)_volumeRate + (decimal)_yMin;
            sVolume.Points.AddXY(k.OpenTime, vol);

            Log.Info(_excha, "UpdateKline", $"deal -> {k.ClosePrice} / {k.Volume}");
        }
        else
        {
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

}
