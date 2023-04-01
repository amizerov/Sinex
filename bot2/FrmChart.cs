using CryptoExchange.Net.CommonObjects;

namespace bot2;

public partial class FrmChart : Form
{
    Charty Charty;

    public FrmChart(int exch_id, string symbo)
    {
        InitializeComponent();
        Charty = new(chart);
        Charty.OnLastKline += OnLastKline;
        Charty.Exchange = exch_id;
        Charty.Symbol = symbo;
    }

    private void FrmChart_Load(object sender, EventArgs e)
    {
        cbInterval.SelectedIndex = 3;
        chart.MouseWheel += chart_MouseWheel;
        InitChart();
    }
    private void InitChart()
    {
        if (Charty.Symbol == "") return;

        Charty.GetKlines();
        Charty.populate();

        Task.Run(() =>
        {
            Thread.Sleep(100);
            Invoke(new Action(() =>
            {
                ZoomIn();
                ZoomOut();
            }));
        });
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
}
