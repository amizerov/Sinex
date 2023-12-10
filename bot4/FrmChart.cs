using CaExch;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace bot4;

public partial class FrmChart : XtraForm
{
    bool IsLoaded = false;
    CaExchanges Exchanges = new();
    public FrmChart(int exch, string symbol)
    {
        InitializeComponent();

        chart.Dock = DockStyle.Fill;
        chart.Exchange = Exchanges.FirstOrDefault(e => e.ID == exch)!;
        chart.Symbol = symbol;
    }

    private void FrmChart_Load(object sender, EventArgs e)
    {
        Tools.LoadFormPosition(this);

        chart.Interval = chart.Exchange.Intervals[1];

        Text = chart.Exchange.Name + " - " + chart.Symbol + " " + chart.Interval;
        barEditItem1.EditValue = chart.Interval;

        cbInterval.Items.Clear();
        cbInterval.Items.AddRange(chart.Exchange.Intervals);

        chart.UpdateData();
        IsLoaded = true;
    }

    private void FrmChart_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tools.SaveFormPosition(this);
    }

    private void barEditItem1_EditValueChanged(object sender, EventArgs e)
    {
        if (!IsLoaded) return;

        chart.Interval = barEditItem1.EditValue.ToString()!;
        Text = chart.Exchange.Name + " - " + chart.Symbol + " " + chart.Interval;

        chart.UpdateData();
    }

    private void btnUpdate_ItemClick(object sender, ItemClickEventArgs e)
    {
        chart.UpdateData();
    }

    private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
    {
        chart.ShowPaneVolu = barCheckItem1.Checked;
    }

    private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
    {
        chart.ShowPaneIndi = barCheckItem2.Checked;
    }

    private void btnZoomIn_ItemClick(object sender, ItemClickEventArgs e)
    {
        chart.Zoom -= 10;
        chart.UpdateData();

        txtZoom.Caption = chart.Zoom + "";
    }

    private void btnZoomOut_ItemClick(object sender, ItemClickEventArgs e)
    {
        chart.Zoom += 10;
        chart.UpdateData();
        
        txtZoom.Caption = chart.Zoom + "";
    }

    private void btnZoom_ItemClick(object sender, ItemClickEventArgs e)
    {
        chart.Zoom = int.Parse(((BarButtonItem)e.Item).Caption);
        chart.UpdateData();
        
        txtZoom.Caption = chart.Zoom + "";
    }
}