namespace bot2
{
    partial class FrmAccount
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
            dataGridView1 = new DataGridView();
            lblBnbAvailable = new Label();
            lblUsdtAvailable = new Label();
            label1 = new Label();
            lblBalance = new Label();
            btnTrade = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            dataGridView1.Location = new Point(12, 126);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 33;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(524, 260);
            dataGridView1.TabIndex = 0;
            // 
            // lblBnbAvailable
            // 
            lblBnbAvailable.AutoSize = true;
            lblBnbAvailable.Location = new Point(18, 20);
            lblBnbAvailable.Name = "lblBnbAvailable";
            lblBnbAvailable.Size = new Size(98, 25);
            lblBnbAvailable.TabIndex = 1;
            lblBnbAvailable.Text = "BNB: 20,50";
            // 
            // lblUsdtAvailable
            // 
            lblUsdtAvailable.AutoSize = true;
            lblUsdtAvailable.Location = new Point(18, 56);
            lblUsdtAvailable.Name = "lblUsdtAvailable";
            lblUsdtAvailable.Size = new Size(118, 25);
            lblUsdtAvailable.TabIndex = 1;
            lblUsdtAvailable.Text = "USDT: 120,50";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 98);
            label1.Name = "label1";
            label1.Size = new Size(138, 25);
            label1.TabIndex = 2;
            label1.Text = "Open positions:";
            // 
            // lblBalance
            // 
            lblBalance.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBalance.AutoSize = true;
            lblBalance.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblBalance.Location = new Point(300, 8);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(236, 45);
            lblBalance.TabIndex = 1;
            lblBalance.Text = "Balance: 220,50";
            // 
            // btnTrade
            // 
            btnTrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTrade.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            btnTrade.Location = new Point(353, 61);
            btnTrade.Name = "btnTrade";
            btnTrade.Size = new Size(180, 60);
            btnTrade.TabIndex = 3;
            btnTrade.Text = "Trade";
            btnTrade.UseVisualStyleBackColor = true;
            btnTrade.Click += btnTrade_Click;
            // 
            // FrmAccount
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(552, 398);
            Controls.Add(btnTrade);
            Controls.Add(label1);
            Controls.Add(lblBalance);
            Controls.Add(lblUsdtAvailable);
            Controls.Add(lblBnbAvailable);
            Controls.Add(dataGridView1);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "FrmAccount";
            Text = "Account";
            FormClosing += FrmAccount_FormClosing;
            Load += FrmAccount_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label lblBnbAvailable;
        private Label lblUsdtAvailable;
        private Label label1;
        private Label lblBalance;
        private Button btnTrade;
    }
}