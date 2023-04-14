namespace bot2.Controls;

public partial class UcIndSMA : UcIndBase
{

    CheckBox[] _chb;
    TextBox[] _txt;
    Button[] _btn;
    NumericUpDown[] _nud;

    public UcIndSMA(string indica = "")
    {
        InitializeComponent();

        _indica = indica;

        _chb = new CheckBox[6] { chbMa1, chbMa2, chbMa3, chbMa4, chbMa5, chbMa6 };
        _txt = new TextBox[6] { txtLookbackPeriods1, txtLookbackPeriods2, txtLookbackPeriods3, txtLookbackPeriods4, txtLookbackPeriods5, txtLookbackPeriods6 };
        _btn = new Button[6] { btnColor1, btnColor2, btnColor3, btnColor4, btnColor5, btnColor6 };
        _nud = new NumericUpDown[6] { nudLine1, nudLine2, nudLine3, nudLine4, nudLine5, nudLine6 };
    }

    private void UcIndSMA_Load(object sender, EventArgs e)
    {
        if (_indica == "") return;

        string[] ar = _indica.Split('|');
        int j = 0;
        foreach (string s in ar)
        {
            string[] a = s.Split(';');
            _chb[j].Checked = true;
            _txt[j].Text = a[0];
            _nud[j].Value = int.Parse(a[1]);
            _btn[j].BackColor = Color.FromArgb(int.Parse(a[2]));
            j++;
        }
    }

    public override string GetIndicatorsString()
    {
        _indica = "";
        for (int i = 0; i < 6; i++)
        {
            if (_chb[i].Checked)
                _indica +=
                    _txt[i].Text + ";" + _nud[i].Value + ";" + _btn[i].BackColor.ToArgb() + "|";
        }
        if (_indica.Length > 0) _indica = _indica.Trim('|');

        return _indica;
    }
}
