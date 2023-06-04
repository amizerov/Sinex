using bot2.Tools;
using CaExch;

namespace bot2;

public partial class FrmAccount : Form
{
    AnExchange _excha;
    public FrmAccount(AnExchange exch)
    {
        InitializeComponent();
        _excha = exch;
    }

    private async void FrmAccount_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);

        _excha.SubscribeToSpotAccountUpdates();
        _excha.OnAccountUpdate += OnAccountUpdate;

        var bals = await _excha.GetBalances();
        OnAccountUpdate(bals);
    }

    void OnAccountUpdate(List<Balance> bals)
    {
        string commis = "BNB";
        string balans = "USDT";

        Balance bnb = bals.FirstOrDefault(b => b.Asset == commis)!;
        Balance usd = bals.FirstOrDefault(b => b.Asset == balans)!;
        List<Balance> balances = bals
                .Where(b => b.Total + b.Available > 0
                       && !(b.Asset == commis || b.Asset == balans)).ToList();

        Invoke(() => {
            lblBnbAvailable.Text = $"{bnb.Asset}: {bnb.Available}";
            lblUsdtAvailable.Text = $"{usd.Asset}: {usd.Available}";
            dataGridView1.DataSource = balances;
        });
    }

    private void FrmAccount_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
    }
}
