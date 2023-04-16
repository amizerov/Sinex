using bot2.Controls;

namespace bot2;

public partial class FrmIndica : Form
{
    public List<JIndica> IndicatorsList = new();
    public FrmIndica(List<JIndica> indicatorsList)
    {
        InitializeComponent();
        IndicatorsList = indicatorsList;
    }

    private void FrmIndica_Load(object sender, EventArgs e)
    {
        if (IndicatorsList == null || IndicatorsList.Count == 0) return;
        chLbIndSma.Items.Clear();
        foreach (JIndica indicator in IndicatorsList)
        {
            int i = chLbIndSma.Items.Add(indicator.Name);
            chLbIndSma.SetItemChecked(i, indicator.IsChecked);
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        foreach (JIndica indicator in IndicatorsList) indicator.IsChecked = false;
        foreach(var ind in chLbIndSma.CheckedItems)
        {
            var i = IndicatorsList.FirstOrDefault(i => i.Name == ind.ToString());
            if(i != null) i.IsChecked = true;
        }

        if (panel1.Controls.Count == 0) return;
        UcIndBase c = (UcIndBase)panel1.Controls[0];

        JIndica jIndica = new JIndica();
        jIndica.Name = c.Name;
        jIndica.Settings = c.GetIndicators();

        var curInd = IndicatorsList.FirstOrDefault(i => i.Name == jIndica.Name);
        if (curInd != null)
        {
            curInd.Settings = jIndica.Settings;
        }
        else
        {
            IndicatorsList.Add(jIndica);
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void chLbIndSma_SelectedIndexChanged(object sender, EventArgs e)
    {
        string itm = chLbIndSma.SelectedItem?.ToString()!;
        JIndica inds = IndicatorsList.FirstOrDefault(ind => ind.Name == itm)!;
        if (inds == null) inds = new();

        if (panel1.Controls.Count == 1)
        {
            UcIndBase cuc = (UcIndBase)panel1.Controls[0];
            if (cuc.Name == itm) return;
        }

        UcIndBase uc = new UcIndMABase(inds);
        uc.Name = itm;

        panel1.Controls.Clear();
        panel1.Controls.Add(uc);
    }
}
