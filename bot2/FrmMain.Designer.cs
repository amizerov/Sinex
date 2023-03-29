namespace bot2
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            button1 = new Button();
            chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            btnZoomOut = new Button();
            btnZoomIn = new Button();
            lbProducts = new ListBox();
            splitContainer1 = new SplitContainer();
            txtSeach = new TextBox();
            cbInterval = new ComboBox();
            label1 = new Label();
            lblZoom = new Label();
            cbExchange = new ComboBox();
            label2 = new Label();
            lblSymbol = new Label();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            mnuExit = new ToolStripMenuItem();
            indicatorsToolStripMenuItem = new ToolStripMenuItem();
            mnuSma = new ToolStripMenuItem();
            mnuRsi = new ToolStripMenuItem();
            mnuVfi = new ToolStripMenuItem();
            windowsToolStripMenuItem = new ToolStripMenuItem();
            mnuLogger = new ToolStripMenuItem();
            mnuOrderBook = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)chart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(1623, 58);
            button1.Margin = new Padding(4, 5, 4, 5);
            button1.Name = "button1";
            button1.Size = new Size(50, 50);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // chart
            // 
            chartArea2.Name = "ChartArea1";
            chart.ChartAreas.Add(chartArea2);
            chart.Dock = DockStyle.Fill;
            legend2.Name = "Legend1";
            chart.Legends.Add(legend2);
            chart.Location = new Point(0, 0);
            chart.Name = "chart";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series2.CustomProperties = "PriceUpColor=Green, PriceDownColor=Red";
            series2.Legend = "Legend1";
            series2.Name = "Klines";
            series2.YValuesPerPoint = 4;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            chart.Series.Add(series2);
            chart.Size = new Size(1450, 921);
            chart.TabIndex = 1;
            chart.Text = "chart";
            // 
            // btnZoomOut
            // 
            btnZoomOut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomOut.Location = new Point(1407, 60);
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.Size = new Size(50, 50);
            btnZoomOut.TabIndex = 2;
            btnZoomOut.Text = "+";
            btnZoomOut.UseVisualStyleBackColor = true;
            btnZoomOut.Click += btnZoomOut_Click;
            // 
            // btnZoomIn
            // 
            btnZoomIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZoomIn.Location = new Point(1560, 58);
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.Size = new Size(50, 50);
            btnZoomIn.TabIndex = 2;
            btnZoomIn.Text = "-";
            btnZoomIn.UseVisualStyleBackColor = true;
            btnZoomIn.Click += btnZoomIn_Click;
            // 
            // lbProducts
            // 
            lbProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbProducts.FormattingEnabled = true;
            lbProducts.ItemHeight = 25;
            lbProducts.Location = new Point(6, 46);
            lbProducts.Name = "lbProducts";
            lbProducts.Size = new Size(198, 854);
            lbProducts.TabIndex = 3;
            lbProducts.SelectedIndexChanged += lbProducts_SelectedIndexChanged;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(12, 148);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(txtSeach);
            splitContainer1.Panel1.Controls.Add(lbProducts);
            splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(chart);
            splitContainer1.Size = new Size(1663, 921);
            splitContainer1.SplitterDistance = 209;
            splitContainer1.TabIndex = 4;
            // 
            // txtSeach
            // 
            txtSeach.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSeach.Location = new Point(8, 8);
            txtSeach.Name = "txtSeach";
            txtSeach.Size = new Size(195, 31);
            txtSeach.TabIndex = 4;
            txtSeach.TextChanged += txtSeach_TextChanged;
            // 
            // cbInterval
            // 
            cbInterval.DropDownStyle = ComboBoxStyle.DropDownList;
            cbInterval.FormattingEnabled = true;
            cbInterval.Items.AddRange(new object[] { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d" });
            cbInterval.Location = new Point(394, 72);
            cbInterval.Name = "cbInterval";
            cbInterval.Size = new Size(71, 33);
            cbInterval.TabIndex = 5;
            cbInterval.SelectedIndexChanged += cbInterval_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(318, 72);
            label1.Name = "label1";
            label1.Size = new Size(70, 25);
            label1.TabIndex = 6;
            label1.Text = "Interval";
            // 
            // lblZoom
            // 
            lblZoom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblZoom.AutoSize = true;
            lblZoom.Location = new Point(1465, 75);
            lblZoom.Name = "lblZoom";
            lblZoom.Size = new Size(89, 25);
            lblZoom.TabIndex = 7;
            lblZoom.Text = "Zoom: 50";
            // 
            // cbExchange
            // 
            cbExchange.DropDownStyle = ComboBoxStyle.DropDownList;
            cbExchange.FormattingEnabled = true;
            cbExchange.Items.AddRange(new object[] { "Binance", "Kucoin", "Huobi" });
            cbExchange.Location = new Point(103, 72);
            cbExchange.Name = "cbExchange";
            cbExchange.Size = new Size(182, 33);
            cbExchange.TabIndex = 8;
            cbExchange.SelectedIndexChanged += cbExchange_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 75);
            label2.Name = "label2";
            label2.Size = new Size(86, 25);
            label2.TabIndex = 6;
            label2.Text = "Exchange";
            // 
            // lblSymbol
            // 
            lblSymbol.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lblSymbol.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold, GraphicsUnit.Point);
            lblSymbol.Location = new Point(471, 72);
            lblSymbol.Name = "lblSymbol";
            lblSymbol.Size = new Size(909, 38);
            lblSymbol.TabIndex = 9;
            lblSymbol.Text = "label3";
            lblSymbol.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(24, 24);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, indicatorsToolStripMenuItem, windowsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1687, 33);
            menuStrip1.TabIndex = 10;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mnuExit });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(54, 29);
            fileToolStripMenuItem.Text = "File";
            // 
            // mnuExit
            // 
            mnuExit.Name = "mnuExit";
            mnuExit.Size = new Size(141, 34);
            mnuExit.Text = "Exit";
            // 
            // indicatorsToolStripMenuItem
            // 
            indicatorsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mnuSma, mnuRsi, mnuVfi });
            indicatorsToolStripMenuItem.Name = "indicatorsToolStripMenuItem";
            indicatorsToolStripMenuItem.Size = new Size(106, 29);
            indicatorsToolStripMenuItem.Text = "Indicators";
            // 
            // mnuSma
            // 
            mnuSma.Name = "mnuSma";
            mnuSma.Size = new Size(270, 34);
            mnuSma.Text = "SMA";
            mnuSma.Click += mnuSma_Click;
            // 
            // mnuRsi
            // 
            mnuRsi.Name = "mnuRsi";
            mnuRsi.Size = new Size(270, 34);
            mnuRsi.Text = "RSI";
            mnuRsi.Click += mnuRsi_Click;
            // 
            // mnuVfi
            // 
            mnuVfi.Name = "mnuVfi";
            mnuVfi.Size = new Size(270, 34);
            mnuVfi.Text = "MFI";
            mnuVfi.Click += mnuVfi_Click;
            // 
            // windowsToolStripMenuItem
            // 
            windowsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mnuLogger, mnuOrderBook });
            windowsToolStripMenuItem.Name = "windowsToolStripMenuItem";
            windowsToolStripMenuItem.Size = new Size(102, 29);
            windowsToolStripMenuItem.Text = "Windows";
            // 
            // mnuLogger
            // 
            mnuLogger.Name = "mnuLogger";
            mnuLogger.Size = new Size(207, 34);
            mnuLogger.Text = "Logger";
            mnuLogger.Click += mnuLogger_Click;
            // 
            // mnuOrderBook
            // 
            mnuOrderBook.Name = "mnuOrderBook";
            mnuOrderBook.Size = new Size(207, 34);
            mnuOrderBook.Text = "Order book";
            mnuOrderBook.Click += mnuOrderBook_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1687, 1081);
            Controls.Add(lblSymbol);
            Controls.Add(cbExchange);
            Controls.Add(lblZoom);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbInterval);
            Controls.Add(splitContainer1);
            Controls.Add(btnZoomIn);
            Controls.Add(btnZoomOut);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 5, 4, 5);
            Name = "FrmMain";
            Text = "Sinex - Algo Tading Crypto bot v.1";
            FormClosing += FrmMain_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)chart).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private Button btnZoomOut;
        private Button btnZoomIn;
        private ListBox lbProducts;
        private SplitContainer splitContainer1;
        private ComboBox cbInterval;
        private Label label1;
        private Label lblZoom;
        private ComboBox cbExchange;
        private Label label2;
        private Label lblSymbol;
        private TextBox txtSeach;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem mnuExit;
        private ToolStripMenuItem indicatorsToolStripMenuItem;
        private ToolStripMenuItem mnuSma;
        private ToolStripMenuItem mnuRsi;
        private ToolStripMenuItem mnuVfi;
        private ToolStripMenuItem windowsToolStripMenuItem;
        private ToolStripMenuItem mnuLogger;
        private ToolStripMenuItem mnuOrderBook;
    }
}