using bot2.Tools;
using CaExch;
using CryptoExchange.Net.CommonObjects;

namespace bot2;

public partial class FrmChart : Form
{
    Charty Charty;
    FrmStakan? frmOrderBook;

    string _indicators = "";

    public FrmChart(AnExchange exch, string symbo)
    {
        InitializeComponent();

        Charty = new(chart, exch, symbo);
        Charty.OnKlineUpdated += OnLastKline;
        Charty.NeedToRepopulateChart += OnNeedToRepopulateChart;

        foreach (var interval in Charty.Exchange.Intervals)
            cbInterval.Items.Add(interval);

        cbInterval.SelectedIndex = 3;
        Charty.Symbol = symbo;

        Text = exch.Name + " - " + symbo + " - Chart";
    }

    private void FrmChart_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);
        _indicators = Utils.LoadIndicators();

        chart.MouseWheel += chart_MouseWheel;
        InitChart();
    }
    private async void InitChart()
    {
        if (Charty.Symbol == "") return;

        await Charty.GetKlines();
        await Charty.populate();

        await Charty.DrawIndicators(_indicators);
    }
    void OnLastKline(Kline k)
    {
        try
        {
            Invoke(new Action(() =>
            {
                lblSymbol.Text = $"{Charty.Symbol}({k.ClosePrice} / {k.Volume})";
            }));
        }
        catch { }
    }
    void OnNeedToRepopulateChart()
    {
        Invoke(new Action(async () =>
        {
            await Charty.populate();
        }));
    }
    void chart_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (e.Delta > 0)
            ZoomIn();
        else
            ZoomOut();
    }
    async void ZoomOut()
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom += 10;
        else
            Charty.Zoom++;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        await Charty.populate();
    }

    async void ZoomIn()
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom -= 10;
        else
            Charty.Zoom--;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        await Charty.populate();
    }

    private void cbInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        Charty.SetInterval(cbInterval.Text);
        InitChart();
    }

    private void btnStakan_Click(object sender, EventArgs e)
    {
        if (frmOrderBook != null)
        {
            if (!frmOrderBook.IsDisposed || frmOrderBook.Visible)
            {
                frmOrderBook.Close();
                frmOrderBook.Dispose();
                frmOrderBook = null;
            }
        }
        frmOrderBook = new(Charty.Exchange.OrderBook);
        frmOrderBook.Show(this);
    }

    private void btnTrade_Click(object sender, EventArgs e)
    {
        FrmTrade f = new();
        f.ShowDialog(this);
    }

    private async void btnIndicator_Click(object sender, EventArgs e)
    {
        /**_indicators*******************
        {
           "Idicators":[
              {"SMA": ["12;2;-45698","27;2;-654433","99;3;-324466"]},
              {"SMMA":["12;2;-45698","27;2;-654433","99;3;-324466"]}
           ]
        }
        **********************************/
        FrmIndicator f = new(_indicators);
        if (f.ShowDialog(this) == DialogResult.OK)
        {
            _indicators = f.IndicatorsString; 
            await Charty.DrawIndicators(_indicators);
        }
    }

    private void FrmChart_FormClosing(object sender, FormClosingEventArgs e)
    {
        Charty.UnsubKlineSocket();
        Utils.SaveFormPosition(this);
        Utils.SaveIndicators(_indicators);
    }
}
