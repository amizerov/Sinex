using System.Windows.Forms.DataVisualization.Charting;

namespace bot2
{
    public class ChartInit
    {
        Chart _ch;
        public ChartInit(Chart chart) 
        { 
            _ch = chart;
            _ch.SizeChanged += chart_SizeChanged;

            _ch.Series.Clear();
            _ch.Legends.Clear();
            _ch.BackColor = Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(223)))), ((int)(((byte)(240)))));
            _ch.BackGradientStyle = GradientStyle.TopBottom;
            _ch.BackSecondaryColor = Color.White;
            _ch.BorderlineColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            _ch.BorderlineDashStyle = ChartDashStyle.Solid;
            _ch.BorderlineWidth = 2;
            _ch.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            var cha = _ch.ChartAreas[0];
            cha.AxisY2.ScrollBar.Enabled = false;
            cha.AxisY2.Enabled = AxisEnabled.True;
            cha.AxisY2.IsStartedFromZero = _ch.ChartAreas[0].AxisY.IsStartedFromZero;
            cha.BackColor = Color.Gray;
            cha.BackGradientStyle = GradientStyle.LeftRight;
            cha.BackSecondaryColor = Color.White;
            cha.ShadowOffset = 5;
            cha.Position.Auto = false;
            cha.Position.Width = 92;
            cha.Position.Height = 90;
            cha.Position.X = 1.5F;
            cha.Position.Y = 7;

            var cx = cha.CursorX;
            cx.IsUserEnabled = true;
            cx.IsUserSelectionEnabled = true;
            cx.LineDashStyle = ChartDashStyle.Dash;

            var cy = cha.CursorY;
            cy.IsUserEnabled = true;
            cy.IsUserSelectionEnabled = true;
            cy.LineDashStyle = ChartDashStyle.Dash;
            cy.AxisType = AxisType.Secondary;

        }
        public void SetTitle(string ExchangeName, string Symbol)
        {
            var title = _ch.Titles[0];
            title.Alignment = ContentAlignment.TopLeft;
            title.BackColor = Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(120)))), ((int)(((byte)(160)))), ((int)(((byte)(240)))));
            title.BorderColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title.Font = new Font("Trebuchet MS", 14.25F, FontStyle.Bold);
            title.ForeColor = Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            title.Name = "Title1";
            title.Position.Auto = false;
            title.Position.Height = 3.5F;
            title.Position.Width = 50F;
            title.Position.X = 2F;
            title.Position.Y = 2F;
            title.ShadowColor = Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            title.Text = ExchangeName + ": " + Symbol;

            title.Tag = (double)title.Font.Size / (double)_ch.Height;

            title.TextStyle = TextStyle.Embed;
        }
        private void chart_SizeChanged(object? sender, EventArgs e)
        {
            var title = _ch.Titles[0];
            if (title.Tag == null || title.Tag.ToString() == "") return;

            float fonSize = (float)(0.013 * (double)_ch.Height);
            title.Font = new Font("Trebuchet MS", fonSize, FontStyle.Bold);
        }

    }
}
