using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;

namespace bot2;

public partial class FrmMain : Form
{
    Charty Charty;
    bool IsLoadingProducts = false;

    public FrmMain()
    {
        InitializeComponent();
        Charty = new(chart);
        Charty.OnLastKline += OnLastKline;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cbExchange.Items.Clear();
        foreach (var ex in Charty.Exchanges) cbExchange.Items.Add(ex.Name);

        cbInterval.SelectedIndex = 3;
        cbExchange.SelectedIndex = 0;

        chart.MouseWheel += chart_MouseWheel;

        Charty.Exchange = 1;
        LoadProducts(Charty.Exchange);

        new FrmLog().Show(this);
    }
    void OnLastKline(Kline k)
    {
        Invoke(new Action(() =>
        {
            lblSymbol.Text = $"{Charty.Symbol}({k.ClosePrice})";
        }));
    }
    private void button1_Click(object sender, EventArgs? e)
    {
        if (IsLoadingProducts ||
            Charty.Symbol == "") return;

        Charty.GetKlines();
        Charty.populate();

        Task.Run(() =>
        {
            Thread.Sleep(100);
            Invoke(new Action(() =>
            {
                btnZoomIn_Click(this, e);
                btnZoomOut_Click(this, e);
            }));
        });
    }

    private void btnZoomOut_Click(object sender, EventArgs e)
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom -= 10;
        else
            Charty.Zoom--;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        Charty.populate();
    }

    private void btnZoomIn_Click(object sender, EventArgs e)
    {
        if (Control.ModifierKeys == Keys.Control)
            Charty.Zoom += 10;
        else
            Charty.Zoom++;

        lblZoom.Text = "Zoom: " + Charty.Zoom;
        Charty.populate();
    }
    void chart_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (e.Delta > 0)
            btnZoomIn_Click(this, e);
        else
            btnZoomOut_Click(this, e);
    }

    private void cbInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        Charty.Interval = cbInterval.Text;
        button1_Click(sender, e);
    }
    private void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
    {
        Charty.Exchange = cbExchange.SelectedIndex + 1;
        LoadProducts(Charty.Exchange);
    }

    private void lbProducts_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSymbol.Text = Charty.Symbol = lbProducts.Text;
        button1_Click(this, e);
    }

    void LoadProducts(int exha)
    {
        IsLoadingProducts = true;
        lbProducts.DataSource = null;

        using (CaDbContext dbContext = new CaDbContext())
        {
            var prods = dbContext.Products?.FromSql($"Get_Products2 {Charty.Exchange}");

            lbProducts.DisplayMember = "symbol";
            lbProducts.DataSource = prods?.ToList();
        }
        IsLoadingProducts = false;
        button1_Click(this, null);
    }
}