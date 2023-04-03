using amLogger;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace bot2;

public partial class FrmLogger : Form
{
    public FrmLogger()
    {
        InitializeComponent();
    }

    private void FrmLog_Load(object sender, EventArgs e)
    {
        LoadFormPosition();
        Logger.Instance.Init((Log log) => DoLog(log));
    }
    void DoLog(Log log)
    {
        if (Tag?.ToString() == "111") return;

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

            using(CaDb.CaDbContext ca = new())
            {
                ca.Database.ExecuteSql($"CaLogger.dbo.DoSinexLogging {log.id}, {log.type}, {log.lvl}, {log.src}, {log.msg}");
            }
        }
        catch { }
    }

    #region Form position
    private void FrmLog_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tag = "111";

        string pos = Top + ";" + Left + ";" + Width + ";" + Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    void LoadFormPosition()
    {
        if (File.Exists(FileFormPosition))
        {
            string[] pos = File.ReadAllText(FileFormPosition).Split(';');
            Top = int.Parse(pos[0]);
            Left = int.Parse(pos[1]);
            Width = int.Parse(pos[2]);
            Height = int.Parse(pos[3]); ;
        }
    }
    string FileFormPosition =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FrmLoggerPosition.txt";
    #endregion
}
