namespace bot1
{
    partial class Form1
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
            button1 = new Button();
            textBox1 = new TextBox();
            button2 = new Button();
            btnStart = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(1043, 12);
            button1.Name = "button1";
            button1.Size = new Size(186, 34);
            button1.TabIndex = 0;
            button1.Text = "Проверка базы";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(12, 61);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(1217, 648);
            textBox1.TabIndex = 1;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top;
            button2.Location = new Point(848, 12);
            button2.Name = "button2";
            button2.Size = new Size(189, 34);
            button2.TabIndex = 2;
            button2.Text = "Количество бирж";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(38, 13);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(443, 34);
            btnStart.TabIndex = 3;
            btnStart.Text = "Начать обновление продуктов всез бирж";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += button3_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1241, 721);
            Controls.Add(btnStart);
            Controls.Add(button2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Button button2;
        private Button btnStart;
    }
}