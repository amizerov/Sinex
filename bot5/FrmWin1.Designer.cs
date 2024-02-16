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
            dgvProds.Location = new Point(7, 50);
            dgvProds.Margin = new Padding(2, 2, 2, 2);
            dgvProds.MultiSelect = false;
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(268, 471);
            dgvProds.TabIndex = 0;
            dgvProds.CellMouseClick += dgvProds_CellMouseClick;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 15.8571434F, FontStyle.Regular, GraphicsUnit.Point, 204);
            txtSearch.Location = new Point(7, 7);
            txtSearch.Margin = new Padding(2, 2, 2, 2);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(230, 36);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ControlLight;
            panel.Location = new Point(284, 125);
            panel.Margin = new Padding(2, 2, 2, 2);
            panel.Name = "panel";
            panel.Size = new Size(590, 396);
            panel.TabIndex = 2;
            // 
            // lblSym
            // 
            lblSym.AutoSize = true;
            lblSym.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 204);
            lblSym.Location = new Point(291, 19);
            lblSym.Margin = new Padding(2, 0, 2, 0);
            lblSym.Name = "lblSym";
            lblSym.Size = new Size(155, 65);
            lblSym.TabIndex = 3;
            lblSym.Text = "label1";
            // 
            // lblExc1
            // 
            lblExc1.AutoSize = true;
            lblExc1.Font = new Font("Segoe UI", 15.8571434F);
            lblExc1.Location = new Point(284, 94);
            lblExc1.Margin = new Padding(2, 0, 2, 0);
            lblExc1.Name = "lblExc1";
            lblExc1.Size = new Size(71, 30);
            lblExc1.TabIndex = 3;
            lblExc1.Text = "label1";
            // 
            // lblExc2
            // 
            lblExc2.AutoSize = true;
            lblExc2.Font = new Font("Segoe UI", 15.8571434F);
            lblExc2.Location = new Point(357, 94);
            lblExc2.Margin = new Padding(2, 0, 2, 0);
            lblExc2.Name = "lblExc2";
            lblExc2.Size = new Size(71, 30);
            lblExc2.TabIndex = 3;
            lblExc2.Text = "label1";
            // 
            // lblMaxProc
            // 
            lblMaxProc.AutoSize = true;
            lblMaxProc.Font = new Font("Segoe UI", 15.8571434F);
            lblMaxProc.Location = new Point(435, 94);
            lblMaxProc.Margin = new Padding(2, 0, 2, 0);
            lblMaxProc.Name = "lblMaxProc";
            lblMaxProc.Size = new Size(71, 30);
            lblMaxProc.TabIndex = 3;
            lblMaxProc.Text = "label1";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Location = new Point(796, 13);
            btnReload.Margin = new Padding(2, 2, 2, 2);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(76, 27);
            btnReload.TabIndex = 4;
            btnReload.Text = "Обновить";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // btnArbit
            // 
            btnArbit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArbit.Location = new Point(796, 42);
            btnArbit.Margin = new Padding(2, 2, 2, 2);
            btnArbit.Name = "btnArbit";
            btnArbit.Size = new Size(76, 27);
            btnArbit.TabIndex = 4;
            btnArbit.Text = "Арбитраж";
            btnArbit.UseVisualStyleBackColor = true;
            btnArbit.Click += btnArbit_Click;
            // 
            // btnBot1
            // 
            btnBot1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBot1.Location = new Point(796, 70);
            btnBot1.Margin = new Padding(2, 2, 2, 2);
            btnBot1.Name = "btnBot1";
            btnBot1.Size = new Size(76, 27);
            btnBot1.TabIndex = 4;
            btnBot1.Text = "Бот1";
            btnBot1.UseVisualStyleBackColor = true;
            btnBot1.Click += btnBot1_Click;
            // 
            // btnScan
            // 
            btnScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnScan.Location = new Point(707, 13);
            btnScan.Margin = new Padding(2, 2, 2, 2);
            btnScan.Name = "btnScan";
            btnScan.Size = new Size(76, 27);
            btnScan.TabIndex = 4;
            btnScan.Text = "Scan";
            btnScan.UseVisualStyleBackColor = true;
            btnScan.Click += btnScan_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(28, 28);
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusCount });
            statusStrip1.Location = new Point(0, 528);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 8, 0);
            statusStrip1.Size = new Size(888, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // statusCount
            // 
            statusCount.Name = "statusCount";
            statusCount.Size = new Size(118, 17);
            statusCount.Text = "toolStripStatusLabel1";
            // 
            // FrmWin1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(888, 550);
            Controls.Add(statusStrip1);
            Controls.Add(btnScan);
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
            Margin = new Padding(2, 2, 2, 2);
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
    }
}