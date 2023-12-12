namespace bot5
{
    partial class FrmWin1
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
            txtSearch = new TextBox();
            panel = new Panel();
            lblSym = new Label();
            lblExc1 = new Label();
            lblExc2 = new Label();
            lblMaxProc = new Label();
            btnReload = new Button();
            btnArbit = new Button();
            button3 = new Button();
            btnScan = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProds).BeginInit();
            SuspendLayout();
            // 
            // dgvProds
            // 
            dgvProds.AllowUserToAddRows = false;
            dgvProds.AllowUserToDeleteRows = false;
            dgvProds.AllowUserToResizeRows = false;
            dgvProds.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvProds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProds.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvProds.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvProds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProds.Location = new Point(12, 100);
            dgvProds.MultiSelect = false;
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(392, 820);
            dgvProds.TabIndex = 0;
            dgvProds.CellMouseClick += dgvProds_CellMouseClick;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 15.8571434F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtSearch.Location = new Point(12, 14);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(392, 57);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ControlLight;
            panel.Location = new Point(430, 250);
            panel.Name = "panel";
            panel.Size = new Size(630, 673);
            panel.TabIndex = 2;
            // 
            // lblSym
            // 
            lblSym.AutoSize = true;
            lblSym.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSym.Location = new Point(443, 35);
            lblSym.Name = "lblSym";
            lblSym.Size = new Size(268, 112);
            lblSym.TabIndex = 3;
            lblSym.Text = "label1";
            // 
            // lblExc1
            // 
            lblExc1.AutoSize = true;
            lblExc1.Font = new Font("Segoe UI", 15.8571434F);
            lblExc1.Location = new Point(430, 184);
            lblExc1.Name = "lblExc1";
            lblExc1.Size = new Size(120, 50);
            lblExc1.TabIndex = 3;
            lblExc1.Text = "label1";
            // 
            // lblExc2
            // 
            lblExc2.AutoSize = true;
            lblExc2.Font = new Font("Segoe UI", 15.8571434F);
            lblExc2.Location = new Point(556, 184);
            lblExc2.Name = "lblExc2";
            lblExc2.Size = new Size(120, 50);
            lblExc2.TabIndex = 3;
            lblExc2.Text = "label1";
            // 
            // lblMaxProc
            // 
            lblMaxProc.AutoSize = true;
            lblMaxProc.Font = new Font("Segoe UI", 15.8571434F);
            lblMaxProc.Location = new Point(690, 184);
            lblMaxProc.Name = "lblMaxProc";
            lblMaxProc.Size = new Size(120, 50);
            lblMaxProc.TabIndex = 3;
            lblMaxProc.Text = "label1";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Location = new Point(927, 26);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(131, 54);
            btnReload.TabIndex = 4;
            btnReload.Text = "Обновить";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // btnArbit
            // 
            btnArbit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArbit.Location = new Point(927, 83);
            btnArbit.Name = "btnArbit";
            btnArbit.Size = new Size(131, 54);
            btnArbit.TabIndex = 4;
            btnArbit.Text = "Арбитраж";
            btnArbit.UseVisualStyleBackColor = true;
            btnArbit.Click += btnArbit_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button3.Location = new Point(927, 140);
            button3.Name = "button3";
            button3.Size = new Size(131, 54);
            button3.TabIndex = 4;
            button3.Text = "Бот1";
            button3.UseVisualStyleBackColor = true;
            // 
            // btnScan
            // 
            btnScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScan.Location = new Point(775, 26);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(131, 54);
            btnScan.TabIndex = 4;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // FrmWin1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1086, 935);
            Controls.Add(btnScan);
            Controls.Add(button3);
            Controls.Add(btnArbit);
            Controls.Add(btnReload);
            Controls.Add(lblMaxProc);
            Controls.Add(lblExc2);
            Controls.Add(lblExc1);
            Controls.Add(lblSym);
            Controls.Add(panel);
            Controls.Add(txtSearch);
            Controls.Add(dgvProds);
            MaximizeBox = false;
            Name = "FrmWin1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Склейки по биржам";
            Load += FrmWin1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProds).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProds;
        private TextBox txtSearch;
        private Panel panel;
        private Label lblSym;
        private Label lblExc1;
        private Label lblExc2;
        private Label lblMaxProc;
        private Button btnReload;
        private Button btnArbit;
        private Button button3;
        private Button btnScan;
    }
}