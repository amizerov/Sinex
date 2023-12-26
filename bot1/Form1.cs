using amLogger;
using caLibProdStat;
using System.IO;

namespace bot1;

public partial class Form1 : Form
{
    bool paused = false;
    string gridLayoutXml = "";

    List<LogLine> logs = new();
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        amTools.Utils.RestoreFormPosition(this);
        Logger.Instance.Init((Log log) => DoLog(log));

        xtraTabControl1.SelectedTabPageIndex = 1;
        btnDbCheck.PerformClick();
        btnExn.PerformClick();

        logs.Add(new LogLine() { dt = DateTime.Now, id = 0, lvl = Level.None, src = "На дериба хорошая погода", msg = "На брайтон опять дожди" });
        gcLog.DataSource = logs;
        gvLog.Columns["dt"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
        gvLog.Columns["dt"].DisplayFormat.FormatString = "G";
        gvLog.BestFitColumns();


        gridLayoutXml = amTools.Utils.GetFileName(this, "xml");
        if (File.Exists(gridLayoutXml))
            gvLog.RestoreLayoutFromXml(gridLayoutXml);
    }
    void DoLog(Log log)
    {
        if (paused) return;
        if (Tag?.ToString() == "FormClosing") return;

        Invoke(new Action(() =>
        {
            textBox1.Text =
                DateTime.Now.ToString("G") + " - " +
                log.id + " - " +
                log.src + " - " +
                log.msg + "\r\n" +
                textBox1.Text;

            logs.Add(new LogLine() { dt = DateTime.Now, id = log.id, lvl = log.lvl, src = log.src, msg = log.msg });
            //gcLog.DataSource = logs.OrderByDescending(l => l.dt).Take(100).ToList();
            //gcLog.RefreshDataSource();

            if (textBox1.Text.Length > 5000)
                textBox1.Text = textBox1.Text.Substring(0, 5000);

            status.Text = logs.Count + " - " + logs.Where(l => l.id == 6).Count();
        }));
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        amTools.Utils.SaveFormPosition(this);
        gvLog.SaveLayoutToXml(gridLayoutXml);

        Tag = "FormClosing";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (CaInfo.IsDbConnectionOk) textBox1.Text = "Db Connection is OK";
        else textBox1.Text = "Error: CaDb is not connected ..";
    }

    private void button2_Click(object sender, EventArgs e)
    {
        textBox1.Text += "\r\n--------------\r\n";
        foreach (var exch in CaInfo.Exchanges)
        {
            textBox1.Text += exch.ID + " - " + exch.Name + "\r\n";
        }
        textBox1.Text += CaInfo.Exchanges.Count.ToString();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        if (btnStart.Tag == null)
            Task.Run(ExecuteAsync);

        if (btnStart.Tag?.ToString() == "1")
        {
            paused = true;
            btnStart.Tag = 0;
            btnStart.Text = "Resume";

            gcLog.DataSource = logs.OrderByDescending(l => l.dt);
            gcLog.RefreshDataSource();
            xtraTabControl1.SelectedTabPageIndex = 0;
        }
        else
        {
            paused = false;
            btnStart.Tag = 1;
            btnStart.Text = "Pause";
            xtraTabControl1.SelectedTabPageIndex = 1;
        }
    }
    void ExecuteAsync()
    {
        ProductsUpdater.Start(OnComplete);
    }
    void OnComplete()
    {
        Invoke(new Action(() =>
        {
            textBox1.Text = "Task Completed";
            logs.Add(new LogLine() { dt = DateTime.Now, src = "OnComplete", msg = "Task Completed" });
            gcLog.DataSource = logs.OrderByDescending(l => l.dt);
            gcLog.RefreshDataSource();
        }));
    }
}

public class LogLine
{
    public DateTime dt { get; set; }
    public int id { get; set; } = 0;
    public Level lvl { get; set; } = 0;
    public string src { get; set; } = "";
    public string msg { get; set; } = "";
}