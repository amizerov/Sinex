using amLogger;
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
        foreach (var exchange in Exchanges.List())
        {
            exchange.GetCoins();
        }
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
