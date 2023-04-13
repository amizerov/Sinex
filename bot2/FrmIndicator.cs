using bot2.Tools;

namespace bot2
{
    public partial class FrmIndicator : Form
    {
        public string IndicatorsSma = "";
        public FrmIndicator()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (var sind in chLbIndSma.CheckedItems)
            {
                if (sind.ToString() == "SMA")
                {
                    if (chbMa1.Checked)
                        IndicatorsSma = "Sma;" + txtLookbackPeriods1.Text + "|";
                    if (chbMa2.Checked)
                        IndicatorsSma += "Sma;" + txtLookbackPeriods2.Text + "|";
                    if (chbMa3.Checked)
                        IndicatorsSma += "Sma;" + txtLookbackPeriods3.Text + "|";
                    if (chbMa4.Checked)
                        IndicatorsSma += "Sma;" + txtLookbackPeriods4.Text + "|";
                    if (chbMa5.Checked)
                        IndicatorsSma += "Sma;" + txtLookbackPeriods5.Text + "|";
                    if (chbMa6.Checked)
                        IndicatorsSma += "Sma;" + txtLookbackPeriods6.Text + "|";
                }
                if (IndicatorsSma.Length > 0) IndicatorsSma = IndicatorsSma.Trim('|');
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FrmIndicator_Load(object sender, EventArgs e)
        {
        }

        private void chbMa_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
