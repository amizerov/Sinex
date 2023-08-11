using DevExpress.Charts.Native;
using DevExpress.XtraCharts;

namespace bot3;

public partial class UcSinexChart
{
    private ChartControl _chart = new();
    CandleStickSeriesView _view = new();
    XYDiagram _diagram = new();

    void MainChart()
    {
        // Create a series and bind it to data.
        Series series = new Series(_symbol + " " + _interval, ViewType.CandleStick);
        series.SetFinancialDataMembers("OpenTime", "LowPrice", "HighPrice", "OpenPrice", "ClosePrice");
        series.CrosshairLabelPattern = "{A:dd-MM HH:mm} O:{OV}, H:{HV}, L:{LV}, C:{CV}";
        series.ArgumentScaleType = ScaleType.DateTime;
        _chart.Series.Add(series);

        _view = (CandleStickSeriesView)series.View;
        /////////////
        // Enable scrolling and zooming for the primary x-axis.
        _diagram = (XYDiagram)_chart.Diagram;
        _diagram.EnableAxisXZooming = true;
        _diagram.EnableAxisXScrolling = true;

        //_diagram.DefaultPane.LayoutOptions.RowSpan = 3;
        // Customize the series view settings.

        _view.LineThickness = 2;
        _view.LevelLineLength = 0.5;

        // Specify the series reduction options.
        _view.ReductionOptions.ColorMode = ReductionColorMode.OpenToCloseValue;
        _view.ReductionOptions.FillMode = CandleStickFillMode.AlwaysFilled;
        _view.ReductionOptions.Level = StockLevel.Close;
        _view.ReductionOptions.Visible = true;

        // Set point colors.
        _view.Color = Color.Green;
        _view.ReductionOptions.Color = Color.Red;

        var y = _diagram.AxisY;
        y.WholeRange.AlwaysShowZeroLevel = false;
        y.Alignment = AxisAlignment.Far;
        y.GridLines.Visible = false;
        y.GridLines.MinorVisible = false;
        y.Interlaced = true;

        // Configure the crosshair options.
        _chart.CrosshairOptions.ShowOnlyInFocusedPane = false;
        _chart.CrosshairOptions.ShowValueLine = true;
        _chart.CrosshairOptions.ContentShowMode = CrosshairContentShowMode.Legend;
        _chart.CrosshairOptions.ShowArgumentLabels = true;
        _chart.CrosshairOptions.ShowValueLabels = true;
        _chart.CrosshairOptions.ShowOutOfRangePoints = true;

        // Specify the default legend's options.
        _chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
        _chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
        _chart.Legend.DockTarget = _diagram.DefaultPane;
        _chart.Legend.MaxCrosshairContentWidth = 600;
    }

    void SetupAxisX()
    {
        var x = _diagram.AxisX;
        //_diagram.AxisX.VisualRange.SetMinMaxValues(DateTime.Now.AddMinutes(-100), DateTime.Now);
        x.WholeRange.SideMarginsValue = 0;
        //x.SetVisibilityInPane(false, _diagram.DefaultPane);
        x.Label.ResolveOverlappingOptions.AllowStagger = true;
        x.Label.ResolveOverlappingOptions.AllowRotate = true;
        x.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
        x.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
        x.DateTimeScaleOptions.GridSpacing = 2;
        x.DateTimeScaleOptions.ScaleMode = ScaleMode.Automatic;
        x.DateTimeScaleOptions.AggregateFunction = AggregateFunction.Financial;
        x.DateTimeScaleOptions.GridOffset = 0;
        //x.GridLines.Visible = true;
        //x.Interlaced = true;
        x.Label.TextPattern = "{A:dd-MM HH:mm}";
    }
}
