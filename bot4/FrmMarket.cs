using CaDb;
using CaExch;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Windows.Controls;

namespace bot4;

public partial class FrmMarket : XtraForm
{

    public FrmMarket()
    {
        InitializeComponent();
    }

    private void FrmMarket_Load(object sender, EventArgs e)
    {
        Tools.LoadFormPosition(this);

        cbExchange.Properties.Items.AddRange(new CaExchanges());
        cbExchange.SelectedIndex = 0;

        gvProducts.RestoreLayoutFromXml("gvProducts.xml");
    }
    void LoadQuoteAssets()
    {
        cbQuote.Properties.Items.Clear();
        using (CaDbContext dbContext = new())
        {
            int ExId = ((AnExchange)cbExchange.SelectedItem).ID;
            var quotes = dbContext.Quotes?.FromSqlRaw($"Sinex_Get_QuoteAssets {ExId}");
            cbQuote.Properties.Items.AddRange(quotes?.ToList());
        }
        cbQuote.SelectedIndex = 0;
    }
    void LoadProducts()
    {
        using (CaDbContext dbContext = new())
        {
            int ExId = ((AnExchange)cbExchange.SelectedItem).ID;

            var prods = dbContext.Products?.FromSql($"Sinex_Get_Products {ExId}");

            gcProducts.DataSource = prods?.ToList().Where(p => p.quoteasset == cbQuote.Text).ToList();
        }
    }
    private void FrmMarket_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tools.SaveFormPosition(this);
        gvProducts.SaveLayoutToXml("gvProducts.xml");
    }

    private void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadQuoteAssets();
        LoadProducts();
    }

    private void cbQuote_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void gvProducts_DoubleClick(object sender, EventArgs e)
    {
        FrmChart frmChart = new();
        frmChart.Show();
    }
}
