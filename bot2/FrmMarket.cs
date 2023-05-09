using bot2.Tools;
using CaDb;
using CaExch;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace bot2;

public partial class FrmMarket : Form
{
    FrmLogger frmLogger = new();

    public List<AnExchange> Exchanges = new(){
    new CaBinance(),
    new CaKucoin(),
    new CaHuobi(),
    new CaBittrex(),
    new CaBybit()
};
    public FrmMarket()
    {
        InitializeComponent();
    }

    private void FrmMarket_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);

        cbExchange.DisplayMember = "Name";
        foreach (var exchange in Exchanges)
        {
            cbExchange.Items.Add(exchange);
        }
        cbExchange.SelectedIndex = 0;

        frmLogger.Show(this);
    }
    void LoadProducts()
    {
        using (CaDbContext dbContext = new())
        {
            int ExId = ((AnExchange)cbExchange.SelectedItem).ID;

            string search = txtSearch.Text.ToLower();

            var prods = dbContext.Products?.FromSql($"Sinex_Get_Products {ExId}, {search}");

            dgProducts.DataSource = prods?.ToList().Where(p => p.quoteasset == cbQuote.Text).ToList();
        }
    }

    void LoadQuoteAssets()
    {
        using (CaDbContext dbContext = new())
        {
            int ExId = ((AnExchange)cbExchange.SelectedItem).ID;

            var quotes = dbContext.Quotes?.FromSqlRaw($"Sinex_Get_QuoteAssets {ExId}");

            cbQuote.DisplayMember = "quoteasset";
            cbQuote.DataSource = quotes?.ToList();
        }
    }
    private void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadQuoteAssets();
        LoadProducts();
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void dgProducts_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
    {
        DataGridViewColumnCollection cols = dgProducts.Columns;
        cols[0].Visible = false;
        cols[2].Visible = false;
        foreach (DataGridViewColumn c in cols)
        {
            if (c.Index > 5) c.Visible = false;
        }
    }

    private void dgProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        string? ex = dgProducts.Rows[e.RowIndex].Cells[2].Value.ToString();
        string? sy = dgProducts.Rows[e.RowIndex].Cells[1].Value.ToString();
        if (sy == null || ex == null) return;
        int exch_id = int.Parse(ex);
        string symbo = sy;

        AnExchange? exchange = Exchanges.FirstOrDefault(exch => exch.ID == exch_id);
        if (exchange == null)
        {
            MessageBox.Show("Exchange selection problem");
            return;
        }
        FrmChart ch = new(exchange, symbo);
        ch.Show();
    }

    private void FrmMarket_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
    }

    private void cbQuote_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void btnBot1_Click(object sender, EventArgs e)
    {
        string p = "D:\\Projects\\CryptoTrading\\Sinex\\" +
                   "bot1\\bot1\\bin\\Debug\\net7.0-windows\\";
        string f = "bot1.exe";
        if(File.Exists(p + f))
        {
            Process.Start(p + f);
        }
    }
}
