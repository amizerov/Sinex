using amLogger;
using bot2.Tools;
using CaExch;
using CryptoExchange.Net.CommonObjects;

namespace bot2;

public partial class FrmAccount : Form
{
    AnExchange _exchange;
    List<Position> _position;

    Balance _comisBal = new() { Asset = "BNB" };
    Balance _quoteBal = new() { Asset = "USDT" };

    decimal _totalEquity;

    public FrmAccount(AnExchange exch)
    {
        InitializeComponent();
        _exchange = exch;
        _position = new();
    }

    private async void FrmAccount_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);

        _exchange.SubscribeToSpotAccountUpdates();
        _exchange.OnAccPositionUpdate += OnAccountPositionUpdate;
        _exchange.OnAccBalanceUpdate += OnAccountBalanceUpdate;
        _exchange.OnTickerUpdate += OnLastPriceUpdated;

        var bals = await _exchange.GetBalances();
        OnAccountPositionUpdate(bals);
    }

    async void OnAccountPositionUpdate(List<Balance> bals)
    {
        _comisBal = bals.First(b => b.Asset == _comisBal.Asset)!;
        _quoteBal = bals.First(b => b.Asset == _quoteBal.Asset)!;

        _totalEquity = (decimal)_quoteBal.Total!;
        //var res = await _excha.GetTickerAsync(bnb.Asset + usd.Asset);
        //bal += res.LastPrice * bnb.Total;

        foreach (var b in bals
                .Where(b => b.Total + b.Available > 0
                       && !(b.Asset == _comisBal.Asset || b.Asset == _quoteBal.Asset)))
        {
            string symbol = b.Asset + _quoteBal.Asset;
            var res = await _exchange.GetTickerAsync(symbol);
            _totalEquity += (decimal)(res.LastPrice * b.Total)!;

            Position p = new Position() { Asset = b.Asset, Total = (decimal)b.Total! };
            p.Current = (decimal)res.LastPrice!;
            _position.Add(p);

            _exchange.SubsсribeToTicker(symbol);
        }

        Invoke(() =>
        {
            lblBnbAvailable.Text = $"{_comisBal.Asset}: {_comisBal.Total}";
            lblUsdtAvailable.Text = $"{_quoteBal.Asset}: {_quoteBal.Total}";
            lblBalance.Text = $"Balance: {_totalEquity.ToString().TrimEnd('0')}";

            dataGridView1.DataSource = _position;
        });
    }

    void OnAccountBalanceUpdate(string asset, decimal delta)
    {
        Log.Info("OnAccountBalanceUpdate", $"{asset}/{delta}");
        Invoke(() => { });
    }
    void OnLastPriceUpdated(Ticker t)
    {
        Log.Info("OnLastPriceUpdated", $"{t.Symbol}/{t.LastPrice}");

        Position pos = _position.First(p => p.Asset == t.Symbol.Replace("USDT", ""));
        decimal? delta = t.LastPrice - pos.Current;
        if (delta == 0) return;

        _position.Remove(pos);
        pos.Current = (decimal)t.LastPrice!;
        _position.Add(pos);

        _totalEquity += (decimal)(pos.Total * delta)!;

        Invoke(() =>
        {
            if (this.IsDisposed) return;
            lblBalance.Text = $"Balance: {_totalEquity.ToString().TrimEnd('0')}";
            dataGridView1.DataSource = _position;
            dataGridView1.Invalidate();
        });
    }
    private void FrmAccount_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
    }
}

struct Position
{
    public string Asset { get; set; }
    public decimal Total { get; set; }
    public decimal Purchase { get; set; }
    public decimal Current { get; set; }
}