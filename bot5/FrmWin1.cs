using Binance.Net.Objects.Models.Futures;
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
                .SqlQuery<ProdEx>($"Sinex_GetProductsExch {txtSearch.Text}");

            dgvProds.DataSource = prods.ToList();
            //dgvProds.Columns[2].Visible = false;
            dgvProds.Columns[3].Visible = false;
            dgvProds.Columns[4].Visible = false;
            dgvProds.Columns[5].Visible = false;

            dgvProds.Columns[0].Width = 60;
            dgvProds.Columns[1].Width = 80;
            dgvProds.Columns[2].Width = 50;
        }
        _loaded = true;
        //ShowPrices();
        DoIt();
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

    async Task<Stat> CalcSt(int[] arrExchanges, string asset)
    {
        Stat st = new();
        foreach (var ex in arrExchanges)
        {
            AnExchange exch = _exs.Find(x => x.ID == ex)!;
            var p = await GetPriceVolum(exch, asset!);
            st.Add(new PriceSt() { 
                exchange = exch, symbol = asset, 
                price = p.Item1, 
                volum = p.Item2
            });
        }
        st.work();
        return st;
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
            if (p.Item1 == 0) continue;

            st.Add(new PriceSt() { 
                exchange = exchange, symbol = ass, 
                price = p.Item1, volum = p.Item2 
            });
        }
        st.work();
        lblExc1.Text = st.exc1!.Name;
        lblExc2.Text = st.exc2!.Name;
        lblMaxProc.Text = st.proc + "%";
    }
    async Task<(decimal, decimal)> GetPriceVolum(AnExchange exc, string ass)
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
        //var t = await exc.GetTickerAsync(symbol);
        //if(t == null) return (0, 0);

        var k = await exc.GetKlines(symbol, "1m", 10);
        if (k == null || k.Count == 0) return (0, 0);
        var s = k.Last();
        var v = s.Volume;
        var p = s.ClosePrice;

        decimal? price = p; // t.LastPrice;
        decimal? volum = v; // t.LastPrice;

        return ((decimal)price!, (decimal)volum!);
    }
    async Task<(decimal, decimal)> AddLabel(AnExchange exc, string ass)
    {
        (decimal price, decimal volum) = await GetPriceVolum(exc, ass);
        if(price == 0) return (0, 0);

        int c = panel.Controls.Count + 2;
        Label lblEx = new();
        lblEx.Text = exc.Name;
        lblEx.Width = 100; lblEx.Top = 30 * c / 2; lblEx.Left = 11;
        Label lblPr = new();
        lblPr.Text = price.ToString();
        lblPr.Width = 200; lblPr.Top = lblEx.Top; lblPr.Left = 111;
        panel.Controls.Add(lblEx);
        panel.Controls.Add(lblPr);

        return (price, volum);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void btnArbit_Click(object sender, EventArgs e)
    {
        FrmWin2 f = new();
        f.Top = this.Top;
        f.Left = this.Left + this.Width + 11;
        f.Show();
    }

    async void DoIt()
    {
        await Task.Run(async () =>
        {
            foreach (DataGridViewRow r in dgvProds.Rows)
            {
                var ass = r.Cells[0].Value.ToString();
                var ens = r.Cells[1].Value.ToString();
                var aex = ens!.Split(',');
                int[] ints = new int[aex.Length];
                for (int i = 0; i < aex.Length; i++) ints[i] = Convert.ToInt32(aex[i]);

                Stat st = await CalcSt(ints, ass!);
                if (st == null || st.exc1 == null || st.exc2 == null) return;

                r.Cells[2].Value = st.proc.ToString();

                using (CaDbContext db = new())
                {
                    db.Database.ExecuteSql(
                        @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage
                        update Sinex_Arbitrage 
                            set procDiffer={st.proc},
                                exch1={st.exc1!.Name}, 
                                exch2={st.exc2!.Name},
                                vol1={st.vol1},
                                vol2={st.vol2}             
                        where 
                            shotNumber=@n 
                            and baseAsset={ass}
                            and quoteAsset='USDT'"
                    );
                }
            }
        });
    }
}

class ProdEx
{
    public string? symbol { get; set; }
    public string? exc { get; set; }
    public string? d { get; set; }
    public int c { get; set; }
    public DateTime dmin { get; set; }
    public DateTime dmax { get; set; }
}

class PriceSt
{
    public AnExchange? exchange { get; set; }
    public string? symbol { get; set; }
    public decimal? price { get; set; }
    public decimal? volum { get; set; }
}
class Stat : List<PriceSt>
{
    public AnExchange? exc1 { get; set; }
    public AnExchange? exc2 { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal vol1 { get; set; }
    public decimal vol2 { get; set; }

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
                        exc1 = i.exchange; vol1 = (decimal)i.volum!;
                        exc2 = j.exchange; vol2 = (decimal)j.volum!;
                    }
                    else
                    {
                        exc2 = i.exchange; vol2 = (decimal)j.volum!;
                        exc1 = j.exchange; vol1 = (decimal)i.volum!;
                    }
                }
            }
        }
        proc = 100 * proc / (decimal)this.Max(a => a.price)!;
        proc = Math.Round(proc, 2);
    }
}