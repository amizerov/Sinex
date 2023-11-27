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
            ((System.ComponentModel.ISupportInitialize)dgvProds).BeginInit();
            SuspendLayout();
            // 
            // dgvProds
            // 
            dgvProds.AllowUserToAddRows = false;
            dgvProds.AllowUserToDeleteRows = false;
            dgvProds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProds.Location = new Point(12, 90);
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(420, 833);
            dgvProds.TabIndex = 0;
            dgvProds.SelectionChanged += dgvProds_SelectionChanged;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(26, 34);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(175, 35);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel
            // 
            panel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel.BackColor = SystemColors.ControlLight;
            panel.Location = new Point(470, 235);
            panel.Name = "panel";
            panel.Size = new Size(604, 673);
            panel.TabIndex = 2;
            // 
            // lblSym
            // 
            lblSym.AutoSize = true;
            lblSym.Location = new Point(465, 52);
            lblSym.Name = "lblSym";
            lblSym.Size = new Size(68, 30);
            lblSym.TabIndex = 3;
            lblSym.Text = "label1";
            // 
            // lblExc1
            // 
            lblExc1.AutoSize = true;
            lblExc1.Location = new Point(465, 141);
            lblExc1.Name = "lblExc1";
            lblExc1.Size = new Size(68, 30);
            lblExc1.TabIndex = 3;
            lblExc1.Text = "label1";
            // 
            // lblExc2
            // 
            lblExc2.AutoSize = true;
            lblExc2.Location = new Point(577, 141);
            lblExc2.Name = "lblExc2";
            lblExc2.Size = new Size(68, 30);
            lblExc2.TabIndex = 3;
            lblExc2.Text = "label1";
            // 
            // lblMaxProc
            // 
            lblMaxProc.AutoSize = true;
            lblMaxProc.Location = new Point(713, 141);
            lblMaxProc.Name = "lblMaxProc";
            lblMaxProc.Size = new Size(68, 30);
            lblMaxProc.TabIndex = 3;
            lblMaxProc.Text = "label1";
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnReload.Location = new Point(943, 46);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(131, 40);
            btnReload.TabIndex = 4;
            btnReload.Text = "button1";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // btnArbit
            // 
            btnArbit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnArbit.Location = new Point(943, 111);
            btnArbit.Name = "btnArbit";
            btnArbit.Size = new Size(131, 40);
            btnArbit.TabIndex = 4;
            btnArbit.Text = "button1";
            btnArbit.UseVisualStyleBackColor = true;
            btnArbit.Click += btnArbit_Click;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            button3.Location = new Point(943, 173);
            button3.Name = "button3";
            button3.Size = new Size(131, 40);
            button3.TabIndex = 4;
            button3.Text = "button1";
            button3.UseVisualStyleBackColor = true;
            // 
            // FrmWin1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1095, 935);
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
    }
}