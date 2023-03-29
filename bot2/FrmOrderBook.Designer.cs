namespace bot2
{
    partial class FrmOrderBook
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dgBook = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgBook).BeginInit();
            SuspendLayout();
            // 
            // dgBook
            // 
            dgBook.AllowUserToAddRows = false;
            dgBook.AllowUserToDeleteRows = false;
            dgBook.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgBook.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgBook.Location = new Point(-63, -2);
            dgBook.MultiSelect = false;
            dgBook.Name = "dgBook";
            dgBook.ReadOnly = true;
            dgBook.RowHeadersWidth = 62;
            dgBook.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgBook.RowTemplate.Height = 33;
            dgBook.ScrollBars = ScrollBars.None;
            dgBook.Size = new Size(522, 982);
            dgBook.TabIndex = 0;
            // 
            // FrmOrderBook
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(448, 979);
            Controls.Add(dgBook);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MaximizeBox = false;
            Name = "FrmOrderBook";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "Order book";
            FormClosing += FrmOrderBook_FormClosing;
            Load += FrmOrders_Load;
            ((System.ComponentModel.ISupportInitialize)dgBook).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dgBook;
    }
}