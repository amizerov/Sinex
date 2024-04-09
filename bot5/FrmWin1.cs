using amLogger;
using System.Diagnostics;
using TelegramBot1;

namespace bot5;

public partial class FrmWin1 : Form
{
    FrmWin2 frmArbitrage = new();

    bool _loaded = false;

    public FrmWin1()
    {
        InitializeComponent();
        (new FrmLog()).Show();
    }

    void LoadProducts()
    {
        _loaded = false;

        dgvProds.DataSource = null;

        dgvProds.DataSource = Db.GetProdsWithExchanges(txtSearch.Text);

        dgvProds.Columns[3].Visible = false;
        dgvProds.Columns[4].Visible = false;
        dgvProds.Columns[5].Visible = false;

        dgvProds.Columns[0].Width = 60;
        dgvProds.Columns[1].Width = 80;
        dgvProds.Columns[2].Width = 50;

        _loaded = true;
        statusCount.Text = dgvProds.Rows.Count.ToString();
    }

    private async void FrmWin1_Load(object sender, EventArgs e)
    {
        LoadProducts();

        await Telega.Init();
        Telega.cmdStart += ()
            => Invoke(() => {
                btnReload_Click(this, new EventArgs());
                btnScan_Click(this, new EventArgs());
            });
        Telega.cmdReset += ()
            => Invoke(() => {
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = "bot5.exe";
                    process.Start();
                    this.Close();
                }
                catch (Exception ex)
                {
                    Log.Error("Telega", ex.Message);
                }
            });
        Telega.cmdRecheckCoin += (coin)
            => Invoke(() =>
            {
                txtSearch.Text = coin;
                ShowAllStatistics();
            });

