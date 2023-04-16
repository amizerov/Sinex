using bot2.Controls;
using Skender.Stock.Indicators;

namespace bot2;

public partial class FrmIndicator : Form
{
    public List<JIndica> IndicatorsList = new();

    public FrmIndicator(List<JIndica> indicatorsList)
    {
        InitializeComponent();

        /**_indicators*******************
        [
	        {
		        "Name": "SMA",
		        "Settings": ["12;2;-45698","27;2;-654433","99;3;-324466"]
	        },
	        {
		        "Name": "SMMA",
		        "Settings": ["12;2;-45698","27;2;-654433","99;3;-324466"]
	        }
        ]
        **********************************/

        IndicatorsList = indicatorsList;
    }

    private void FrmIndicator_Load(object sender, EventArgs e)
    {
        var itms = chLbIndSma.Items;
        for (int i = 0; i < itms.Count; i++)
        {
            if (IndicatorsList.Count > 0 
             && IndicatorsList.Exists(ind => ind.Name == itms[i].ToString())
             )
            {
                chLbIndSma.SetItemChecked(i, true);
            }
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
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

    private void chbMa_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void chLbIndSma_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel1.Controls.Clear();
        string itm = chLbIndSma.SelectedItem?.ToString()!;
        JIndica inds = IndicatorsList.FirstOrDefault(ind => ind.Name == itm)!;
        if (inds == null) inds = new();

        UcIndBase uc = new();
        if (itm == "SMA")
        {
            uc = new UcIndSMA(inds.Settings);
        }
        if (itm == "SMMA")
        {
            uc = new UcIndSMMA(inds.Settings);
        }
        if (itm == "EMA")
        {
            uc = new UcIndEMA(inds.Settings);
        }
        uc.Name = itm;
        panel1.Controls.Add(uc);
    }
}

public class JIndica
{
    public string Name { get; set; }
    public List<string> Settings { get; set; }

    public JIndica()
    {
        Name = "";
        Settings = new List<string>();
    }
}

