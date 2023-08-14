
using CaExch;
using CryptoExchange.Net.CommonObjects;
using DevExpress.Utils;
using DevExpress.XtraCharts;

namespace bot3;

public partial class UcSinexChart : UserControl
{
    string _symbol = "BTCUSDT";
    string _interval = "1m";
    AnExchange Exchange = new CaBinance();
    List<Kline> _klines = new();

    public UcSinexChart()
    {
        InitializeComponent();

        this.Controls.Add(_chart);
        _chart.Dock = DockStyle.Fill;
    }

    private async void UcSinexChart_Load(object sender, EventArgs e)
    {
        _klines = await Exchange.GetKlines(_symbol, _interval);
        _klines = _klines.Skip(_klines.Count - 100).ToList();
        _chart.DataSource = _klines;

        MainChart();


        SetupAxisX();

        Indicators();
        //Indicators1();
        
        //AddVolume();

        _diagram.DefaultPane.LayoutOptions.RowSpan = 3;


        if (_diagram.Panes.Count > 0)
            _diagram.AxisX.SetVisibilityInPane(false, _diagram.DefaultPane);

        for (int i = 0; i < _diagram.Panes.Count - 1; i++)
        {
            _diagram.AxisX.SetVisibilityInPane(false, _diagram.Panes[i]);
            _diagram.Panes[i].LayoutOptions.RowSpan = 1;
        }

    }


}
      