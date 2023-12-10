namespace bot1
{
    partial class Form1
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
            btnDbCheck = new Button();
            textBox1 = new TextBox();
            btnExn = new Button();
            btnStart = new Button();
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            gcLog = new DevExpress.XtraGrid.GridControl();
            gvLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            statusStrip1 = new StatusStrip();
            status = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gcLog).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gvLog).BeginInit();
            xtraTabPage2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // btnDbCheck
            // 
            btnDbCheck.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDbCheck.Location = new Point(491, 7);
            btnDbCheck.Margin = new Padding(2);
            btnDbCheck.Name = "btnDbCheck";
            btnDbCheck.Size = new Size(130, 30);
            btnDbCheck.TabIndex = 0;
            btnDbCheck.Text = "Проверка базы";
            btnDbCheck.UseVisualStyleBackColor = true;
            btnDbCheck.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(0, 0);
            textBox1.Margin = new Padding(2);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(612, 326);
            textBox1.TabIndex = 1;
            // 
            // btnExn
            // 
            btnExn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExn.Location = new Point(354, 7);
            btnExn.Margin = new Padding(2);
            btnExn.Name = "btnExn";
            btnExn.Size = new Size(132, 30);
            btnExn.TabIndex = 2;
            btnExn.Text = "Количество бирж";
            btnExn.UseVisualStyleBackColor = true;
            btnExn.Click += button2_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(27, 8);
            btnStart.Margin = new Padding(2);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(310, 30);
            btnStart.TabIndex = 3;
            btnStart.Text = "Начать обновление продуктов всех бирж";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += button3_Click;
            // 
            // xtraTabControl1
            // 
            xtraTabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            xtraTabControl1.Location = new Point(7, 50);
            xtraTabControl1.Margin = new Padding(2);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            xtraTabControl1.Size = new Size(614, 351);
            xtraTabControl1.TabIndex = 4;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { xtraTabPage1, xtraTabPage2 });
            // 
            // xtraTabPage1
            // 
            xtraTabPage1.Controls.Add(gcLog);
            xtraTabPage1.Margin = new Padding(2);
            xtraTabPage1.Name = "xtraTabPage1";
            xtraTabPage1.Size = new Size(612, 326);
            xtraTabPage1.TabPageWidth = 200;
            xtraTabPage1.Text = "Table";
            // 
            // gcLog
            // 
            gcLog.Dock = DockStyle.Fill;
            gcLog.EmbeddedNavigator.Margin = new Padding(2);
            gcLog.Location = new Point(0, 0);
            gcLog.MainView = gvLog;
            gcLog.Margin = new Padding(2);
            gcLog.Name = "gcLog";
            gcLog.Size = new Size(612, 326);
            gcLog.TabIndex = 0;
            gcLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gvLog });
            // 
            // gvLog
            // 
            gvLog.ColumnPanelRowHeight = 0;
            gvLog.DetailHeight = 175;
            gvLog.FooterPanelHeight = 0;
            gvLog.GridControl = gcLog;
            gvLog.GroupRowHeight = 0;
            gvLog.Name = "gvLog";
            gvLog.OptionsBehavior.Editable = false;
            gvLog.OptionsView.ShowFooter = true;
            gvLog.OptionsView.ShowGroupPanel = false;
            gvLog.RowHeight = 0;
            gvLog.ViewCaptionHeight = 0;
            // 
            // xtraTabPage2
            // 
            xtraTabPage2.Controls.Add(textBox1);
            xtraTabPage2.Margin = new Padding(2);
            xtraTabPage2.Name = "xtraTabPage2";
            xtraTabPage2.Size = new Size(612, 326);
            xtraTabPage2.TabPageWidth = 200;
            xtraTabPage2.Text = "Text";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { status });
            statusStrip1.Location = new Point(0, 410);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(632, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            status.Name = "status";
            status.Size = new Size(118, 17);
            status.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(632, 432);
            Controls.Add(statusStrip1);
            Controls.Add(xtraTabControl1);
            Controls.Add(btnStart);
            Controls.Add(btnExn);
            Controls.Add(btnDbCheck);
            Margin = new Padding(2);
            MinimumSize = new Size(648, 39);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gcLog).EndInit();
            ((System.ComponentModel.ISupportInitialize)gvLog).EndInit();
            xtraTabPage2.ResumeLayout(false);
            xtraTabPage2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDbCheck;
        private TextBox textBox1;
        private Button btnExn;
        private Button btnStart;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gcLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLog;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel status;
    }
}