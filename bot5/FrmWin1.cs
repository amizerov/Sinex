using amLogger;
using CaDb;
using CaExch;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using System.Windows.Forms;

namespace bot5;

public partial class FrmWin1 : Form
{
    FrmWin2 frm2 = new();

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

        dgvProds.DataSource = Data.GetProdsWithExchanges(txtSearch.Text);

        dgvProds.Columns[3].Visible = false;
        dgvProds.Columns[4].Visible = false;
        dgvProds.Columns[5].Visible = false;

        dgvProds.Columns[0].Width = 60;
        dgvProds.Columns[1].Width = 80;
        dgvProds.Columns[2].Width = 50;

        _loaded = true;
    }

    private void FrmWin1_Load(object sender, EventArgs e)
    {
        LoadProducts();
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
        LoadProducts();
    }

    async void ShowAllStatistics()
    {
        if (!_loaded) return;
        _loaded = false;

        panel.Controls.Clear();
        lblExc1.Text = lblExc2.Text = lblMaxProc.Text = "";
        if (dgvProds.Rows.Count == 0) return;

        var ass = dgvProds.CurrentRow.Cells[0].Value.ToString();
        var ens = dgvProds.CurrentRow.Cells[1].Value.ToString();
        if (ens == null || ass == null) return;

        lblSym.Text = ass;
        FullStat st = await FullStat.Init(ens, ass, s => AddLabel(s));
        dgvProds.CurrentRow.Cells[2].Value = st.proc.ToString();

        if (st == null || st.exc1 == null || st.exc2 == null)
        {
            Log.Warn(ass, "sciped");
            return;
        }

        await st.Save();

        lblExc1.Text = st.exc1.Name;
        lblExc2.Text = st.exc2.Name;
        lblMaxProc.Text = st.proc + "%";
    
        _loaded = true;
    }

    void AddLabel(PriceSt s)
    {
        int c = panel.Controls.Count + 3;
        Label lblEx = new();
        lblEx.Text = s.exchange!.Name;
        lblEx.Width = 60; lblEx.Top = 30 * c / 3; lblEx.Left = 10;
        Label lblPr = new();
        lblPr.Text = s.price.ToString();
        lblPr.Width = 100; lblPr.Top = lblEx.Top; lblPr.Left = 80;
        Label lblVo = new();
        lblVo.Text = s.volum.ToString();
        lblVo.Width = 200; lblVo.Top = lblEx.Top; lblVo.Left = 200;

        panel.Controls.Add(lblEx);
        panel.Controls.Add(lblPr);
        panel.Controls.Add(lblVo);
    }

    private void txtSearch_TextChanged(object sender, EventArgs e)
    {
        int rowIndex = -1;
        var rs = dgvProds.Rows
            .Cast<DataGridViewRow>()
                .Where(r => {
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
    }

    private void btnArbit_Click(object sender, EventArgs e)
    {
        frm2.Show();
        frm2.Top = this.Top;
        frm2.Left = this.Left + this.Width + 11;
    }

    async void StartScan(Action? OnComplete = null)
    {
        await Task.Run(async () =>
        {
            foreach (DataGridViewRow r in dgvProds.Rows)
            {
                if (btnScan.Text == "Scan") return;

                var ass = r.Cells[0].Value.ToString();
                var ens = r.Cells[1].Value.ToString();
                if (ens == null || ass == null) continue;

                FullStat st = await FullStat.Init(ens, ass);
                if (st == null || st.exc1 == null || st.exc2 == null)
                {
                    Log.Warn(ass, "sciped");
                    return; 
                }

                r.Cells[2].Value = st.proc.ToString();

                await st.Save();

                if (st.vol1 > 0 && st.vol2 > 0)
                {
                    Invoke(() => frm2.btnUpdate_Click(this, new EventArgs()));
                }

                r.DefaultCellStyle.BackColor = Color.LightGreen;
            }

            OnComplete?.Invoke();
        });
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
            txtSearch.Enabled = false;

            StartScan(() => {
                Invoke(() =>
                {
                    btnScan.Text = "Scan";
                    btnReload.Enabled = true;
                    txtSearch.Enabled = true;
                });
            });
        }
        else
        {
            btnScan.Text = "Scan";
            btnReload.Enabled = true;
            txtSearch.Enabled = true;
        }

    }
}
