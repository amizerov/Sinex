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
            button7 = new Button();
            button4 = new Button();
            button6 = new Button();
            button2 = new Button();
            button5 = new Button();
            button1 = new Button();
            txtLookbackPeriods6 = new TextBox();
            txtLookbackPeriods3 = new TextBox();
            chbMa6 = new CheckBox();
            chbMa3 = new CheckBox();
            txtLookbackPeriods5 = new TextBox();
            chbMa5 = new CheckBox();
            txtLookbackPeriods2 = new TextBox();
            txtLookbackPeriods4 = new TextBox();
            chbMa2 = new CheckBox();
            chbMa4 = new CheckBox();
            txtLookbackPeriods1 = new TextBox();
            chbMa1 = new CheckBox();
            label1 = new Label();
            chLbIndSma = new CheckedListBox();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            tabPage4 = new TabPage();
            colorDialog1 = new ColorDialog();
            panelSma = new Panel();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelSma.SuspendLayout();
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
            tabControl1.Size = new Size(578, 467);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panelSma);
            tabPage1.Controls.Add(button3);
            tabPage1.Controls.Add(btnSave);
            tabPage1.Controls.Add(chLbIndSma);
            tabPage1.Location = new Point(4, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(570, 429);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Moving Average";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(309, 381);
            button3.Name = "button3";
            button3.Size = new Size(112, 34);
            button3.TabIndex = 5;
            button3.Text = "Reset";
            button3.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(427, 381);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(112, 34);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // button7
            // 
            button7.Location = new Point(218, 280);
            button7.Name = "button7";
            button7.Size = new Size(89, 34);
            button7.TabIndex = 4;
            button7.Text = "Color";
            button7.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(218, 158);
            button4.Name = "button4";
            button4.Size = new Size(89, 34);
            button4.TabIndex = 4;
            button4.Text = "Color";
            button4.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(218, 238);
            button6.Name = "button6";
            button6.Size = new Size(89, 34);
            button6.TabIndex = 4;
            button6.Text = "Color";
            button6.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(218, 116);
            button2.Name = "button2";
            button2.Size = new Size(89, 34);
            button2.TabIndex = 4;
            button2.Text = "Color";
            button2.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(218, 198);
            button5.Name = "button5";
            button5.Size = new Size(89, 34);
            button5.TabIndex = 4;
            button5.Text = "Color";
            button5.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(218, 76);
            button1.Name = "button1";
            button1.Size = new Size(89, 34);
            button1.TabIndex = 4;
            button1.Text = "Color";
            button1.UseVisualStyleBackColor = true;
            // 
            // txtLookbackPeriods6
            // 
            txtLookbackPeriods6.BorderStyle = BorderStyle.None;
            txtLookbackPeriods6.Location = new Point(124, 285);
            txtLookbackPeriods6.Name = "txtLookbackPeriods6";
            txtLookbackPeriods6.Size = new Size(66, 24);
            txtLookbackPeriods6.TabIndex = 3;
            txtLookbackPeriods6.Text = "0";
            // 
            // txtLookbackPeriods3
            // 
            txtLookbackPeriods3.BorderStyle = BorderStyle.None;
            txtLookbackPeriods3.Location = new Point(124, 163);
            txtLookbackPeriods3.Name = "txtLookbackPeriods3";
            txtLookbackPeriods3.Size = new Size(66, 24);
            txtLookbackPeriods3.TabIndex = 3;
            txtLookbackPeriods3.Text = "99";
            // 
            // chbMa6
            // 
            chbMa6.AutoSize = true;
            chbMa6.Location = new Point(33, 283);
            chbMa6.Name = "chbMa6";
            chbMa6.Size = new Size(76, 29);
            chbMa6.TabIndex = 2;
            chbMa6.Text = "MA6";
            chbMa6.UseVisualStyleBackColor = true;
            chbMa6.CheckedChanged += chbMa_CheckedChanged;
            // 
            // chbMa3
            // 
            chbMa3.AutoSize = true;
            chbMa3.Location = new Point(33, 161);
            chbMa3.Name = "chbMa3";
            chbMa3.Size = new Size(76, 29);
            chbMa3.TabIndex = 2;
            chbMa3.Text = "MA3";
            chbMa3.UseVisualStyleBackColor = true;
            chbMa3.CheckedChanged += chbMa_CheckedChanged;
            // 
            // txtLookbackPeriods5
            // 
            txtLookbackPeriods5.BorderStyle = BorderStyle.None;
            txtLookbackPeriods5.Location = new Point(124, 243);
            txtLookbackPeriods5.Name = "txtLookbackPeriods5";
            txtLookbackPeriods5.Size = new Size(66, 24);
            txtLookbackPeriods5.TabIndex = 3;
            txtLookbackPeriods5.Text = "0";
            // 
            // chbMa5
            // 
            chbMa5.AutoSize = true;
            chbMa5.Location = new Point(33, 241);
            chbMa5.Name = "chbMa5";
            chbMa5.Size = new Size(76, 29);
            chbMa5.TabIndex = 2;
            chbMa5.Text = "MA5";
            chbMa5.UseVisualStyleBackColor = true;
            chbMa5.CheckedChanged += chbMa_CheckedChanged;
            // 
            // txtLookbackPeriods2
            // 
            txtLookbackPeriods2.BorderStyle = BorderStyle.None;
            txtLookbackPeriods2.Location = new Point(124, 121);
            txtLookbackPeriods2.Name = "txtLookbackPeriods2";
            txtLookbackPeriods2.Size = new Size(66, 24);
            txtLookbackPeriods2.TabIndex = 3;
            txtLookbackPeriods2.Text = "25";
            // 
            // txtLookbackPeriods4
            // 
            txtLookbackPeriods4.BorderStyle = BorderStyle.None;
            txtLookbackPeriods4.Location = new Point(124, 203);
            txtLookbackPeriods4.Name = "txtLookbackPeriods4";
            txtLookbackPeriods4.Size = new Size(66, 24);
            txtLookbackPeriods4.TabIndex = 3;
            txtLookbackPeriods4.Text = "0";
            // 
            // chbMa2
            // 
            chbMa2.AutoSize = true;
            chbMa2.Location = new Point(33, 119);
            chbMa2.Name = "chbMa2";
            chbMa2.Size = new Size(76, 29);
            chbMa2.TabIndex = 2;
            chbMa2.Text = "MA2";
            chbMa2.UseVisualStyleBackColor = true;
            chbMa2.CheckedChanged += chbMa_CheckedChanged;
            // 
            // chbMa4
            // 
            chbMa4.AutoSize = true;
            chbMa4.Location = new Point(33, 201);
            chbMa4.Name = "chbMa4";
            chbMa4.Size = new Size(76, 29);
            chbMa4.TabIndex = 2;
            chbMa4.Text = "MA4";
            chbMa4.UseVisualStyleBackColor = true;
            chbMa4.CheckedChanged += chbMa_CheckedChanged;
            // 
            // txtLookbackPeriods1
            // 
            txtLookbackPeriods1.BorderStyle = BorderStyle.None;
            txtLookbackPeriods1.Location = new Point(124, 81);
            txtLookbackPeriods1.Name = "txtLookbackPeriods1";
            txtLookbackPeriods1.Size = new Size(66, 24);
            txtLookbackPeriods1.TabIndex = 3;
            txtLookbackPeriods1.Text = "7";
            // 
            // chbMa1
            // 
            chbMa1.AutoSize = true;
            chbMa1.Location = new Point(33, 79);
            chbMa1.Name = "chbMa1";
            chbMa1.Size = new Size(76, 29);
            chbMa1.TabIndex = 2;
            chbMa1.Text = "MA1";
            chbMa1.UseVisualStyleBackColor = true;
            chbMa1.CheckedChanged += chbMa_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 24);
            label1.Name = "label1";
            label1.Size = new Size(257, 25);
            label1.TabIndex = 1;
            label1.Text = "SMA - Simple Moving Average";
            // 
            // chLbIndSma
            // 
            chLbIndSma.FormattingEnabled = true;
            chLbIndSma.Items.AddRange(new object[] { "ALMA", "DEMA", "EPMA", "EMA", "HMA", "KAMA", "LSMA", "MAMA", "McGinley Dynamic", "MMA", "RMA", "SMA", "SMMA", "Tillson T3 MA", "TEMA", "VWAP", "VWMA", "WMA" });
            chLbIndSma.Location = new Point(3, 3);
            chLbIndSma.Name = "chLbIndSma";
            chLbIndSma.Size = new Size(223, 424);
            chLbIndSma.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(588, 435);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Price channels";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 34);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(588, 435);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Oscillators";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Location = new Point(4, 34);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(588, 435);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Price trends";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // panelSma
            // 
            panelSma.Controls.Add(chbMa1);
            panelSma.Controls.Add(label1);
            panelSma.Controls.Add(txtLookbackPeriods1);
            panelSma.Controls.Add(button7);
            panelSma.Controls.Add(chbMa4);
            panelSma.Controls.Add(button4);
            panelSma.Controls.Add(chbMa2);
            panelSma.Controls.Add(button6);
            panelSma.Controls.Add(txtLookbackPeriods4);
            panelSma.Controls.Add(button2);
            panelSma.Controls.Add(txtLookbackPeriods2);
            panelSma.Controls.Add(button5);
            panelSma.Controls.Add(chbMa5);
            panelSma.Controls.Add(button1);
            panelSma.Controls.Add(txtLookbackPeriods5);
            panelSma.Controls.Add(txtLookbackPeriods6);
            panelSma.Controls.Add(chbMa3);
            panelSma.Controls.Add(txtLookbackPeriods3);
            panelSma.Controls.Add(chbMa6);
            panelSma.Location = new Point(232, 9);
            panelSma.Name = "panelSma";
            panelSma.Size = new Size(329, 346);
            panelSma.TabIndex = 6;
            // 
            // FrmIndicator
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 467);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "FrmIndicator";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Indicators";
            Load += FrmIndicator_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panelSma.ResumeLayout(false);
            panelSma.PerformLayout();
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
        private Button button1;
        private TextBox txtLookbackPeriods1;
        private CheckBox chbMa1;
        private Label label1;
        private ColorDialog colorDialog1;
        private Button button7;
        private Button button4;
        private Button button6;
        private Button button2;
        private Button button5;
        private TextBox txtLookbackPeriods6;
        private TextBox txtLookbackPeriods3;
        private CheckBox chbMa6;
        private CheckBox chbMa3;
        private TextBox txtLookbackPeriods5;
        private CheckBox chbMa5;
        private TextBox txtLookbackPeriods2;
        private TextBox txtLookbackPeriods4;
        private CheckBox chbMa2;
        private CheckBox chbMa4;
        private Panel panelSma;
    }
}