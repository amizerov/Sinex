using CaExch;
namespace bot2;

public partial class FrmMain : Form
{
    Charty Charty;
    string Symbol = "BUSDUSDT";

    public FrmMain()
    {
        InitializeComponent();
        Charty = new(chart);
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        cbInterval.SelectedIndex = 3;
        cbExchange.SelectedIndex = 0;

        chart.MouseWheel += chart_MouseWheel;
        LoadProducts(cbExchange.SelectedIndex + 1);

        button1_Click(sender, e);

        new FrmLog().Show(this);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Charty.GetKlines(cbExchange.SelectedIndex + 1, Symbol, cbInterval.Text);
        Charty.populate();
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
        button1_Click(sender, e);
    }
    private void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProducts(cbExchange.SelectedIndex + 1);
    }

    private void lbProducts_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblSymbol.Text = Symbol = lbProducts.Text;
        button1_Click(this, e);
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

    void LoadProducts(int exha)
    {
        using (CaDbContext dbContext = new CaDbContext())
        {
            var prods = from p in dbContext.Products
                        where p.exchange == exha
                              && p.Version == 1
                              && p.cnt1 > 60
                              && p.cnt3 == 100
                              && p.liquidity > 90
                        orderby p.volatility descending, p.cnt3 descending
                        select p.symbol;

            lbProducts.DataSource = prods.ToList();
        }
    }
}