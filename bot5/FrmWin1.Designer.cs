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
            btnBot1 = new Button();
            btnScan = new Button();
            statusStrip1 = new StatusStrip();
            statusCount = new ToolStripStatusLabel();
            lblChain = new Label();
            btnBot6 = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProds).BeginInit();
            statusStrip1.SuspendLayout();
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
            dgvProds.Margin = new Padding(3, 4, 3, 4);
            dgvProds.MultiSelect = false;
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(459, 942);
            dgvProds.TabIndex = 0;
            dgvProds.CellMouseClick += dgvProds_CellMouseClick;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 15.8571434F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtSearch.Location = new Point(12, 14);
            txtSearch.Margin = new Padding(3, 4, 3, 4);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(391, 57);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ControlLight;
            panel.Location = new Point(487, 277);
            panel.Margin = new Padding(3, 4, 3, 4);
            panel.Name = "panel";
            panel.Size = new Size(1011, 765);
            panel.TabIndex = 2;
            // 
            // lblSym
            // 
            lblSym.AutoSize = true;
            lblSym.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSym.Location = new Point(499, 38);
            lblSym.Name = "lblSym";
            lblSym.Size = new Size(268, 112);
            lblSym.TabIndex = 3;
            lblSym.Text = "label1";
            // 
            // lblExc1
            // 
            lblExc1.AutoSize = true;
            lblExc1.Font = new Font("Segoe UI", 15.8571434F);
            lblExc1.Location = new Point(487, 188);
            lblExc1.Name = "lblExc1";
            lblExc1.Size = new Size(156, 50);
            lblExc1.TabIndex = 3;
            lblExc1.Text = "ExchBuy";
            // 
            // lblExc2
            // 
            lblExc2.AutoSize = true;
            lblExc2.Font = new Font("Segoe UI", 15.8571434F);
            lblExc2.Location = new Point(818, 188);
            lblExc2.Name = "lblExc2";
            lblExc2.Size = new Size(153, 50);
            lblExc2.TabIndex = 3;
            lblExc2.Text = "ExchSell";
            // 
            // lblMaxProc
            // 
            lblMaxProc.AutoSize = true;
            lblMaxProc.Font = new Font("Segoe UI", 15.8571434F);
            lblMaxProc.Location = new Point(1031, 188);
            lblMaxProc.Name = "lblMaxProc";
            lblMaxProc.Size = new Size(95, 50);
            lblMaxProc.TabIndex = 3;
            lblMaxProc.Text = "Proc";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Location = new Point(1365, 26);
            btnReload.Margin = new Padding(3, 4, 3, 4);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(130, 54);
            btnReload.TabIndex = 4;
            btnReload.Text = "Обновить";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // btnArbit
            // 
            btnArbit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArbit.Location = new Point(1365, 83);
            btnArbit.Margin = new Padding(3, 4, 3, 4);
            btnArbit.Name = "btnArbit";
            btnArbit.Size = new Size(130, 54);
            btnArbit.TabIndex = 4;
            btnArbit.Text = "Арбитраж";
            btnArbit.UseVisualStyleBackColor = true;
            btnArbit.Click += btnArbit_Click;
            // 
            // btnBot1
            // 
            btnBot1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBot1.Location = new Point(1365, 140);
            btnBot1.Margin = new Padding(3, 4, 3, 4);
            btnBot1.Name = "btnBot1";
            btnBot1.Size = new Size(130, 54);
            btnBot1.TabIndex = 4;
            btnBot1.Text = "Бот1";
            btnBot1.UseVisualStyleBackColor = true;
            btnBot1.Click += btnBot1_Click;
            // 
            // btnScan
            // 
            btnScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScan.Location = new Point(1212, 26);
            btnScan.Margin = new Padding(3, 4, 3, 4);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(130, 54);
            btnScan.TabIndex = 4;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(28, 28);
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusCount });
            statusStrip1.Location = new Point(0, 1061);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(2, 0, 14, 0);
            statusStrip1.Size = new Size(1522, 39);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusCount
            // 
            statusCount.Name = "statusCount";
            statusCount.Size = new Size(206, 30);
            statusCount.Text = "toolStripStatusLabel1";
            // 
            // lblChain
            // 
            lblChain.AutoSize = true;
            lblChain.Location = new Point(695, 199);
            lblChain.Name = "lblChain";
            lblChain.Size = new Size(66, 30);
            lblChain.TabIndex = 6;
            lblChain.Text = "Chain";
            // 
            // btnBot6
            // 
            btnBot6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBot6.Location = new Point(1365, 197);
            btnBot6.Margin = new Padding(3, 4, 3, 4);
            btnBot6.Name = "btnBot6";
            btnBot6.Size = new Size(130, 54);
            btnBot6.TabIndex = 4;
            btnBot6.Text = "Бот6";
            btnBot6.UseVisualStyleBackColor = true;
            btnBot6.Click += btnBot1_Click;
            // 
            // FrmWin1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1522, 1100);
            Controls.Add(lblChain);
            Controls.Add(statusStrip1);
            Controls.Add(btnScan);
            Controls.Add(btnBot6);
            Controls.Add(btnBot1);
            Controls.Add(btnArbit);
            Controls.Add(btnReload);
            Controls.Add(lblMaxProc);
            Controls.Add(lblExc2);
            Controls.Add(lblExc1);
            Controls.Add(lblSym);
            Controls.Add(panel);
            Controls.Add(txtSearch);
            Controls.Add(dgvProds);
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "FrmWin1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Склейки по биржам";
            Load += FrmWin1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProds).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
        private Button btnBot1;
        private Button btnScan;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel statusCount;
        private Label lblChain;
        private Button btnBot6;
    }
}