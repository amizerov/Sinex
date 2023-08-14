using DevExpress.Utils;
using DevExpress.XtraCharts;

namespace bot3;

public partial class UcSinexChart
{
    void Indicators()
    {
        // Add indicators.
        SimpleMovingAverage sma = new SimpleMovingAverage
        {
            ValueLevel = ValueLevel.Close,
            LegendText = "Simple Moving Average",
            ShowInLegend = true,
            CrosshairEnabled = DefaultBoolean.False
        };
        _view.Indicators.Add(sma);
    }

    void Indicators1()
    {
        RateOfChange rateOfChange = new RateOfChange
        {
            ValueLevel = ValueLevel.Close,
            CrosshairEnabled = DefaultBoolean.True,
            CrosshairLabelPattern = "{V:f3}",
            LegendText = "Rate of Change",
            ShowInLegend = true
        };
        _view.Indicators.Add(rateOfChange);

        // Define the default pane options.
        _diagram.DefaultPane.LayoutOptions.RowSpan = 2;
        _diagram.DefaultPane.ScrollBarOptions.XAxisScrollBarVisible = false;

        // Add a separate pane and configure its layout options.
        XYDiagramPane rocPane = new XYDiagramPane();
        rocPane.LayoutOptions.RowSpan = 1;
        _diagram.Panes.Add(rocPane);

        // Assign the pane to the indicator.
        rateOfChange.Pane = rocPane;

        // Add a secondary y-axis and configure its options.
        SecondaryAxisY rocAxisY = new SecondaryAxisY();
        rocAxisY.WholeRange.AlwaysShowZeroLevel = false;
        rocAxisY.Alignment = AxisAlignment.Far;
        rocAxisY.GridLines.Visible = true;
        _diagram.SecondaryAxesY.Add(rocAxisY);

        // Assign the axis to the indicator.
        rateOfChange.AxisY = rocAxisY;

        // Define the primary axis options.
        _diagram.DependentAxesYRange = DefaultBoolean.True;

        // Add a separate legend for an indicator.
        Legend rocLegend = new Legend { AlignmentHorizontal = LegendAlignmentHorizontal.Left };
        rocLegend.DockTarget = rocPane;
        rocLegend.AlignmentVertical = LegendAlignmentVertical.Top;
        _chart.Legends.Add(rocLegend);
        rateOfChange.Legend = rocLegend;
    }

}
