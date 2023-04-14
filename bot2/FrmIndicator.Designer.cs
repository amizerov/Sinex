namespace bot2
{
    partial class FrmIndicator
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button3 = new Button();
            btnSave = new Button();
            chLbIndSma = new CheckedListBox();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            colorDialog1 = new ColorDialog();
            panel1 = new Panel();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
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
            tabControl1.Size = new Size(734, 591);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panel1);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(btnSave);
            tabPage1.Controls.Add(chLbIndSma);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(726, 553);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Moving Average";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button3.Location = new Point(462, 505);
            button3.Name = "button3";
            button3.Size = new Size(112, 34);
            button3.TabIndex = 5;
            button3.Text = "Reset";
            button3.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.Location = new Point(580, 505);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // chLbIndSma
            // 
            chLbIndSma.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            chLbIndSma.FormattingEnabled = true;
            chLbIndSma.Items.AddRange(new object[] { "ALMA", "DEMA", "EPMA", "EMA", "HMA", "KAMA", "LSMA", "MAMA", "McGinley Dynamic", "MMA", "RMA", "SMA", "SMMA", "Tillson T3 MA", "TEMA", "VWAP", "VWMA", "WMA" });
            chLbIndSma.Location = new Point(3, 3);
            chLbIndSma.Name = "chLbIndSma";
            chLbIndSma.Size = new Size(223, 536);
            chLbIndSma.TabIndex = 0;
            chLbIndSma.SelectedIndexChanged += chLbIndSma_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(771, 593);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Price channels";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(771, 593);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Oscillators";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(771, 593);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Price trends";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Location = new Point(243, 22);
            panel1.Name = "panel1";
            panel1.Size = new Size(463, 461);
            panel1.TabIndex = 6;
            // 
            // FrmIndicator
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(734, 591);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmIndicator";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Indicators";
            Load += FrmIndicator_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private CheckedListBox chLbIndSma;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private Button button3;
        private Button btnSave;
        private ColorDialog colorDialog1;
        private Panel panel1;
    }
}