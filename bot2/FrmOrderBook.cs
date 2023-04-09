using amLogger;
using bot2.Tools;
using CryptoExchange.Net.Interfaces;
using System.Drawing;
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
        Utils.LoadFormPosition(this);

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
        foreach (var a in book.Asks)
        {
            ba.Add(a); if (++c == 15) break;
        }
        if (ba.Count == 0) return;
        dgBook.Rows.Clear(); ba.Reverse();
        double qMax = (double)ba.Max(a => a.Quantity);
        double qMin = (double)ba.Min(a => a.Quantity);
        double qq = 55 / (qMax - qMin);
        foreach (var a in ba)
        {
            int q = (int)(qq * ((double)a.Quantity - qMin));
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[0].Value = "";
            dgBook.Rows[n].Cells[1].Value = a.Price.ToString();
            dgBook.Rows[n].Cells[2].Value = a.Quantity.ToString();
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.FromArgb(240, 128, 128);
            dgBook.Rows[n].Cells[2].Style.BackColor = Color.FromArgb(255 - q, 128 - q, 128 - q);
        }
        c = 0;
        List<ISymbolOrderBookEntry> bb = new();
        foreach (var b in book.Bids)
        {
            bb.Add(b); if (++c == 15) break;
        }
        qMax = (double)bb.Max(a => a.Quantity);
        qMin = (double)bb.Min(a => a.Quantity);
        qq = 55 / (qMax - qMin);
        foreach (var b in bb)
        {
            int q = (int)(qq * ((double)b.Quantity - qMin));
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[0].Value = b.Quantity.ToString();
            dgBook.Rows[n].Cells[1].Value = b.Price.ToString();
            dgBook.Rows[n].Cells[2].Value = "";
            dgBook.Rows[n].Cells[0].Style.BackColor = Color.FromArgb(128 - q, 240 - q, 128 - q);
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.FromArgb(128, 240, 128);
        }

    }

    private async void FrmOrderBook_FormClosing(object sender, FormClosingEventArgs e)
    {
        await book.StopAsync();
        Utils.SaveFormPosition(this);
    }
}

