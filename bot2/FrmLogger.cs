using amLogger;
using bot2.Tools;
using Microsoft.EntityFrameworkCore;

namespace bot2;

public partial class FrmLogger : Form
{
    public FrmLogger()
    {
        InitializeComponent();
    }

    private void FrmLog_Load(object sender, EventArgs e)
    {
        Utils.LoadFormPosition(this);
        Logger.Instance.Init((Log log) => DoLog(log));
    }
    void DoLog(Log log)
    {
        if (Tag?.ToString() == "111") return;
        if (this.IsDisposed) return;
        try
        {
            Invoke(new Action(() =>
            {
                TextBox txt = new TextBox();
                switch (log.lvl)
                {
                    case Level.Trace:
                        txt = txtTrace; break;
                    case Level.Info:
                        txt = txtInfo; break;
                    case Level.Error:
                        txt = txtError; break;
                }

                txt.Text =
                    DateTime.Now.ToString("G") + " - " +
                    log.id + " - " +
                    log.src + " - " +
                    log.msg + "\r\n" +
                    txt.Text;

                txtAll.Text =
                    DateTime.Now.ToString("G") + " - " +
                    log.id + " - " +
                    log.src + " - " +
                    log.msg + "\r\n" +
                    txtAll.Text;
            }));

            using (CaDb.CaDbContext ca = new())
            {
                ca.Database.ExecuteSql($"CaLogger.dbo.DoSinexLogging {log.id}, {log.type}, {log.lvl}, {log.src}, {log.msg}");
            }
        }
        catch { }
    }

    private void FrmLog_FormClosing(object sender, FormClosingEventArgs e)
    {
        Utils.SaveFormPosition(this);
        Tag = "111";
    }
}
