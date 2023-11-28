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
            dgvProds.Location = new Point(12, 95);
            dgvProds.MultiSelect = false;
            dgvProds.Name = "dgvProds";
            dgvProds.ReadOnly = true;
            dgvProds.RowHeadersVisible = false;
            dgvProds.RowHeadersWidth = 72;
            dgvProds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProds.Size = new Size(735, 909);
            dgvProds.TabIndex = 0;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(613, 29);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(131, 40);
            btnUpdate.TabIndex = 1;
            btnUpdate.Text = "button1";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // FrmWin2
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1273, 1016);
            Controls.Add(btnUpdate);
            Controls.Add(dgvProds);
            Name = "FrmWin2";
            Text = "FrmWin2";
            Load += FrmWin2_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProds).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgvProds;
        private Button btnUpdate;
    }
}