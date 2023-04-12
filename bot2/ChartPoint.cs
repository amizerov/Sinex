﻿using amLogger;
using System.Windows.Forms.DataVisualization.Charting;

namespace bot2;

public class ChartPoint
{
    Chart _ch;
    ChartArea _cha;

    public ChartPoint(Chart ch)
    {
        _ch = ch;
        _ch.PostPaint += chart_PostPaint;
        _cha = _ch.ChartAreas[0];

        _ch.MouseMove += chart_MouseMove;

    }
    void chart_PostPaint(object? sender, ChartPaintEventArgs e)
    {
        Series sKlines = _ch.Series["Klines"];
        if (sKlines.Points.Count == 0) return;

        DataPoint p = sKlines.Points.Last();

        Axis ay = _cha.AxisY2;
        Axis ax = _cha.AxisX;

        double vx = ax.Maximum;
        double vy = (double)p.YValues[3];

        int x = (int)ax.ValueToPixelPosition(vx);
        int y = (int)ay.ValueToPixelPosition(vy);

        Graphics g = e.ChartGraphics.Graphics;

        //Подложка под текщей ценой
        Pen pen = new Pen(Brushes.FloralWhite, 50);
        Rectangle rec = new Rectangle(x + 32, y - 2, 120, 5);
        g.DrawRectangle(pen, rec);
        //Текущая цена
        Font f = new Font(FontFamily.GenericSansSerif, 18);
        g.DrawString(vy + "", f, Brushes.DarkRed, x + 25, y - 20);
        //Стрелка
        Image mark = Image.FromFile("Content\\mark.png");
        g.DrawImage(mark, new Point(x - 1, y - 6));

        CursorPrice(g);
    }
    public void CursorLines(Point p)
    {
        try
        {
            double x = _cha.AxisX.PixelPositionToValue(p.X);
            double y = _cha.AxisY2.PixelPositionToValue(p.Y);

            _cha.CursorX.Position = x;
            _cha.CursorY.Position = y;

            CursorPrice();

        }
        catch (Exception ex) { Log.Error("cc", "Error: " + ex.Message); }
    }
    void CursorPrice(Graphics? g = null)
    {
        double yMinValue = _cha.AxisY2.ScaleView.ViewMinimum;
        double yMaxValue = _cha.AxisY2.ScaleView.ViewMaximum;
        double xMinValue = _cha.AxisX.Minimum;
        double xMaxValue = _cha.AxisX.Maximum;
        
        double xValue = _cha.CursorX.Position;
        double yValue = _cha.CursorY.Position;
        if(xValue.Equals(double.NaN) || yValue.Equals(double.NaN)) return;

        int xMaxPixels = (int)_cha.AxisX.ValueToPixelPosition(xMaxValue);
        int yPixel = (int)_cha.AxisY2.ValueToPixelPosition(yValue);
        int yMinPixel = (int)_cha.AxisY2.ValueToPixelPosition(yMinValue);
        int yMaxPixel = (int)_cha.AxisY2.ValueToPixelPosition(yMaxValue);

        if (yValue < yMinValue || yValue > yMaxValue) return;
        if (yPixel > 0 && xValue > xMinValue && xValue < xMaxValue)
        {
            if (g == null) g = _ch.CreateGraphics();
            //Подложка под ценой курсора
            Pen pen = new Pen(Brushes.FloralWhite, 30);
            Rectangle rec = new Rectangle(xMaxPixels + 22, yPixel - 5, 120, 5);
            g.DrawRectangle(pen, rec);
            //цена курсора
            Font f = new Font(FontFamily.GenericSansSerif, 10);
            string sPrice = yValue.ToString();
            if (sPrice.Length > 9) sPrice = sPrice.Substring(0, 9);
            g.DrawString(sPrice, f, Brushes.DarkOliveGreen, xMaxPixels + 15, yPixel - 15);
            //Стрелка
            //Image mark = Image.FromFile("Content\\markc.png");
            //g.DrawImage(mark, new Point(x - 1, y - 6));
        }
    }
    private void chart_MouseMove(object? sender, MouseEventArgs e)
    {
        CursorLines(e.Location);
    }

}
