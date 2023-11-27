using CaDb;
using CaExch;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Data;

namespace bot5;

public partial class FrmWin1 : Form
{
    bool _loaded = false;
    CaExchanges _exs = new();
    public FrmWin1()
    {
        InitializeComponent();
    }

    void LoadProducts()
    {
        _loaded = false;
        using (CaDbContext db = new())
        {
            dgvProds.DataSource = null;
            var prods = db.Database
                .SqlQuery<ProdEx>($"Sinex_GetProductsExchanges {txtSearch.Text}");

            dgvProds.DataSource = prods.ToList();
            dgvProds.Columns[2].Visible = false;
            dgvProds.Columns[3].Visible = false;
            dgvProds.Columns[4].Visible = false;
        }
        _loaded = true;
        ShowPrices();
    }

    private void FrmWin1_Load(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void dgvProds_SelectionChanged(object sender, EventArgs e)
    {
        if (!_loaded) return;

        ShowPrices();
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
        LoadProducts();
    }

    async void ShowPrices()
    {
        lblExc1.Text = lblExc2.Text = lblMaxProc.Text = "";
        if (dgvProds.Rows.Count == 0) return;

        var ass = dgvProds.CurrentRow.Cells[0].Value.ToString();
        lblSym.Text = ass;

        Stat st = new();
        var ens = dgvProds.CurrentRow.Cells[1].Value.ToString();
        var aex = ens!.Split(',');
        panel.Controls.Clear();
        foreach (var ex in aex)
        {
            AnExchange exchange = _exs.Find(x => x.ID == int.Parse(ex))!;
            var p = await AddLabel(exchange, ass!);
            st.Add(new PriceSt() { exchange = exchange, symbol = ass, price = p });
        }
        st.work();
        lblExc1.Text = st.exc1!.Name;
        lblExc2.Text = st.exc2!.Name;
        lblMaxProc.Text = st.proc + "%";
    }
    async Task<decimal> AddLabel(AnExchange exc, string ass)
    {
        string symbol = "";
        switch (exc.ID)
        {
            case 1:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 2:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 3:
                symbol = ass.ToLower() + "usdt";
                break;
            case 4:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 5:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 6:
                symbol = ass.ToLower() + "usdt";
                break;
            case 7:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 8:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 9:
                symbol = ass.ToUpper() + "USDT";
                break;
        }
        var t = await exc.GetTickerAsync(symbol);
        decimal? price = t.LastPrice;

        Label lblEx = new();
        lblEx.Text = exc.Name;
        lblEx.Width = 100;
        Label lblPr = new();
        lblPr.Text = price.ToString();
        lblPr.Width = 200;
        panel.Controls.Add(lblEx);
        panel.Controls.Add(lblPr);

        return (decimal)price!;
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void btnArbit_Click(object sender, EventArgs e)
    {
        (new FrmWin2()).Show();
    }
}

class ProdEx
{
    public string? symbol { get; set; }
    public string? exc { get; set; }
    public int c { get; set; }
    public DateTime dmin { get; set; }
    public DateTime dmax { get; set; }
}

class PriceSt
{
    public AnExchange? exchange { get; set; }
    public string? symbol { get; set; }
    public decimal? price { get; set; }
}
class Stat : List<PriceSt>
{
    public AnExchange? exc1 { get; set; }
    public AnExchange? exc2 { get; set; }
    public decimal proc { get; set; } = 0;

    public void work()
    {
        foreach (var i in
            this.Where(e => e.exchange!.ID != 4
                         && e.exchange!.ID != 1))
        {
            foreach (var j in
                this.Where(e => e.exchange!.ID != i.exchange!.ID
                             && e.exchange!.ID != 4
                             && e.exchange!.ID != 1))
            {
                var d = (decimal)(i.price - j.price)!;
                if (proc < Math.Abs(d))
                {
                    proc = d;
                    if (i.price > j.price)
                    {
                        exc1 = i.exchange;
                        exc2 = j.exchange;
                    }
                    else
                    {
                        exc2 = i.exchange;
                        exc1 = j.exchange;
                    }
                }
            }
        }
        proc = 100 * proc / (decimal)this.Max(a => a.price)!;
        proc = Math.Round(proc, 2);
    }
}