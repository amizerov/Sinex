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
            btnBuy = new Button();
            btnSell = new Button();
            txtBase = new TextBox();
            lblBase = new Label();
            txtQuote = new TextBox();
            lblQuote = new Label();
            lblPrice = new Label();
            lblAvlblBase = new Label();
            lblAvlblQuote = new Label();
            SuspendLayout();
            // 
            // btnBuy
            // 
            btnBuy.BackColor = Color.DarkSeaGreen;
            btnBuy.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnBuy.ForeColor = Color.White;
            btnBuy.Location = new Point(41, 255);
            btnBuy.Name = "btnBuy";
            btnBuy.Size = new Size(260, 69);
            btnBuy.TabIndex = 0;
            btnBuy.Text = "Buy";
            btnBuy.UseVisualStyleBackColor = false;
            // 
            // btnSell
            // 
            btnSell.BackColor = Color.IndianRed;
            btnSell.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point);
            btnSell.ForeColor = Color.White;
            btnSell.Location = new Point(318, 255);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(260, 69);
            btnSell.TabIndex = 1;
            btnSell.Text = "Sell";
            btnSell.UseVisualStyleBackColor = false;
            // 
            // txtBase
            // 
            txtBase.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            txtBase.Location = new Point(50, 179);
            txtBase.Name = "txtBase";
            txtBase.PlaceholderText = "Amount";
            txtBase.Size = new Size(150, 50);
            txtBase.TabIndex = 2;
            txtBase.Text = "0";
            txtBase.TextAlign = HorizontalAlignment.Right;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblBase.Location = new Point(206, 179);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(78, 45);
            lblBase.TabIndex = 3;
            lblBase.Text = "ZEN";
            // 
            // txtQuote
            // 
            txtQuote.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            txtQuote.Location = new Point(327, 179);
            txtQuote.Name = "txtQuote";
            txtQuote.PlaceholderText = "Amount";
            txtQuote.Size = new Size(150, 50);
            txtQuote.TabIndex = 2;
            txtQuote.Text = "0";
            txtQuote.TextAlign = HorizontalAlignment.Right;
            // 
            // lblQuote
            // 
            lblQuote.AutoSize = true;
            lblQuote.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            lblQuote.Location = new Point(483, 179);
            lblQuote.Name = "lblQuote";
            lblQuote.Size = new Size(97, 45);
            lblQuote.TabIndex = 3;
            lblQuote.Text = "USDT";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point);
            lblPrice.Location = new Point(119, 39);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(368, 96);
            lblPrice.TabIndex = 4;
            lblPrice.Text = "28.022365";
            // 
            // lblAvlblBase
            // 
            lblAvlblBase.AutoSize = true;
            lblAvlblBase.Location = new Point(50, 151);
            lblAvlblBase.Name = "lblAvlblBase";
            lblAvlblBase.Size = new Size(67, 25);
            lblAvlblBase.TabIndex = 5;
            lblAvlblBase.Text = "Avlbl 0";
            // 
            // lblAvlblQuote
            // 
            lblAvlblQuote.AutoSize = true;
            lblAvlblQuote.Location = new Point(327, 151);
            lblAvlblQuote.Name = "lblAvlblQuote";
            lblAvlblQuote.Size = new Size(67, 25);
            lblAvlblQuote.TabIndex = 5;
            lblAvlblQuote.Text = "Avlbl 0";
            // 
            // FrmTrade
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 366);
            Controls.Add(lblAvlblQuote);
            Controls.Add(lblAvlblBase);
            Controls.Add(lblPrice);
            Controls.Add(lblQuote);
            Controls.Add(txtQuote);
            Controls.Add(lblBase);
            Controls.Add(txtBase);
            Controls.Add(btnSell);
            Controls.Add(btnBuy);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmTrade";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "FrmTade";
            TopMost = true;
            FormClosing += FrmTrade_FormClosing;
            Load += FrmTade_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBuy;
        private Button btnSell;
        private TextBox txtBase;
        private Label lblBase;
        private TextBox txtQuote;
        private Label lblQuote;
        private Label lblPrice;
        private Label lblAvlblBase;
        private Label lblAvlblQuote;
    }
}