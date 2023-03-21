using System.Windows.Forms.DataVisualization.Charting;
using CryptoExchange.Net.CommonObjects;
using amLogger;
using CaExch;

namespace bot2;

public class Charty
{
    Chart _ch;
    ChartArea _cha;
    Series sKlines = new Series("Klines");
    List<Kline> klines = new();
    
    List<AnExchange> exchas = new(){
        new CaExch.Binance(),
        new CaExch.Kucoin(),
        new CaExch.Huobi()
    };

    int zoom = 50;
    public int Zoom { get { return zoom; }
        set {
            zoom = value;
            if(zoom < 10) zoom = 10;
            if (zoom > klines.Count) zoom = klines.Count;
        }
    }

    public void GetKlines(int eid, string symbo, string inter)
    {
        var excha = exchas.FirstOrDefault(e => e.ID == eid);
        if(excha != null)
            klines = excha.GetKlines(symbo, inter);
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

        _ch.Legends.Clear();
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
        _cha.AxisX.LabelStyle.Format = "yy-MM-dd hh:mm";

        double cc = ymax - ymin;
        double c = _cha.AxisY2.Maximum - _cha.AxisY2.Minimum;
        Logger.Write(new Log() { msg = $"c={c} - {cc}" });
    }
}
