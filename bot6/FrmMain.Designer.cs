namespace bot6
{
    partial class FrmMain
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
            btnStart = new Button();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Location = new Point(406, 122);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(131, 40);
            btnStart.TabIndex = 0;
            btnStart.Text = "button1";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStart);
            Name = "FrmMain";
            Text = "Загрузка деталей коинов для криптобирж";
            ResumeLayout(false);
        }

        #endregion

        private Button btnStart;
    }
}
