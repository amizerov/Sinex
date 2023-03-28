using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace bot2;

public partial class FrmMain : Form
{
    Charty Charty;
    FrmOrderBook frmOrders = new();
    FrmLogger frmLogger = new();

    public FrmMain()
    {
        InitializeComponent();
        Charty = new(chart);
        Charty.OnLastKline += OnLastKline;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        LoadFormPosition();

        cbExchange.Items.Clear();
        foreach (var ex in Charty.Exchanges) cbExchange.Items.Add(ex.Name);

        cbInterval.SelectedIndex = 3;
        cbExchange.SelectedIndex = 0;

        chart.MouseWheel += chart_MouseWheel;

        Charty.Exchange = 1;
        LoadProducts(Charty.Exchange);

        new FrmLogger().Show(this);
        frmOrders.Show(this);
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
    private void button1_Click(object sender, EventArgs? e)
    {
        if (Charty.Symbol == "") return;

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
        txtSeach_TextChanged(this, null);
        button1_Click(this, null);
    }

    private void txtSeach_TextChanged(object sender, EventArgs? e)
    {
        using (CaDbContext dbContext = new CaDbContext())
        {
            var prods = dbContext.Products?.FromSql($"Get_Products2 {Charty.Exchange}");

            lbProducts.DisplayMember = "symbol";
            lbProducts.DataSource = prods?.ToList()
                .Where(p => p.symbol.ToLower().Contains(txtSeach.Text.ToLower())).ToList();
        }
    }

    private void mnuSma_Click(object sender, EventArgs e)
    {
        Charty.DrawIndicator("sma");
    }

    private void mnuOrderBook_Click(object sender, EventArgs e)
    {
        if (frmOrders.IsDisposed)
            frmOrders = new();

        frmOrders.Visible = false;
        frmOrders.Show(this);
    }

    private void mnuLogger_Click(object sender, EventArgs e)
    {
        if (frmLogger.IsDisposed)
            frmLogger = new();

        frmLogger.Visible = false;
        frmLogger.Show(this);
    }
}

public partial class FrmMain : Form
{
    #region Form position

    string FileFormPosition =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FrmMainPosition.txt";

    private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        string pos = Top + ";" + Left + ";" + Width + ";" + Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    void LoadFormPosition()
    {
        if (File.Exists(FileFormPosition))
        {
            string[] pos = File.ReadAllText(FileFormPosition).Split(';');
            Top = int.Parse(pos[0]);
            Left = int.Parse(pos[1]);
            Width = int.Parse(pos[2]);
            Height = int.Parse(pos[3]); ;
        }
    }
    #endregion
}