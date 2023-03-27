using System.Diagnostics;

namespace Sinex01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RunProcessAsync("..\\..\\..\\..\\bot1\\bot1\\bin\\Debug\\net7.0-windows\\bot1.exe");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            RunProcessAsync("..\\..\\..\\..\\bot2\\bin\\Debug\\net7.0-windows\\bot2.exe");
        }

        static Task<int> RunProcessAsync(string fileName)
        {
            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = { FileName = fileName },
                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.SetResult(process.ExitCode);
                process.Dispose();
            };

            process.Start();

            return tcs.Task;
        }
    }
}