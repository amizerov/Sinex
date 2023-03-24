
using amLogger;

namespace bot2;

public partial class FrmLog : Form
{
    public FrmLog()
    {
        InitializeComponent();
    }

    private void FrmLog_Load(object sender, EventArgs e)
    {
        Top = Owner!.Top;
        Left = Owner!.Right - 20;
        Height = Owner!.Height;

        Logger.Instance.Init((Log log) => DoLog(log));
    }
    void DoLog(Log log)
    {
        if (Tag?.ToString() == "111") return;

        Invoke(new Action(() =>
        {
            textBox1.Text = 
                DateTime.Now.ToString("G") + " - " +
                log.id + " - " +
                log.src + " - " +
                log.msg + "\r\n" +
                textBox1.Text;
        }));
    }

    private void FrmLog_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tag = "111";
    }
}
