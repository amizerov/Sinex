
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
        Left = Owner!.Right-20;
        Height = Owner!.Height;

        Logger.Instance.Init((Log log) =>
            textBox1.Text =
                DateTime.Now.ToString("G") + " - " +
                log.msg + "\r\n" +
                textBox1.Text
        );
    }
}
