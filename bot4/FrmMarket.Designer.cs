namespace bot4
{
    partial class FrmMarket
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
            cbExchange = new DevExpress.XtraEditors.ComboBoxEdit();
            cbQuote = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            btnAccount = new DevExpress.XtraEditors.SimpleButton();
            gcProducts = new DevExpress.XtraGrid.GridControl();
            gvProducts = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)cbExchange.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbQuote.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gcProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gvProducts).BeginInit();
            SuspendLayout();
            // 
            // cbExchange
            // 
            cbExchange.Location = new Point(42, 37);
            cbExchange.Margin = new Padding(3, 4, 3, 4);
            cbExchange.Name = "cbExchange";
            cbExchange.Properties.Appearance.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point);
            cbExchange.Properties.Appearance.Options.UseFont = true;
            cbExchange.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbExchange.Properties.DropDownRows = 9;
            cbExchange.Properties.Sorted = true;
            cbExchange.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbExchange.Size = new Size(187, 48);
            cbExchange.TabIndex = 0;
            cbExchange.SelectedIndexChanged += cbExchange_SelectedIndexChanged;
            // 
            // cbQuote
            // 
            cbQuote.Location = new Point(267, 46);
            cbQuote.Margin = new Padding(3, 4, 3, 4);
            cbQuote.Name = "cbQuote";
            cbQuote.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cbQuote.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cbQuote.Size = new Size(138, 38);
            cbQuote.TabIndex = 1;
            cbQuote.SelectedIndexChanged += cbQuote_SelectedIndexChanged;
            // 
            // labelControl1
            // 
            labelControl1.Location = new Point(42, 9);
            labelControl1.Margin = new Padding(3, 4, 3, 4);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(82, 23);
            labelControl1.TabIndex = 2;
            labelControl1.Text = "Exchange";
            // 
            // labelControl2
            // 
            labelControl2.Location = new Point(267, 18);
            labelControl2.Margin = new Padding(3, 4, 3, 4);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new Size(98, 23);
            labelControl2.TabIndex = 3;
            labelControl2.Text = "Quote asset";
            // 
            // btnAccount
            // 
            btnAccount.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAccount.Location = new Point(715, 14);
            btnAccount.Margin = new Padding(3, 4, 3, 4);
            btnAccount.Name = "btnAccount";
            btnAccount.Size = new Size(228, 71);
            btnAccount.TabIndex = 5;
            btnAccount.Text = "Account";
            // 
            // gcProducts
            // 
            gcProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gcProducts.EmbeddedNavigator.Margin = new Padding(3, 4, 3, 4);
            gcProducts.Location = new Point(28, 110);
            gcProducts.MainView = gvProducts;
            gcProducts.Margin = new Padding(3, 4, 3, 4);
            gcProducts.Name = "gcProducts";
            gcProducts.Size = new Size(932, 741);
            gcProducts.TabIndex = 6;
            gcProducts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gvProducts });
            // 
            // gvProducts
            // 
            gvProducts.GridControl = gcProducts;
            gvProducts.Name = "gvProducts";
            gvProducts.OptionsBehavior.Editable = false;
            gvProducts.OptionsSelection.EnableAppearanceFocusedCell = false;
            gvProducts.OptionsSelection.UseIndicatorForSelection = false;
            gvProducts.OptionsView.ShowGroupPanel = false;
            gvProducts.DoubleClick += gvProducts_DoubleClick;
            // 
            // FrmMarket
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(985, 888);
            Controls.Add(gcProducts);
            Controls.Add(btnAccount);
            Controls.Add(labelControl2);
            Controls.Add(labelControl1);
            Controls.Add(cbQuote);
            Controls.Add(cbExchange);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "FrmMarket";
            Text = "Market watch";
            FormClosing += FrmMarket_FormClosing;
            Load += FrmMarket_Load;
            ((System.ComponentModel.ISupportInitialize)cbExchange.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbQuote.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)gcProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)gvProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit cbExchange;
        private DevExpress.XtraEditors.ComboBoxEdit cbQuote;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnAccount;
        private DevExpress.XtraGrid.GridControl gcProducts;
        private DevExpress.XtraGrid.Views.Grid.GridView gvProducts;
    }
}