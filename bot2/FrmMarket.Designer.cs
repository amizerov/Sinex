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
            cbQuote = new ComboBox();
            label3 = new Label();
            btnBot1 = new Button();
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
            dgProducts.EditMode = DataGridViewEditMode.EditProgrammatically;
            dgProducts.Location = new Point(12, 124);
            dgProducts.MultiSelect = false;
            dgProducts.Name = "dgProducts";
            dgProducts.RowHeadersVisible = false;
            dgProducts.RowHeadersWidth = 62;
            dgProducts.RowTemplate.Height = 33;
            dgProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgProducts.ShowEditingIcon = false;
            dgProducts.Size = new Size(925, 641);
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
            // cbQuote
            // 
            cbQuote.DropDownStyle = ComboBoxStyle.DropDownList;
            cbQuote.FormattingEnabled = true;
            cbQuote.Location = new Point(324, 69);
            cbQuote.Name = "cbQuote";
            cbQuote.Size = new Size(115, 33);
            cbQuote.TabIndex = 4;
            cbQuote.SelectedIndexChanged += cbQuote_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(324, 22);
            label3.Name = "label3";
            label3.Size = new Size(107, 25);
            label3.TabIndex = 5;
            label3.Text = "Quote asset";
            // 
            // btnBot1
            // 
            btnBot1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBot1.Location = new Point(851, 17);
            btnBot1.Name = "btnBot1";
            btnBot1.Size = new Size(86, 34);
            btnBot1.TabIndex = 6;
            btnBot1.Text = "bot1";
            btnBot1.UseVisualStyleBackColor = true;
            btnBot1.Click += btnBot1_Click;
            // 
            // FrmMarket
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(949, 777);
            Controls.Add(btnBot1);
            Controls.Add(label3);
            Controls.Add(cbQuote);
            Controls.Add(txtSearch);
            Controls.Add(dgProducts);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cbExchange);
            Name = "FrmMarket";
            Text = "Market watch";
            FormClosing += FrmMarket_FormClosing;
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
        private ComboBox cbQuote;
        private Label label3;
        private Button btnBot1;
    }
}