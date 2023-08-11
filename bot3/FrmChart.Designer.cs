namespace bot3
{
    partial class FrmChart
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ucSinexChart1 = new UcSinexChart();
            SuspendLayout();
            // 
            // ucSinexChart1
            // 
            ucSinexChart1.Dock = DockStyle.Fill;
            ucSinexChart1.Location = new Point(0, 0);
            ucSinexChart1.Name = "ucSinexChart1";
            ucSinexChart1.Size = new Size(1956, 1196);
            ucSinexChart1.TabIndex = 0;
            // 
            // FrmChart
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1956, 1196);
            Controls.Add(ucSinexChart1);
            LookAndFeel.SkinName = "DevExpress Dark Style";
            LookAndFeel.UseDefaultLookAndFeel = false;
            Margin = new Padding(2);
            Name = "FrmChart";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private UcSinexChart ucSinexChart1;
    }
}