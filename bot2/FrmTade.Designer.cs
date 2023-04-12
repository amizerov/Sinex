namespace bot2
{
    partial class FrmTade
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
            SuspendLayout();
            // 
            // btnBuy
            // 
            btnBuy.Location = new Point(39, 174);
            btnBuy.Name = "btnBuy";
            btnBuy.Size = new Size(150, 69);
            btnBuy.TabIndex = 0;
            btnBuy.Text = "Buy";
            btnBuy.UseVisualStyleBackColor = true;
            // 
            // btnSell
            // 
            btnSell.Location = new Point(253, 174);
            btnSell.Name = "btnSell";
            btnSell.Size = new Size(147, 69);
            btnSell.TabIndex = 1;
            btnSell.Text = "Sell";
            btnSell.UseVisualStyleBackColor = true;
            // 
            // txtBase
            // 
            txtBase.Location = new Point(39, 111);
            txtBase.Name = "txtBase";
            txtBase.Size = new Size(150, 31);
            txtBase.TabIndex = 2;
            // 
            // lblBase
            // 
            lblBase.AutoSize = true;
            lblBase.Location = new Point(39, 74);
            lblBase.Name = "lblBase";
            lblBase.Size = new Size(136, 25);
            lblBase.TabIndex = 3;
            lblBase.Text = "Amount of ZEN";
            // 
            // txtQuote
            // 
            txtQuote.Location = new Point(253, 111);
            txtQuote.Name = "txtQuote";
            txtQuote.Size = new Size(150, 31);
            txtQuote.TabIndex = 2;
            // 
            // lblQuote
            // 
            lblQuote.AutoSize = true;
            lblQuote.Location = new Point(253, 74);
            lblQuote.Name = "lblQuote";
            lblQuote.Size = new Size(147, 25);
            lblQuote.TabIndex = 3;
            lblQuote.Text = "Amount of USDT";
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(177, 20);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(96, 25);
            lblPrice.TabIndex = 4;
            lblPrice.Text = "28.022365";
            // 
            // FrmTade
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(445, 283);
            Controls.Add(lblPrice);
            Controls.Add(lblQuote);
            Controls.Add(txtQuote);
            Controls.Add(lblBase);
            Controls.Add(txtBase);
            Controls.Add(btnSell);
            Controls.Add(btnBuy);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmTade";
            ShowInTaskbar = false;
            Text = "FrmTade";
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
    }
}