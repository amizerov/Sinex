namespace bot2
{
    partial class FrmTrade
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
            btnBuySell = new Button();
            txtBase = new TextBox();
            lblBase = new Label();
            txtQuote = new TextBox();
            lblQuote = new Label();
            lblPrice = new Label();
            lblAvlblBase = new Label();
            lblAvlblQuote = new Label();
            label2 = new Label();
            label3 = new Label();
            tabControl1 = new TabControl();
            tpBuy = new TabPage();
            tpSell = new TabPage();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // btnBuySell
            // 
            btnBuySell.BackColor = Color.DarkSeaGreen;
            btnBuySell.FlatStyle = FlatStyle.Popup;
            btnBuySell.Font = new Font("Segoe UI Black", 16F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuySell.ForeColor = Color.White;
            btnBuySell.Location = new Point(49, 462);
            btnBuySell.Name = "btnBuySell";
            btnBuySell.Size = new Size(528, 87);
            btnBuySell.TabIndex = 0;
            btnBuySell.Text = "Buy";
            btnBuySell.UseVisualStyleBackColor = false;
            // 
            // txtBase
            // 
            txtBase.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            txtBase.Location = new Point(196, 275);
            txtBase.Name = "txtBase";
            txtBase.PlaceholderText = "Amount";
            txtBase.Size = new Size(272, 50);
            txtBase.TabIndex = 2;
            txtBase.Text = "0";
            txtBase.TextAlign = HorizontalAlignment.Right;
            txtBase.TextChanged += txtBase_TextChanged;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblBase.Location = new Point(474, 275);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(78, 45);
            lblBase.TabIndex = 3;
            lblBase.Text = "ZEN";
            // 
            // txtQuote
            // 
            txtQuote.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            txtQuote.Location = new Point(196, 377);
            txtQuote.Name = "txtQuote";
            txtQuote.PlaceholderText = "Amount";
            txtQuote.Size = new Size(272, 50);
            txtQuote.TabIndex = 2;
            txtQuote.Text = "0";
            txtQuote.TextAlign = HorizontalAlignment.Right;
            txtQuote.TextChanged += txtQuote_TextChanged;
            // 
            // lblQuote
            // 
            lblQuote.AutoSize = true;
            lblQuote.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblQuote.Location = new Point(474, 377);
            lblQuote.Name = "lblQuote";
            lblQuote.Size = new Size(97, 45);
            lblQuote.TabIndex = 3;
            lblQuote.Text = "USDT";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrice.Location = new Point(130, 109);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(368, 96);
            lblPrice.TabIndex = 4;
            lblPrice.Text = "28.022365";
            // 
            // lblAvlblBase
            // 
            lblAvlblBase.AutoSize = true;
            lblAvlblBase.Location = new Point(196, 243);
            lblAvlblBase.Name = "lblAvlblBase";
            lblAvlblBase.Size = new Size(98, 25);
            lblAvlblBase.TabIndex = 5;
            lblAvlblBase.Text = "Aveilable 0";
            // 
            // lblAvlblQuote
            // 
            lblAvlblQuote.AutoSize = true;
            lblAvlblQuote.Location = new Point(196, 345);
            lblAvlblQuote.Name = "lblAvlblQuote";
            lblAvlblQuote.Size = new Size(98, 25);
            lblAvlblQuote.TabIndex = 5;
            lblAvlblQuote.Text = "Aveilable 0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(49, 382);
            label2.Name = "label2";
            label2.Size = new Size(75, 38);
            label2.TabIndex = 5;
            label2.Text = "Total";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(49, 280);
            label3.Name = "label3";
            label3.Size = new Size(116, 38);
            label3.TabIndex = 5;
            label3.Text = "Amount";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tpBuy);
            tabControl1.Controls.Add(tpSell);
            tabControl1.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point);
            tabControl1.Location = new Point(144, 22);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(339, 66);
            tabControl1.TabIndex = 6;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tpBuy
            // 
            tpBuy.BackColor = Color.Transparent;
            tpBuy.Location = new Point(4, 69);
            tpBuy.Name = "tpBuy";
            tpBuy.Padding = new Padding(3);
            tpBuy.Size = new Size(331, 0);
            tpBuy.TabIndex = 0;
            tpBuy.Text = "   BUY";
            // 
            // tpSell
            // 
            tpSell.BackColor = Color.Transparent;
            tpSell.Location = new Point(4, 54);
            tpSell.Name = "tpSell";
            tpSell.Padding = new Padding(3);
            tpSell.Size = new Size(285, 1);
            tpSell.TabIndex = 1;
            tpSell.Text = "   SELL";
            // 
            // FrmTrade
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 607);
            Controls.Add(tabControl1);
            Controls.Add(lblAvlblQuote);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(lblAvlblBase);
            Controls.Add(lblPrice);
            Controls.Add(lblQuote);
            Controls.Add(txtQuote);
            Controls.Add(lblBase);
            Controls.Add(txtBase);
            Controls.Add(btnBuySell);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmTrade";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "FrmTade";
            TopMost = true;
            FormClosing += FrmTrade_FormClosing;
            Load += FrmTade_Load;
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBuySell;
        private TextBox txtBase;
        private Label lblBase;
        private TextBox txtQuote;
        private Label lblQuote;
        private Label lblPrice;
        private Label lblAvlblBase;
        private Label lblAvlblQuote;
        private Label label2;
        private Label label3;
        private TabControl tabControl1;
        private TabPage tpBuy;
        private TabPage tpSell;
    }
}