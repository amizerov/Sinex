using bot2.Tools;
using CaDb;
using CaExch;
using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;

namespace bot2;

public partial class FrmTrade : Form
{
    bool _buySell = true;

    decimal _baseAvailable;
    decimal _quoteAvailable;

    string? _base;
    string? _quote;
    string _symbol;
    AnExchange _excha;

    Charty _charty;

    public FrmTrade(Charty chart)
    {
        InitializeComponent();

        _charty = chart;
        _symbol = _charty.Symbol;
        _excha = _charty.Exchange;

        GetAssets();

        _charty.OnKlineUpdated += OnPriceUpdated;
    }

    private async void FrmTade_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this, false);

        var r = await _excha.GetTickerAsync(_symbol);
        lblPrice.Text = r.LastPrice.ToString();

        Text = _symbol;
        lblBase.Text = _base;
        lblQuote.Text = _quote;

        btnBuySell.Text = "Buy " + _base;

        var res = await _excha.GetBalances();
        List<CaExch.Balance> bals = res.ToList();

        _baseAvailable = bals.FirstOrDefault(b => b.Asset == _base)!.Available;
        _quoteAvailable = bals.FirstOrDefault(b => b.Asset == _quote)!.Available;

        lblAvlblBase.Text = "Available " + _baseAvailable;
        lblAvlblQuote.Text = "Available " + _quoteAvailable;
    }

    void OnPriceUpdated(Kline k)
    {
        if (this.IsDisposed) return;
        Invoke(() => lblPrice.Text = k.ClosePrice.ToString());
    }

    void GetAssets()
    {
        using (CaDbContext dbContext = new())
        {
            var prods = dbContext.Products?.FromSql($"Sinex_Get_Products {_excha.ID}, {_symbol}");
            if (prods != null)
            {
                List<Product> ps = prods.ToList();
                if (ps == null || ps.Count == 0) return;

                Product? p = ps.FirstOrDefault();

                if (p != null)
                {
                    _base = p.baseasset;
                    _quote = p.quoteasset;
                }
            }
        }
    }

    private void FrmTrade_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
    }

    private void txtBase_TextChanged(object sender, EventArgs e)
    {
        if (txtQuote.Focused) return;
        txtQuote.Text = decimal.Parse(txtBase.Text) * decimal.Parse(lblPrice.Text) + "";
    }

    private void txtQuote_TextChanged(object sender, EventArgs e)
    {
        if (txtBase.Focused) return;
        txtBase.Text = decimal.Parse(txtQuote.Text) / decimal.Parse(lblPrice.Text) + "";
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(tabControl1.SelectedIndex == 0)
        {
            btnBuySell.BackColor = Color.DarkSeaGreen;
            btnBuySell.Text = "Buy " + _base;
            _buySell = true;
        }
        else
        {
            btnBuySell.BackColor = Color.IndianRed;
            btnBuySell.Text = "Sell " + _base;
            _buySell = false;
        }
    }
}
