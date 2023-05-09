using bot2.Tools;
using CaDb;
using CaExch;
using CryptoExchange.Net.CommonObjects;
using Microsoft.EntityFrameworkCore;

namespace bot2;

public partial class FrmTrade : Form
{
    string? _base;
    string? _quote;
    string _symbol;
    int _exchangeId;

    Charty _charty;

    public FrmTrade(Charty chart)
    {
        InitializeComponent();

        _charty = chart;
        _symbol = _charty.Symbol;
        _exchangeId = _charty.Exchange.ID;

        GetBaseQuote();

        _charty.OnKlineUpdated += OnPriceUpdated;
    }

    private async void FrmTade_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);

        var r = await _charty.Exchange.GetTickerAsync(_symbol);
        lblPrice.Text = r.LastPrice.ToString();

        Text = _symbol;
        lblBase.Text = _base;
        lblQuote.Text = _quote;
    }

    void OnPriceUpdated(Kline k)
    {
        if (this.IsDisposed) return;
        Invoke(() =>
        {
            lblPrice.Text = k.ClosePrice.ToString();
        });
    }

    void GetBaseQuote()
    {
        using (CaDbContext dbContext = new())
        {
            var prods = dbContext.Products?.FromSql($"Sinex_Get_Products {_exchangeId}, {_symbol}");
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
}
