using amLogger;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using System.Reflection;

namespace bot2;

public partial class FrmOrderBook : Form
{
    SymbolOrderBook book;
    public FrmOrderBook(SymbolOrderBook somebook)
    {
        InitializeComponent();
        book = somebook;
        Text = book.Id + " - " + book.Symbol + " - Order book";
    }

    private async void FrmOrders_Load(object sender, EventArgs e)
    {
        LoadFormPosition();

        book.OnOrderBookUpdate += Book_OnOrderBookUpdate;
        book.OnStatusChange += (oldState, newState) =>
            Log.Trace("book.OnStatusChange", $"State changed from {oldState} to {newState}");

        var startResult = await book.StartAsync();
        if (!startResult.Success)
        {
            Log.Error("BinanceSpotSymbolOrderBook", "Failed to start order book: " + startResult.Error);
        }
    }

    private void Book_OnOrderBookUpdate((IEnumerable<ISymbolOrderBookEntry> Bids, IEnumerable<ISymbolOrderBookEntry> Asks) obj)
    {
        if (Tag?.ToString() == "111") return;

        string asks = "";
        foreach (var a in obj.Asks.Reverse().Skip(obj.Asks.Count() - 15))
        {
            asks += a.Price + " | " + a.Quantity + "\r\n";
        }
        string bids = "";
        foreach (var b in obj.Bids.Reverse().Skip(obj.Bids.Count() - 15).Reverse())
        {
            bids += b.Price + " | " + b.Quantity + "\r\n";
        }

        try
        {
            Invoke(new Action(() =>
                textBox1.Text = asks + "-----------\r\n" + bids
            ));
        }catch (Exception ex) { Log.Error("Book_OnOrderBookUpdate", ex.Message); }
    }

    #region Form position
    private void FrmOrderBook_FormClosing(object sender, FormClosingEventArgs e)
    {
        Tag = "111";
        book.StopAsync();

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
