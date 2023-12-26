using CoinsLoader;

namespace bot6;

public partial class FrmMain : Form
{
    public FrmMain()
    {
        InitializeComponent();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        AnExchange exchange = new Mexc();
        exchange.GetCoins();
    }
}
