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

        var bals = await _exchange.GetBalances();
        OnAccountPositionUpdate(bals);

        _exchange.SubscribeToSpotAccountUpdates();
        _exchange.OnAccPositionUpdate += OnAccountPositionUpdate;
        _exchange.OnAccBalanceUpdate += OnAccountBalanceUpdate;
        _exchange.OnTickerUpdate += OnLastPriceUpdated;
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
            var ord = await _exchange.GetLastSpotOrder(symbol);
            p.Purchase = (decimal)ord.Price!;
            p.Current = (decimal)res.LastPrice!;
            _position.Add(p);

            int subsId = await _exchange.SubsсribeToTicker(symbol);
        }

        if (IsDisposed) return;
        try
        {
            Invoke(() =>
            {
                lblBnbAvailable.Text = $"{_comisBal.Asset}: {_comisBal.Total}";
                lblUsdtAvailable.Text = $"{_quoteBal.Asset}: {_quoteBal.Total}";
                lblBalance.Text = $"Balance: {_totalEquity.ToString().TrimEnd('0')}";

                dataGridView1.DataSource = _position;

                btnTrade.Enabled = _position.Count > 0;
            });
        }
        catch (Exception ex) { Log.Error("OnAccountPositionUpdate", ex.Message); }
    }

    void OnAccountBalanceUpdate(string asset, decimal delta)
    {
        Log.Info("OnAccountBalanceUpdate", $"{asset}/{delta}");
        Invoke(() => { });
    }
    void OnLastPriceUpdated(Ticker t)
    {
        if (_position.Count == 0) return;

        Position pos = _position.FirstOrDefault(p => p.Asset == t.Symbol.Replace("USDT", ""));
        decimal? delta = t.LastPrice - pos.Current;
        if (delta == 0) return;
        Log.Trace(_exchange.ID, "FrmAccount - OnLastPriceUpdated", $"{t.Symbol}/{t.LastPrice}");

        _position.Remove(pos);
        pos.Current = (decimal)t.LastPrice!;
        pos.Delta = (decimal)(t.LastPrice - pos.Purchase)!;

        if (pos.Purchase > 0)
            pos.Percent = 100 * (decimal)(t.LastPrice - pos.Purchase) / pos.Purchase!;

        _position.Add(pos);

        _totalEquity += (decimal)(pos.Total * delta)!;

        if (IsDisposed) return;
        Invoke(() =>
        {
            lblBalance.Text = $"Balance: {_totalEquity.ToString().TrimEnd('0')}";

            int r = -1;
            if (dataGridView1.SelectedRows.Count > 0)
                r = dataGridView1.SelectedRows[0].Index;

            dataGridView1.DataSource = _position.OrderBy(p => p.Asset).ToList();
            dataGridView1.Invalidate();

            if (dataGridView1.Rows.Count > 0 && r >= 0)
                dataGridView1.Rows[r].Selected = true;

            btnTrade.Enabled = _position.Count > 0;
        });
    }
    private void FrmAccount_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
    }

    private void btnTrade_Click(object sender, EventArgs e)
    {
        if(dataGridView1.SelectedRows.Count == 0) return;
        string symbol = dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + _quoteBal.Asset;
        FrmTrade frm = new(_exchange, symbol, false);
        frm.ShowDialog();
    }
}

struct Position
{
    public string Asset { get; set; }
    public decimal Total { get; set; }
    public decimal Purchase { get; set; }
    public decimal Current { get; set; }
    public decimal Delta { get; set; }
    public decimal Percent { get; set; }
}