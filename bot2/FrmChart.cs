using CaExch;
using CryptoExchange.Net.CommonObjects;

namespace bot2;

public partial class FrmChart : Form
{
    Charty Charty;

    public FrmChart(AnExchange exch, string symbo)
    {
        InitializeComponent();

        Charty = new(chart, exch, symbo);
        Charty.OnLastKline += OnLastKline;

        foreach (var interval in Charty.Exchange.Intervals)
            cbInterval.Items.Add(interval);

        cbInterval.SelectedIndex = 3;
        Charty.Symbol = symbo;

    }

    private void FrmChart_Load(object sender, EventArgs e)
    {
        chart.MouseWheel += chart_MouseWheel;
        InitChart();
    }
    private async void InitChart()
    {
        if (Charty.Symbol == "") return;

        await Charty.GetKlines();
        Charty.populate();
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
    void chart_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (e.Delta > 0)
            ZoomIn();
        else
            ZoomOut();
    }
    private void ZoomOut()
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom -= 10;
        else
            Charty.Zoom--;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        Charty.populate();
    }

    private void ZoomIn()
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom += 10;
        else
            Charty.Zoom++;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        Charty.populate();
    }

    private void cbInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        Charty.SetInterval(cbInterval.Text);
        InitChart();
    }

    private void FrmChart_FormClosing(object sender, FormClosingEventArgs e)
    {
        Charty.UnsubKlineSocket();
    }
}
