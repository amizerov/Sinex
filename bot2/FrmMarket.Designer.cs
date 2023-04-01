namespace bot2
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
            cbExchange = new ComboBox();
            label1 = new Label();
            dgProducts = new DataGridView();
            txtSearch = new TextBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgProducts).BeginInit();
            SuspendLayout();
            // 
            // cbExchange
            // 
            cbExchange.DropDownStyle = ComboBoxStyle.DropDownList;
            cbExchange.FormattingEnabled = true;
            cbExchange.Location = new Point(154, 22);
            cbExchange.Name = "cbExchange";
            cbExchange.Size = new Size(150, 33);
            cbExchange.TabIndex = 0;
            cbExchange.SelectedIndexChanged += cbExchange_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(62, 22);
            label1.Name = "label1";
            label1.Size = new Size(86, 25);
            label1.TabIndex = 1;
            label1.Text = "Exchange";
            // 
            // dgProducts
            // 
            dgProducts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgProducts.Location = new Point(12, 124);
            dgProducts.MultiSelect = false;
            dgProducts.Name = "dgProducts";
            dgProducts.RowHeadersVisible = false;
            dgProducts.RowHeadersWidth = 62;
            dgProducts.RowTemplate.Height = 33;
            dgProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgProducts.ShowEditingIcon = false;
            dgProducts.Size = new Size(1282, 641);
            dgProducts.TabIndex = 2;
            dgProducts.CellDoubleClick += dgProducts_CellDoubleClick;
            dgProducts.RowPrePaint += dgProducts_RowPrePaint;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(154, 71);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(150, 31);
            txtSearch.TabIndex = 3;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 71);
            label2.Name = "label2";
            label2.Size = new Size(128, 25);
            label2.TabIndex = 1;
            label2.Text = "Search symbol";
            // 
            // FrmMarket
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1306, 777);
            Controls.Add(txtSearch);
            Controls.Add(dgProducts);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbExchange);
            Name = "FrmMarket";
            Text = "Market watch";
            Load += FrmMarket_Load;
            ((System.ComponentModel.ISupportInitialize)dgProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cbExchange;
        private Label label1;
        private DataGridView dgProducts;
        private TextBox txtSearch;
        private Label label2;
    }
}