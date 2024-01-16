using amLogger;
using CoinsLoader;
using Microsoft.Extensions.Logging;

namespace bot6;

public partial class FrmMain : Form
{
    public FrmMain()
    {
        InitializeComponent();
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        List<AnExchange> exchanges = new();
        exchanges.Add(new AscendEx());
        exchanges.Add(new BingX());
        exchanges.Add(new BitGet());
        exchanges.Add(new Bybit());
        exchanges.Add(new BitMart());
        exchanges.Add(new Kucoin());
        exchanges.Add(new Mexc());
        exchanges.Add(new Gate());
        exchanges.Add(new CoinEx());

        AnExchange exchange = new CoinEx();
        exchange.GetCoins();
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        Logger.Instance.Init((Log log) => DoLog(log));
    }

    private void DoLog(Log log)
    {
        if(IsDisposed) return;
        Invoke(() => { 
            txt.Text =
                DateTime.Now.ToString("G") + " - " +
                log.id + " - " +
                log.src + " - " +
                log.msg + "\r\n" +
                txt.Text;
        });
    }
}
