using amLogger;
using Binance.Net.SymbolOrderBooks;
using System.Linq;
using System.Reflection;

namespace bot2;

public partial class FrmOrderBook : Form
{
    public FrmOrderBook()
    {
        InitializeComponent();
    }

    private async void FrmOrders_Load(object sender, EventArgs e)
    {
        LoadFormPosition();

        var book = new BinanceSpotSymbolOrderBook("BTCUSDT");
        book.OnStatusChange += (oldState, newState) => Log.Trace("book.OnStatusChange", $"State changed from {oldState} to {newState}");
        var startResult = await book.StartAsync();
        if (!startResult.Success)
        {
            Console.WriteLine("Failed to start order book: " + startResult.Error);
            return;
        }

        while (true)
        {
            string asks = "";
            foreach (var a in book.Asks.Reverse().Skip(book.Asks.Count() - 10))
            {
                asks += a.Price + " | " + a.Quantity + "\r\n";
            }
            string bids = "";
            foreach (var b in book.Bids.Reverse().Skip(book.Bids.Count() - 10).Reverse())
            {
                bids += b.Price + " | " + b.Quantity + "\r\n";
            }
            textBox1.Text = asks + "-----------\r\n" + bids;
            await Task.Delay(5000);
        }
    }

    #region Form position
    private void FrmOrderBook_FormClosing(object sender, FormClosingEventArgs e)
    {
        string pos = Top + ";" + Left + ";" + Width + ";" + Height;
        File.WriteAllText(FileFormPosition, pos);
    }
    void LoadFormPosition()
    {
        if (File.Exists(FileFormPosition))
        {
            string[] pos = File.ReadAllText(FileFormPosition).Split(';');
            Top = int.Parse(pos[0]);
            Left = int.Parse(pos[1]);
            Width = int.Parse(pos[2]);
            Height = int.Parse(pos[3]); ;
        }
    }
    string FileFormPosition =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\FrmOrderBookPosition.txt";
    #endregion
}
