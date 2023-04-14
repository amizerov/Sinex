using bot2.Controls;
using System.Text.Json;

namespace bot2;

public class JIndica
{
    string Name = "";
    List<string> Settings = new();
}

public partial class FrmIndicator : Form
{
    public string IndicatorsString => JsonSerializer.Serialize(IndicatorsList);

    List<JIndica> IndicatorsList = new();
    UcIndBase? UcIndicator;

    public FrmIndicator(string indicator)
    {
        InitializeComponent();

        /**_indicators*******************
        {
           "Idicators":[
              {"SMA": ["12;2;-45698","27;2;-654433","99;3;-324466"]},
              {"SMMA":["12;2;-45698","27;2;-654433","99;3;-324466"]}
           ]
        }
        **********************************/

        IndicatorsList = JsonSerializer.Deserialize<List<JIndica>>(indicator)!;
    }

    private void FrmIndicator_Load(object sender, EventArgs e)
    {
        var itms = chLbIndSma.Items;
        for (int i = 0; i < itms.Count; i++)
        {
            if (IndicatorsList.Count > 0 
             && IndicatorsList.Contains(itms[i].ToString()!))
            {
                chLbIndSma.SetItemChecked(i, true);
            }
        }
    }

    // 12;1;2534|25;2;3546
    private void btnSave_Click(object sender, EventArgs e)
    {
        //IndicatorsSma = "";
        foreach (var sind in chLbIndSma.CheckedItems)
        {
            if (sind.ToString() == "SMA")
            {
            }
        }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void chbMa_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void btnColor_Click(object sender, EventArgs e)
    {
        if (colorDialog1.ShowDialog() == DialogResult.OK)
        {
            ((Button)sender).BackColor = colorDialog1.Color;
        }
    }

    private void chLbIndSma_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel1.Controls.Clear();
        if (chLbIndSma.SelectedItem?.ToString() == "SMA")
        {
            UcIndicator = new UcIndSMA();
            panel1.Controls.Add(UcIndicator);
        }
    }
}
