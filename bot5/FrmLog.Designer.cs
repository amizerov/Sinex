namespace bot5
{
    partial class FrmLog
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
            txtAll = new TextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            txtTrace = new TextBox();
            txtInfo = new TextBox();
            txtError = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // txtAll
            // 
            txtAll.Dock = DockStyle.Fill;
            txtAll.Location = new Point(3, 3);
            txtAll.Multiline = true;
            txtAll.Name = "txtAll";
            txtAll.Size = new Size(786, 401);
            txtAll.TabIndex = 0;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(txtAll);
            tabPage1.Location = new Point(4, 39);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(792, 407);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "All";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(txtTrace);
            tabPage2.Location = new Point(4, 39);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(792, 407);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Trace";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(txtInfo);
            tabPage3.Location = new Point(4, 39);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(792, 407);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Info";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(txtError);
            tabPage4.Location = new Point(4, 39);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(792, 407);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Error";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtTrace
            // 
            txtTrace.Dock = DockStyle.Fill;
            txtTrace.Location = new Point(3, 3);
            txtTrace.Multiline = true;
            txtTrace.Name = "txtTrace";
            txtTrace.Size = new Size(786, 401);
            txtTrace.TabIndex = 0;
            // 
            // txtInfo
            // 
            txtInfo.Dock = DockStyle.Fill;
            txtInfo.Location = new Point(0, 0);
            txtInfo.Multiline = true;
            txtInfo.Name = "txtInfo";
            txtInfo.Size = new Size(792, 407);
            txtInfo.TabIndex = 1;
            // 
            // txtError
            // 
            txtError.Dock = DockStyle.Fill;
            txtError.Location = new Point(0, 0);
            txtError.Multiline = true;
            txtError.Name = "txtError";
            txtError.Size = new Size(792, 407);
            txtError.TabIndex = 1;
            // 
            // FrmLog
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "FrmLog";
            Text = "FrmLog";
            Load += FrmLog_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtAll;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtTrace;
        private TabPage tabPage3;
        private TextBox txtInfo;
        private TabPage tabPage4;
        private TextBox txtError;
    }
}