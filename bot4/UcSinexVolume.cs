using DevExpress.Utils;
using DevExpress.XtraCharts;

namespace bot4;

public partial class UcSinexChart
{
    private void OnCustomDrawAxisLabel(object sender, CustomDrawAxisLabelEventArgs e)
    {
        if (_diagram == null) return;
        if (e.Item.Axis == _diagram.SecondaryAxesY["Volume"])
        {
            if (_klines.Count == 0) return;
            decimal? maxVol = _klines.MaxBy(k => k.Volume)!.Volume;
            string? txt = e.Item.AxisValue.ToString();
            if (txt == null) return;
            decimal vol = decimal.Parse(txt);
            if (vol > maxVol)
            {
                e.Item.Text = "";
            }
        }
    }

    void OnCustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
    {
        var sv = _seriVolu;
        if (e.Series.Equals(sv))
        {
            var ps = e.Series.Points;
            if (ps.Count == 0) return;

            ps[0].Color = Color.FromArgb(124, 149, 137);
            for (int i = 1; i < ps.Count; i++)
            {
                var p = ps[i];
                var pp = ps[i - 1];

                if (p.Values[0] > pp.Values[0])
                    p.Color = Color.FromArgb(124, 149, 137);
                else
                    p.Color = Color.FromArgb(225, 105, 105);
            }
        }
    }

    void ShowVolume(bool InOwnPane = false)
    {
        if(_chart == null || _diagram == null) return;

        _chart.CustomDrawAxisLabel += OnCustomDrawAxisLabel;
        _chart.CustomDrawSeries += OnCustomDrawSeries;

        if (InOwnPane)
        {
            XYDiagramPane paneVol = new();
            paneVol.Name = "Volume";
            paneVol.LayoutOptions.RowSpan = 1;

            _diagram.Panes.Add(paneVol);
            ((XYDiagramSeriesViewBase)_seriVolu.View).Pane = paneVol;
        }
        else
        {
            SecondaryAxisY volAxisY = new SecondaryAxisY();
            volAxisY.Name = "Volume";
            volAxisY.WholeRange.AlwaysShowZeroLevel = true;
            volAxisY.Alignment = AxisAlignment.Near;
            volAxisY.Visibility = DefaultBoolean.True;
            _diagram.SecondaryAxesY.Add(volAxisY);

            ((BarSeriesView)_seriVolu.View).AxisY = volAxisY;
        }
    }
}
