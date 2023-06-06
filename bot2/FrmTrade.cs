using amLogger;
using bot2.Tools;
using CaDb;
using CaExch;
using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

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

    int _tickerSubscriptionId = 0;

    public FrmTrade(AnExchange exch, string symbol, bool buySell = true)
    {
        InitializeComponent();

        _symbol = symbol;
        _excha = exch;
        _buySell = buySell;

        GetAssets();
    }

    private async void FrmTade_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this, false);

        tabControl1.SelectedIndex = _buySell ? 0 : 1;
        btnBuySell.Text = (_buySell ? "Buy " : "Sell ") + _base;

        var r = await _excha.GetTickerAsync(_symbol);
        lblPrice.Text = r.LastPrice.ToString();

        Text = _symbol;
        lblBase.Text = _base;
        lblQuote.Text = _quote;

        var res = await _excha.GetBalances();
        List<Balance> bals = res.ToList();
        Balance basee = bals.FirstOrDefault(b => b.Asset == _base)!;
        Balance quote = bals.FirstOrDefault(b => b.Asset == _quote)!;
        _baseAvailable = (decimal)basee.Available!;
        _quoteAvailable = (decimal)quote.Available!;

        lblAvlblBase.Text = "Available " + _baseAvailable;
        lblAvlblQuote.Text = "Available " + _quoteAvailable;

        _tickerSubscriptionId = await _excha.SubsсribeToTicker(_symbol);
        _excha.OnTickerUpdate += OnLastPriceUpdated;
    }

    void OnLastPriceUpdated(Ticker t)
    {
        try
        {
            if (decimal.Parse(lblPrice.Text) - t.LastPrice == 0) return;
            if (this.IsDisposed) return;

            Invoke(() => lblPrice.Text = t.LastPrice.ToString());
        }
        catch {}
        Log.Trace("FrmTrade - OnLastPriceUpdated", $"{t.Symbol}/{t.LastPrice}");
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
        _excha.UnSubFromTicker(_tickerSubscriptionId);
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
        _buySell = tabControl1.SelectedIndex == 0;
        btnBuySell.Text = (_buySell ? "Buy " : "Sell ") + _base;
        btnBuySell.BackColor = _buySell ? Color.DarkSeaGreen : Color.IndianRed;
    }

    private async void btnBuySell_Click(object sender, EventArgs e)
    {
        bool res = false;
        decimal quontity = decimal.Parse(txtBase.Text);
        if (_buySell)
        {
            res = await _excha.PlaceSpotOrderBuy(_symbol, quontity);
        }
        else
        {
            res = await _excha.PlaceSpotOrderSell(_symbol, quontity);
        }
        if (res)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            MessageBox.Show("Error");
        }
    }
}
