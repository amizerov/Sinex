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

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }
}