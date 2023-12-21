using amLogger;
using CaDb;
using CaExch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Buffers;
using System.Windows.Forms;

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
        if (!_loaded) return; _loaded = false;

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

        await st.Update();

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
        dgvProds.FirstDisplayedScrollingRowIndex = rowIndex > 2 ? rowIndex - 1 : rowIndex;
    }

    private void btnArbit_Click(object sender, EventArgs e)
    {
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

                FullStat st = await FullStat.Init(exchs, asset);
                if (st == null || st.exc1 == null || st.exc2 == null)
                {
                    Log.Warn(asset, "sciped 2");
                    continue; 
                }

                r.Cells[2].Value = st.proc.ToString();

                await st.Save();

                if (st.vol1 > 0 && st.vol2 > 0)
                {
                    if(frmArbitrage.Visible)
                        Invoke(() => frmArbitrage.btnUpdate_Click(this, new EventArgs()));
                }

                var Num = st.proc;
                int red, green, blue;
                if (Num < (decimal)1.5)
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
                r.DefaultCellStyle.BackColor = Color.FromArgb(red, green, blue);
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
            //txtSearch.Enabled = false;

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
            //txtSearch.Enabled = true;
        }

    }
}
