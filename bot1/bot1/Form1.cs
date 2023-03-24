using amLogger;
using caLibProdStat;

namespace bot1;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
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

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tag = "111";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (CaInfo.IsDbConnectionOk) textBox1.Text = "Db Connection is OK";
        else textBox1.Text = "Error: CaDb is not connected ..";
    }

    private void button2_Click(object sender, EventArgs e)
    {
        textBox1.Text = CaInfo.ExchasList.Count.ToString();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        Task.Run(ExecuteAsync);
    }
    void ExecuteAsync()
    {
        bool done = false;
        ProductsUpdater.Start(() => done = true);
        while (!done)
            Thread.Sleep(1000);
    }

}