        Text = "Sinex bot 5, version: " + Application.ProductVersion;
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
        LoadProducts();
    }

    async void ShowAllStatistics()
    {
        if (!_loaded) return; _loaded = false;

        panel.Controls.Clear();
        lblExc1.Text = lblChain.Text = lblExc2.Text = lblMaxProc.Text = "";
        if (dgvProds.Rows.Count == 0) return;

        var ass = dgvProds.SelectedRows[0].Cells[0].Value.ToString();
        var exs = dgvProds.SelectedRows[0].Cells[1].Value.ToString();
        if (exs == null || ass == null) return;

        lblSym.Text = ass;
        FullStat st = await FullStat.Calculate(exs, ass, coinStat => AddLabel(coinStat));
        dgvProds.SelectedRows[0].Cells[2].Value = st.proc.ToString();

        if (st == null || st.excSell == null || st.excBuy == null)
        {
            Log.Warn(ass, "sciped");
            return;
        }

        await st.Update();

        lblExc1.Text = st.excBuy.Name;
        lblChain.Text = st.chain;
        lblExc2.Text = st.excSell.Name;
        lblMaxProc.Text = st.proc + "%";

        _loaded = true;
    }

    void AddLabel(CoinExchStat s)
    {
        int c = panel.Controls.Count + 4;
        Label lblEx = new();
        lblEx.Text = s.exchange!.ID + " - " + s.exchange!.Name;
        lblEx.Width = 90; lblEx.Top = 30 * c / 4; lblEx.Left = 10;
        Label lblPr = new();
        lblPr.Text = s.price.ToString();
        lblPr.Width = 100; lblPr.Top = lblEx.Top; lblPr.Left = lblEx.Left + lblEx.Width + 5;
        Label lblVo = new();
        lblVo.Text = s.volum.ToString();
        lblVo.Width = 110; lblVo.Top = lblEx.Top; lblVo.Left = lblPr.Left + lblPr.Width + 5;
        Label lblCh = new();
        lblCh.Text = s.coinChains
            .Aggregate<CoinChain, string>("", (cs, c) => cs = cs + "," + c.chainName);
        lblCh.Text = lblCh.Text.Length > 0 ? lblCh.Text.Substring(1) : "";
        lblCh.Width = 500; lblCh.Top = lblEx.Top; lblCh.Left = lblVo.Left + lblVo.Width + 5;

        panel.Controls.Add(lblEx);
        panel.Controls.Add(lblPr);
        panel.Controls.Add(lblVo);
        panel.Controls.Add(lblCh);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        int rowIndex = -1;
        var rs = dgvProds.Rows
            .Cast<DataGridViewRow>()
                .Where(r =>
                {
                    var c = r.Cells[0];
                    if (c == null) return false;
                    var v = c.Value;
                    if (v == null) return false;
                    string? s = v.ToString();
                    if (s == null) return false;

                    return s.ToUpper().StartsWith(txtSearch.Text.ToUpper());
                });
        if (rs.Count() == 0) return;
        DataGridViewRow row = rs.First();
        rowIndex = row.Index;
        dgvProds.Rows[rowIndex].Selected = true;
        dgvProds.FirstDisplayedScrollingRowIndex = rowIndex > 2 ? rowIndex - 1 : rowIndex;
    }

    private void btnArbit_Click(object sender, EventArgs e)
    {
        frmArbitrage = new();
        frmArbitrage.Show();
        frmArbitrage.Top = this.Top;
        frmArbitrage.Left = this.Left + this.Width + 11;
    }

    async void StartScan(Action? OnComplete = null)
    {
        await Task.Run(async () =>
        {
            foreach (DataGridViewRow r in dgvProds.Rows)
            {
                if (btnScan.Text == "Scan") return;

                var asset = r.Cells[0].Value.ToString();
                var exchs = r.Cells[1].Value.ToString();
                if (exchs == null || asset == null)
                {
                    Log.Warn("Scan", "sciped 1");
                    continue;
                }

                FullStat st = await FullStat.Calculate(exchs, asset);
                if (st == null || st.excSell == null || st.excBuy == null)
                {
                    Log.Warn(asset, "sciped 2");
                    continue;
                }

                r.Cells[2].Value = st.proc.ToString();

                await st.Save();

                ColorizeRow(r, st.proc);
                if (st.volSell > 0 && st.volBuy > 0)
                {
                    if (frmArbitrage.Visible)
                        Invoke(() => frmArbitrage.btnUpdate_Click(this, new EventArgs()));
                }

            }

            OnComplete?.Invoke();
        });
    }
    void ColorizeRow(DataGridViewRow r, decimal proc)
    {
        var Num = proc;
        int red, green, blue;
        if (Num < 50)
        {
            red = (int)(2 * 255 * Num / 100);
            green = 255;
            blue = 0;
        }
        else
        {
            red = 255;
            green = (int)((2 - 2 * Num / 100) * 255);
            blue = 0;
        }
        try
        {
            r.DefaultCellStyle.BackColor = Color.FromArgb(red, green, blue);
        }
        catch (Exception ex)
        {
            Log.Error("ColorizeRow", ex.Message);
        }
    }

    private void dgvProds_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        ShowAllStatistics();
    }

    private void btnScan_Click(object sender, EventArgs e)
    {
        if (btnScan.Text == "Scan")
        {
            btnScan.Text = "Stop scan";
            btnReload.Enabled = false;
            Telega.IsRunning = true;

            StartScan(() =>
            {
                Invoke(async () =>
                {
                    btnScan.Text = "Scan";
                    btnReload.Enabled = true;
                    txtSearch.Enabled = true;
                    Telega.IsRunning = false;
                    await Telega.SendMessageToAll("Сканирование завершено");
                    return;
                });
            });
        }
        else
        {
            btnScan.Text = "Scan";
            btnReload.Enabled = true;
            Telega.IsRunning = false;
        }
    }

    private void btnBot1_Click(object sender, EventArgs e)
    {
        // Укажите путь к исполняемому файлу
        string pathToExe = "D:\\Projects\\CryptoTrading\\Sinex\\bot1\\bin\\Debug\\net8.0-windows\\bot1.exe";
        if(sender == btnBot6)
            pathToExe = "D:\\Projects\\CryptoTrading\\Sinex\\bot6\\bin\\Debug\\net8.0-windows\\bot6.exe";

        // Создаем новый процесс
        Process process = new Process();

        try
        {
            // Устанавливаем свойства процесса
            process.StartInfo.FileName = pathToExe;

            // Запускаем процесс
            process.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при запуске приложения: " + ex.Message);
        }
        finally
        {
            // Важно закрыть процесс после использования
            process.Close();
            process.Dispose();
        }
    }
}
