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
    Series sKlines = new Series("Klines");
    List<Kline> klines = new();
    
    public List<AnExchange> Exchanges = new(){
        new CaExch.Binance(),
        new CaExch.Kucoin(),
        new CaExch.Huobi(),
        new CaExch.Bittrex(),
        new CaExch.Bybit()
    };

    int zoom = 50;
    public int Zoom { get { return zoom; }
        set {
            zoom = value;
            if(zoom < 10) zoom = 10;
            if (zoom > klines.Count) zoom = klines.Count;
        }
    }

    public void GetKlines()
    {
        var excha = Exchanges.FirstOrDefault(e => e.ID == Exchange);
        if(excha != null)
            klines = excha.GetKlines(Symbol, Interval);
    }

    public Charty(Chart chart)
    {
        _ch = chart;
        _ch.Series.Clear();
        _ch.Series.Add(sKlines);
        _ch.Name = "Candlestick Chart";
        sKlines.ChartType = SeriesChartType.Candlestick;
        sKlines["OpenCloseStyle"] = "Triangle";
        sKlines["ShowOpenClose"] = "Both";
        sKlines["PointWidth"] = "1.0";
        sKlines["PriceUpColor"] = "Green";
        sKlines["PriceDownColor"] = "Red";

        _cha = _ch.ChartAreas[0];

        _cha.AxisY2.ScrollBar.Enabled = false;
        _cha.AxisY2.Enabled = AxisEnabled.True;
        _cha.AxisY2.IsStartedFromZero = _ch.ChartAreas[0].AxisY.IsStartedFromZero;
        sKlines.YAxisType = AxisType.Secondary;

        _cha.AxisX.LabelStyle.Format = "yy-MM-dd hh:mm";

        _ch.Legends.Clear();

        AnExchange.OnKline += OnKline;
    }
    void OnKline(int id, string s, Kline k)
    {
        OnLastKline?.Invoke(k);
        UpdateKline(k);
    }
    public void populate() 
    {
        List<Kline> ks = klines.Skip(klines.Count - zoom).ToList();
        Series sKlines = _ch.Series["Klines"];
        sKlines.Points.Clear();
        foreach (var k in ks)
        {
            sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);
        }

        double ymax = Convert.ToDouble(ks.Max(k => k.HighPrice));
        double ymin = Convert.ToDouble(ks.Min(k => k.LowPrice));
        
        _cha.AxisY2.ScaleView.Zoom(ymin, ymax);
    }

    void UpdateKline(Kline k)
    {
        var lk = klines.Last();
        if (lk.OpenTime == k.OpenTime)
        {
            Series sKlines = _ch.Series["Klines"];
            sKlines.Points.RemoveAt(sKlines.Points.Count - 1);
            sKlines.Points.AddXY(k.OpenTime, k.HighPrice, k.LowPrice, k.OpenPrice, k.ClosePrice);

            Logger.Write(new Log() { msg = $"deal -> {k.ClosePrice}" });
        }
        else
        {
            GetKlines();
            populate();
        }
    }
}
