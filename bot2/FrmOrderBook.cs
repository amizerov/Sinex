using amLogger;
using CryptoExchange.Net.Interfaces;
using System.Reflection;

namespace bot2;

public partial class FrmOrderBook : Form
{
    ISymbolOrderBook book;

    public FrmOrderBook(ISymbolOrderBook somebook)
    {
        InitializeComponent();
        book = somebook;
        Text = book.Id + " - " + book.Symbol + " - Order book";
    }

    private async void FrmOrders_Load(object sender, EventArgs e)
    {
        LoadFormPosition();

        book.OnStatusChange += (oldState, newState) =>
            Log.Trace("book.OnStatusChange", $"State changed from {oldState} to {newState}");

        var startResult = await book.StartAsync();
        if (!startResult.Success)
        {
            Log.Error("BinanceSpotSymbolOrderBook", "Failed to start order book: " + startResult.Error);
        }

        while (true)
        {
            SetBook(book);
            await Task.Delay(500);
        }
    }

    void SetBook(ISymbolOrderBook book)
    {
        if (dgBook.Columns.Count != 3)
        {
            dgBook.Columns.Clear();
            dgBook.Columns.Add("Column1", "Buy");
            dgBook.Columns.Add("Column2", "Price");
            dgBook.Columns.Add("Column3", "Sell");
        }
        int c = 0;
        List<ISymbolOrderBookEntry> ba = new();
        foreach(var a in book.Asks)
        {
            ba.Add(a); if(++c == 15) break;
        }
        
        dgBook.Rows.Clear(); ba.Reverse();
        foreach (var a in ba)
        {
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[0].Value = "";
            dgBook.Rows[n].Cells[1].Value = a.Price.ToString();
            dgBook.Rows[n].Cells[2].Value = a.Quantity.ToString();
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.Red;
            dgBook.Rows[n].Cells[2].Style.BackColor = Color.Red;
        }
        c = 0;
        List<ISymbolOrderBookEntry> bb = new();
        foreach (var b in book.Bids)
        {
            bb.Add(b); if (++c == 15) break;
        }
        foreach (var b in bb)
        {
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[0].Value = b.Quantity.ToString();
            dgBook.Rows[n].Cells[1].Value = b.Price.ToString();
            dgBook.Rows[n].Cells[2].Value = "";
            dgBook.Rows[n].Cells[0].Style.BackColor = Color.Green;
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.Green;
        }

    }

    #region Form position
    private async void FrmOrderBook_FormClosing(object sender, FormClosingEventArgs e)
    {
        await book.StopAsync();

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

