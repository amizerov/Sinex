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
            btnStakan = new Button();
            ((System.ComponentModel.ISupportInitialize)chart).BeginInit();
            SuspendLayout();
            // 
            // chart
            // 
            chart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea1.Name = "ChartArea1";
            chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chart.Legends.Add(legend1);
            chart.Location = new Point(12, 52);
            chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart.Series.Add(series1);
            chart.Size = new Size(1226, 726);
            chart.TabIndex = 0;
            chart.Text = "chart1";
            // 
            // cbInterval
            // 
            cbInterval.DropDownStyle = ComboBoxStyle.DropDownList;
            cbInterval.FormattingEnabled = true;
            cbInterval.Location = new Point(125, 8);
            cbInterval.Name = "cbInterval";
            cbInterval.Size = new Size(85, 33);
            cbInterval.TabIndex = 1;
            cbInterval.SelectedIndexChanged += cbInterval_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 11);
            label1.Name = "label1";
            label1.Size = new Size(70, 25);
            label1.TabIndex = 2;
            label1.Text = "Interval";
            // 
            // lblSymbol
            // 
            lblSymbol.AutoSize = true;
            lblSymbol.Location = new Point(388, 11);
            lblSymbol.Name = "lblSymbol";
            lblSymbol.Size = new Size(72, 25);
            lblSymbol.TabIndex = 3;
            lblSymbol.Text = "Symbol";
            // 
            // lblZoom
            // 
            lblZoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblZoom.AutoSize = true;
            lblZoom.Location = new Point(1152, 11);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(60, 25);
            lblZoom.TabIndex = 3;
            lblZoom.Text = "Zoom";
            // 
            // btnStakan
            // 
            btnStakan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStakan.Location = new Point(1118, 8);
            btnStakan.Name = "btnStakan";
            btnStakan.Size = new Size(112, 34);
            btnStakan.TabIndex = 4;
            btnStakan.Text = "Stakan";
            btnStakan.UseVisualStyleBackColor = true;
            btnStakan.Click += btnStakan_Click;
            // 
            // FrmChart
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1250, 790);
            Controls.Add(btnStakan);
            Controls.Add(lblZoom);
            Controls.Add(lblSymbol);
            Controls.Add(label1);
            Controls.Add(cbInterval);
            Controls.Add(chart);
            Name = "FrmChart";
            Text = "Chart";
            FormClosing += FrmChart_FormClosing;
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
        private Button btnStakan;
    }
}