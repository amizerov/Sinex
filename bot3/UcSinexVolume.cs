using DevExpress.XtraCharts;

namespace bot3;

public partial class UcSinexChart
{
    void OnCustomDrawSeries(object sender, CustomDrawSeriesEventArgs e)
    {
        var sv = _chart.Series.FirstOrDefault(s => s.Name == "Volume");
        if (e.Series.Equals(sv))
        {
            var ps = e.Series.Points;
            ps[0].Color = Color.Green;
            for (int i = 1; i < ps.Count; i++)
            {
                var p = ps[i];
                var pp = ps[i - 1];

                if (p.Values[0] > pp.Values[0])
                    p.Color = Color.Green;
                else
                    p.Color = Color.Red;
            }

        }
    }

    void AddVolume()
    {
        _chart.CustomDrawSeries += OnCustomDrawSeries;

        XYDiagramPane paneVol = new();
        paneVol.Name = "Volume";
        paneVol.LayoutOptions.RowSpan = 1;

        _diagram.Panes.Add(paneVol);

        Series seriVol = new Series("Volume", ViewType.Bar);

        seriVol.ArgumentDataMember = "OpenTime";
        seriVol.ValueDataMembers.AddRange("Volume");
        seriVol.CrosshairLabelPattern = "{A:dd-MM HH:mm} V:{V}";
        seriVol.ArgumentScaleType = ScaleType.DateTime;
        var arPoints = _klines.Select(x => new SeriesPoint(x.OpenTime, x.Volume)).ToArray();
        seriVol.Points.AddRange(arPoints);

        SecondaryAxisY volAxisY = new SecondaryAxisY();
        volAxisY.WholeRange.AlwaysShowZeroLevel = false;
        volAxisY.Alignment = AxisAlignment.Far;
        volAxisY.GridLines.Visible = true;
        _diagram.SecondaryAxesY.Add(volAxisY);

        volAxisY.SetVisibilityInPane(true, paneVol);
        volAxisY.SetVisibilityInPane(false, _diagram.DefaultPane);
        ((BarSeriesView)seriVol.View).AxisY = volAxisY;

        _chart.Series.Add(seriVol);

        ((XYDiagramSeriesViewBase)seriVol.View).Pane = paneVol;
        seriVol.View.Color = Color.Green;

    }
}
