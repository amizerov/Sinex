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
        List<Balance> balances = new List<Balance>();
        var bals = await _excha.GetBalances();
        foreach (var bal in bals)
        {
            if (bal.Total > 0)
                balances.Add(bal);
        }
        dataGridView1.DataSource = balances.ToList();
    }
}
