using CaExch;
using CryptoExchange.Net.CommonObjects;
using System.Windows.Forms.DataVisualization.Charting;

namespace bot2;

public class ChartyBase
{
    public AnExchange Exchange;
    protected Chart _ch;
    protected ChartArea _cha;
    protected Title _title;

    protected List<Kline> _klines = new();
    protected List<JIndica> IndicatorsList = new();

    protected int _zoom = 100;
    protected double _volumeRate = 0;
    protected double _yMax = 0;
    protected double _yMin = 0;
    protected string _symbol = "";
    protected string _interval = "";

    protected int _klineSubscriptionId = 0;

    public string Symbol
    {
        get { return _symbol; }
        set { _symbol = value; }
    }    
    
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

    public ChartyBase(Chart chart, AnExchange ech, string symbo) 
    {
        Exchange = ech;
        _symbol = symbo;
        _ch = chart;
        _cha = _ch.ChartAreas[0];
        _title = _ch.Titles[0];

        //Когда уменьшаем форму графика шрифт надписи надо тоже уменьшить
        _ch.SizeChanged += chart_SizeChanged;

        _ch.Series.Clear();
        _ch.Legends.Clear();
        _ch.BackColor = Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
        _ch.BackGradientStyle = GradientStyle.TopBottom;
        _ch.BackSecondaryColor = Color.White;
        _ch.BorderlineColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        _ch.BorderlineDashStyle = ChartDashStyle.Solid;
        _ch.BorderlineWidth = 2;
        _ch.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

        SetChatArea();
        SetSeries();
        SetTitle();
    }
    void SetSeries()
    {
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
    }
    void SetChatArea()
    {
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
    }
    void SetTitle()
    {
        _title.Alignment = ContentAlignment.TopLeft;
        _title.BackColor = Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(120)))), ((int)(((byte)(160)))), ((int)(((byte)(240)))));
        _title.BorderColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        _title.Font = new Font("Trebuchet MS", 14.25F, FontStyle.Bold);
        _title.ForeColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        _title.Name = "Title1";
        _title.Position.Auto = false;
        _title.Position.Height = 3.5F;
        _title.Position.Width = 50F;
        _title.Position.X = 2F;
        _title.Position.Y = 2F;
        _title.ShadowColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        _title.Text = Exchange.Name + ": " + _symbol;

        _title.Tag = (double)_title.Font.Size / (double)_ch.Height;

        _title.TextStyle = TextStyle.Embed;
    }
    private void chart_SizeChanged(object? sender, EventArgs e)
    {
        // Буквы не влазят если график уменьшить
        // Поле для тайтла пропорционально уменьшается
        // когда график уменьшаем, поэтому придется поиграть с размером шрифта

        if (_title.Tag == null || _title.Tag.ToString() == "") return;

        float fonSize = (float)(0.013 * (double)_ch.Height);
        if(fonSize == 0) return;

        _title.Font = new Font("Trebuchet MS", fonSize, FontStyle.Bold);
    }

}
