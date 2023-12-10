namespace bot4
{
    partial class FrmChart
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
            components = new System.ComponentModel.Container();
            CaExch.CaBinance caBinance1 = new CaExch.CaBinance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            chart = new UcSinexChart();
            barManager1 = new DevExpress.XtraBars.BarManager(components);
            bar1 = new DevExpress.XtraBars.Bar();
            barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            cbInterval = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            barCheckItem1 = new DevExpress.XtraBars.BarCheckItem();
            barCheckItem2 = new DevExpress.XtraBars.BarCheckItem();
            barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            btnUpdate = new DevExpress.XtraBars.BarButtonItem();
            bar2 = new DevExpress.XtraBars.Bar();
            bar3 = new DevExpress.XtraBars.Bar();
            btnZoom = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
            barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            btnZoomIn = new DevExpress.XtraBars.BarButtonItem();
            txtZoom = new DevExpress.XtraBars.BarStaticItem();
            btnZoomOut = new DevExpress.XtraBars.BarButtonItem();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbInterval).BeginInit();
            SuspendLayout();
            // 
            // chart
            // 
            chart.Exchange = caBinance1;
            chart.Interval = "1m";
            chart.Location = new Point(782, 297);
            chart.Margin = new Padding(2);
            chart.Name = "chart";
            chart.Size = new Size(883, 541);
            chart.Symbol = "BTCUSDT";
            chart.TabIndex = 0;
            chart.Zoom = 100;
            // 
            // barManager1
            // 
            barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] { bar1, bar2, bar3 });
            barManager1.DockControls.Add(barDockControlTop);
            barManager1.DockControls.Add(barDockControlBottom);
            barManager1.DockControls.Add(barDockControlLeft);
            barManager1.DockControls.Add(barDockControlRight);
            barManager1.Form = this;
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barStaticItem2, barEditItem1, barCheckItem1, barButtonItem1, barButtonItem2, barButtonItem3, btnZoomIn, btnUpdate, barCheckItem2, btnZoomOut, barButtonItem4, barButtonItem5, barButtonItem6, barButtonItem7, barButtonItem8, barButtonItem9, barButtonItem10, barButtonItem11, barButtonItem12, btnZoom, barButtonItem14, txtZoom });
            barManager1.MainMenu = bar2;
            barManager1.MaxItemId = 33;
            barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { cbInterval });
            barManager1.StatusBar = bar3;
            // 
            // bar1
            // 
            bar1.BarName = "Tools";
            bar1.DockCol = 0;
            bar1.DockRow = 1;
            bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(barStaticItem2), new DevExpress.XtraBars.LinkPersistInfo(barEditItem1), new DevExpress.XtraBars.LinkPersistInfo(barCheckItem1), new DevExpress.XtraBars.LinkPersistInfo(barCheckItem2), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem2), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem1), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem3), new DevExpress.XtraBars.LinkPersistInfo(btnUpdate) });
            bar1.OptionsBar.UseWholeRow = true;
            bar1.Text = "Tools";
            // 
            // barStaticItem2
            // 
            barStaticItem2.Caption = "Interval";
            barStaticItem2.Id = 7;
            barStaticItem2.Name = "barStaticItem2";
            // 
            // barEditItem1
            // 
            barEditItem1.Edit = cbInterval;
            barEditItem1.Id = 8;
            barEditItem1.Name = "barEditItem1";
            barEditItem1.EditValueChanged += barEditItem1_EditValueChanged;
            // 
            // cbInterval
            // 
            cbInterval.AutoHeight = false;
            cbInterval.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbInterval.DropDownRows = 15;
            cbInterval.Name = "cbInterval";
            cbInterval.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // barCheckItem1
            // 
            barCheckItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barCheckItem1.BindableChecked = true;
            barCheckItem1.Caption = "Volume";
            barCheckItem1.Checked = true;
            barCheckItem1.Id = 9;
            barCheckItem1.Name = "barCheckItem1";
            barCheckItem1.CheckedChanged += barCheckItem1_CheckedChanged;
            // 
            // barCheckItem2
            // 
            barCheckItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barCheckItem2.BindableChecked = true;
            barCheckItem2.Caption = "Indicator";
            barCheckItem2.Checked = true;
            barCheckItem2.Id = 15;
            barCheckItem2.Name = "barCheckItem2";
            barCheckItem2.CheckedChanged += barCheckItem2_CheckedChanged;
            // 
            // barButtonItem2
            // 
            barButtonItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem2.Caption = "Indicators";
            barButtonItem2.Id = 11;
            barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem1
            // 
            barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem1.Caption = "Trade";
            barButtonItem1.Id = 10;
            barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem3
            // 
            barButtonItem3.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem3.Caption = "Stakan";
            barButtonItem3.Id = 12;
            barButtonItem3.Name = "barButtonItem3";
            // 
            // btnUpdate
            // 
            btnUpdate.Caption = "Update";
            btnUpdate.Id = 14;
            btnUpdate.ImageOptions.Image = (Image)resources.GetObject("btnUpdate.ImageOptions.Image");
            btnUpdate.ImageOptions.LargeImage = (Image)resources.GetObject("btnUpdate.ImageOptions.LargeImage");
            btnUpdate.Name = "btnUpdate";
            btnUpdate.ItemClick += btnUpdate_ItemClick;
            // 
            // bar2
            // 
            bar2.BarName = "Main menu";
            bar2.DockCol = 0;
            bar2.DockRow = 0;
            bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar2.OptionsBar.MultiLine = true;
            bar2.OptionsBar.UseWholeRow = true;
            bar2.Text = "Main menu";
            bar2.Visible = false;
            // 
            // bar3
            // 
            bar3.BarName = "Status bar";
            bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            bar3.DockCol = 0;
            bar3.DockRow = 0;
            bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(btnZoom), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem12), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem11), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem4), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem5), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem6), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem7), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem8), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem9), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem14), new DevExpress.XtraBars.LinkPersistInfo(barButtonItem10), new DevExpress.XtraBars.LinkPersistInfo(btnZoomIn), new DevExpress.XtraBars.LinkPersistInfo(txtZoom), new DevExpress.XtraBars.LinkPersistInfo(btnZoomOut) });
            bar3.OptionsBar.AllowQuickCustomization = false;
            bar3.OptionsBar.DrawDragBorder = false;
            bar3.OptionsBar.UseWholeRow = true;
            bar3.Text = "Status bar";
            // 
            // btnZoom
            // 
            btnZoom.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            btnZoom.Caption = "1000";
            btnZoom.Id = 28;
            btnZoom.Name = "btnZoom";
            btnZoom.Size = new Size(50, 0);
            btnZoom.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem12
            // 
            barButtonItem12.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem12.Caption = "900";
            barButtonItem12.Id = 27;
            barButtonItem12.Name = "barButtonItem12";
            barButtonItem12.Size = new Size(50, 0);
            barButtonItem12.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem11
            // 
            barButtonItem11.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem11.Caption = "800";
            barButtonItem11.Id = 26;
            barButtonItem11.Name = "barButtonItem11";
            barButtonItem11.Size = new Size(50, 0);
            barButtonItem11.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem4
            // 
            barButtonItem4.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem4.Caption = "700";
            barButtonItem4.Id = 19;
            barButtonItem4.Name = "barButtonItem4";
            barButtonItem4.Size = new Size(50, 0);
            barButtonItem4.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem5
            // 
            barButtonItem5.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem5.Caption = "600";
            barButtonItem5.Id = 20;
            barButtonItem5.Name = "barButtonItem5";
            barButtonItem5.Size = new Size(50, 0);
            barButtonItem5.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem6
            // 
            barButtonItem6.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem6.Caption = "500";
            barButtonItem6.Id = 21;
            barButtonItem6.Name = "barButtonItem6";
            barButtonItem6.Size = new Size(50, 0);
            barButtonItem6.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem7
            // 
            barButtonItem7.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem7.Caption = "400";
            barButtonItem7.Id = 22;
            barButtonItem7.Name = "barButtonItem7";
            barButtonItem7.Size = new Size(50, 0);
            barButtonItem7.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem8
            // 
            barButtonItem8.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem8.Caption = "300";
            barButtonItem8.Id = 23;
            barButtonItem8.Name = "barButtonItem8";
            barButtonItem8.Size = new Size(50, 0);
            barButtonItem8.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem9
            // 
            barButtonItem9.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem9.Caption = "200";
            barButtonItem9.Id = 24;
            barButtonItem9.Name = "barButtonItem9";
            barButtonItem9.Size = new Size(50, 0);
            barButtonItem9.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem14
            // 
            barButtonItem14.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem14.Caption = "100";
            barButtonItem14.Id = 31;
            barButtonItem14.Name = "barButtonItem14";
            barButtonItem14.Size = new Size(50, 0);
            barButtonItem14.ItemClick += btnZoom_ItemClick;
            // 
            // barButtonItem10
            // 
            barButtonItem10.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            barButtonItem10.Caption = "50";
            barButtonItem10.Id = 25;
            barButtonItem10.Name = "barButtonItem10";
            barButtonItem10.Size = new Size(50, 0);
            barButtonItem10.ItemClick += btnZoom_ItemClick;
            // 
            // btnZoomIn
            // 
            btnZoomIn.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            btnZoomIn.Caption = "Zoom In";
            btnZoomIn.Id = 13;
            btnZoomIn.Name = "btnZoomIn";
            btnZoomIn.ItemClick += btnZoomIn_ItemClick;
            // 
            // txtZoom
            // 
            txtZoom.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            txtZoom.Caption = "100";
            txtZoom.Id = 32;
            txtZoom.Name = "txtZoom";
            // 
            // btnZoomOut
            // 
            btnZoomOut.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            btnZoomOut.Caption = "Zoom Out";
            btnZoomOut.Id = 17;
            btnZoomOut.Name = "btnZoomOut";
            btnZoomOut.ItemClick += btnZoomOut_ItemClick;
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = DockStyle.Top;
            barDockControlTop.Location = new Point(0, 0);
            barDockControlTop.Manager = barManager1;
            barDockControlTop.Margin = new Padding(3, 4, 3, 4);
            barDockControlTop.Size = new Size(1738, 78);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = DockStyle.Bottom;
            barDockControlBottom.Location = new Point(0, 891);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Margin = new Padding(3, 4, 3, 4);
            barDockControlBottom.Size = new Size(1738, 38);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = DockStyle.Left;
            barDockControlLeft.Location = new Point(0, 78);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Margin = new Padding(3, 4, 3, 4);
            barDockControlLeft.Size = new Size(0, 813);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = DockStyle.Right;
            barDockControlRight.Location = new Point(1738, 78);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Margin = new Padding(3, 4, 3, 4);
            barDockControlRight.Size = new Size(0, 813);
            // 
            // FrmChart
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1738, 929);
            Controls.Add(chart);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            LookAndFeel.SkinName = "DevExpress Dark Style";
            LookAndFeel.UseDefaultLookAndFeel = false;
            Margin = new Padding(2);
            Name = "FrmChart";
            Text = "Form1";
            FormClosing += FrmChart_FormClosing;
            Load += FrmChart_Load;
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbInterval).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private UcSinexChart chart;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cbInterval;
        private DevExpress.XtraBars.BarCheckItem barCheckItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem btnZoomIn;
        private DevExpress.XtraBars.BarButtonItem btnUpdate;
        private DevExpress.XtraBars.BarCheckItem barCheckItem2;
        private DevExpress.XtraBars.BarButtonItem btnZoomOut;
        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
        private DevExpress.XtraBars.BarButtonItem btnZoom;
        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
        private DevExpress.XtraBars.BarButtonItem barButtonItem14;
        private DevExpress.XtraBars.BarStaticItem txtZoom;
    }
}