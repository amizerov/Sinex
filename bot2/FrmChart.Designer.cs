namespace bot2
{
    partial class FrmChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            cbInterval = new ComboBox();
            label1 = new Label();
            lblSymbol = new Label();
            lblZoom = new Label();
            ((System.ComponentModel.ISupportInitialize)chart).BeginInit();
            SuspendLayout();
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart.Legends.Add(legend1);
            chart.Location = new Point(12, 74);
            chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart.Series.Add(series1);
            chart.Size = new Size(1339, 835);
            chart.TabIndex = 0;
            chart.Text = "chart1";
            // 
            // cbInterval
            // 
            cbInterval.FormattingEnabled = true;
            cbInterval.Location = new Point(125, 26);
            cbInterval.Name = "cbInterval";
            cbInterval.Size = new Size(182, 33);
            cbInterval.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 26);
            label1.Name = "label1";
            label1.Size = new Size(78, 25);
            label1.TabIndex = 2;
            label1.Text = "Intervals";
            // 
            // lblSymbol
            // 
            lblSymbol.AutoSize = true;
            lblSymbol.Location = new Point(388, 29);
            lblSymbol.Name = "lblSymbol";
            lblSymbol.Size = new Size(72, 25);
            lblSymbol.TabIndex = 3;
            lblSymbol.Text = "Symbol";
            // 
            // lblZoom
            // 
            lblZoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblZoom.AutoSize = true;
            lblZoom.Location = new Point(1265, 26);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(60, 25);
            lblZoom.TabIndex = 3;
            lblZoom.Text = "Zoom";
            // 
            // FrmChart
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1363, 921);
            Controls.Add(lblZoom);
            Controls.Add(lblSymbol);
            Controls.Add(label1);
            Controls.Add(cbInterval);
            Controls.Add(chart);
            Name = "FrmChart";
            Text = "Chart";
            Load += FrmChart_Load;
            ((System.ComponentModel.ISupportInitialize)chart).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private ComboBox cbInterval;
        private Label label1;
        private Label lblSymbol;
        private Label lblZoom;
    }
}