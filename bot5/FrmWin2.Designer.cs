namespace bot5
{
    partial class FrmWin2
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
            dgvProds = new DataGridView();
            btnUpdate = new Button();
            btnSend = new Button();
            txtExch = new TextBox();
            txtMon = new TextBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProds).BeginInit();
            SuspendLayout();
            // 
            // dgvProds
            // 
            dgvProds.AllowUserToAddRows = false;
            dgvProds.AllowUserToDeleteRows = false;
            dgvProds.AllowUserToResizeRows = false;
            dgvProds.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProds.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvProds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProds.Location = new Point(12, 23);
            dgvProds.MultiSelect = false;
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(1053, 981);
            dgvProds.TabIndex = 0;
            // 
            // btnUpdate
            // 
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnUpdate.Location = new Point(1114, 23);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(131, 51);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "Обновить";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.Location = new Point(1274, 23);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(197, 51);
            btnSend.TabIndex = 1;
            btnSend.Text = "Отправка";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // txtExch
            // 
            txtExch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtExch.Location = new Point(1114, 190);
            txtExch.Multiline = true;
            txtExch.Name = "txtExch";
            txtExch.Size = new Size(357, 309);
            txtExch.TabIndex = 2;
            txtExch.Text = "Huobi";
            // 
            // txtMon
            // 
            txtMon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtMon.Location = new Point(1114, 605);
            txtMon.Multiline = true;
            txtMon.Name = "txtMon";
            txtMon.Size = new Size(357, 331);
            txtMon.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1119, 148);
            label1.Name = "label1";
            label1.Size = new Size(127, 30);
            label1.TabIndex = 3;
            label1.Text = "Биржы - ЧС";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1122, 564);
            label2.Name = "label2";
            label2.Size = new Size(291, 30);
            label2.TabIndex = 3;
            label2.Text = "Монеты - ЧС. Через запятую";
            // 
            // FrmWin2
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1505, 1016);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtMon);
            Controls.Add(txtExch);
            Controls.Add(btnSend);
            Controls.Add(btnUpdate);
            Controls.Add(dgvProds);
            Name = "FrmWin2";
            Text = "Арбитраж";
            Load += FrmWin2_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProds).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProds;
        public Button btnUpdate;
        public Button btnSend;
        private TextBox txtExch;
        private TextBox txtMon;
        private Label label1;
        private Label label2;
    }
}