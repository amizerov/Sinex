using bot2.Tools;
using CaExch;
using CryptoExchange.Net.CommonObjects;

namespace bot2;

public partial class FrmChart : Form
{
    Charty Charty;
    FrmStakan? frmOrderBook;
    public FrmChart(AnExchange exch, string symbo)
    {
        InitializeComponent();

        Charty = new(chart, exch, symbo);
        Charty.OnKlineUpdated += OnLastKline;
        Charty.NeedToRepopulate += OnNeedToRepopulate;

        foreach (var interval in Charty.Exchange.Intervals)
            cbInterval.Items.Add(interval);

        cbInterval.SelectedIndex = 3;
        Charty.Symbol = symbo;

        Text = exch.Name + " - " + symbo + " - Chart";
    }

    private void FrmChart_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);
        chart.MouseWheel += chart_MouseWheel;
        SetTitle();
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
    void OnNeedToRepopulate()
    {
        Invoke(new Action(() =>
        {
            lblZoom.Text = "Zoom: " + Charty.Zoom;
            Charty.populate();
        }));
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
            Charty.Zoom += 10;
        else
            Charty.Zoom++;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        Charty.populate();
    }

    private void ZoomIn()
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom -= 10;
        else
            Charty.Zoom--;

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
        Utils.SaveFormPosition(this);
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
    }

    void SetTitle()
    {
        var title = chart.Titles[0];
        title.Alignment = ContentAlignment.TopLeft;
        title.BackColor = Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(120)))), ((int)(((byte)(160)))), ((int)(((byte)(240)))));
        title.BorderColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        title.Font = new Font("Trebuchet MS", 14.25F, System.Drawing.FontStyle.Bold);
        title.ForeColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
        title.Name = "Title1";
        title.Position.Auto = false;
        title.Position.Height = 3.5F;
        title.Position.Width = 50F;
        title.Position.X = 2F;
        title.Position.Y = 2F;
        title.ShadowColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        title.Text = "Chart Control for .NET Framework";
    }
}
