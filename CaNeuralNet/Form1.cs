using static Tensorflow.Binding;
namespace CaNeuralNet;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        var hello = tf.constant("Hello TensorFlow!");
        label1.Text = hello.ToString();
    }
}