﻿using amLogger;
using CryptoExchange.Net.Interfaces;

namespace bot4;

public partial class FrmStakan : Form
{
    ISymbolOrderBook book;

    public FrmStakan(ISymbolOrderBook somebook)
    {
        InitializeComponent();
        book = somebook;
        Text = book.Id + " - " + book.Symbol + " - Order book";
    }

    private async void FrmOrders_Load(object sender, EventArgs e)
    {
        Toolss.Utils.LoadFormPosition(this);

        book.OnStatusChange += (oldState, newState) =>
            Log.Trace("book.OnStatusChange", $"State changed from {oldState} to {newState}");

        var startResult = await book.StartAsync();
        if (!startResult.Success)
        {
            Log.Error("BinanceSpotSymbolOrderBook", "Failed to start order book: " + startResult.Error);
        }

        while (true)
        {
            UpdateBook();
            await Task.Delay(500);
        }
    }

    void UpdateBook()
    {
        if (dgBook.Columns.Count != 3)
        {
            dgBook.Columns.Clear();
            dgBook.Columns.Add("Column1", "Buy");
            dgBook.Columns.Add("Column2", "Price");
            dgBook.Columns.Add("Column3", "Sell");
        }

        dgBook.Rows.Clear();

        UpdateAsks();
        UpdateBids();
    }

    void UpdateAsks()
    {
        if (book.AskCount == 0) return;

        List<ISymbolOrderBookEntry> ba = book.Asks.Take(15).ToList();
        double qMax = (double)ba.Max(a => a.Quantity);
        double qMin = (double)ba.Min(a => a.Quantity);
        double qq = 65 / (qMax - qMin);
        ba.Reverse();
        foreach (var a in ba)
        {
            int q = (int)(qq * ((double)a.Quantity - qMin));
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[0].Value = "";
            dgBook.Rows[n].Cells[1].Value = PriceFormated(a.Price);
            dgBook.Rows[n].Cells[2].Value = QuantFormated(a.Quantity);
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.FromArgb(240, 128, 128);
            dgBook.Rows[n].Cells[2].Style.BackColor = Color.FromArgb(255 - q, 128 - q, 128 - q);
        }
    }

    void UpdateBids()
    {
        if (book.BidCount == 0) return;

        List<ISymbolOrderBookEntry> bb = book.Bids.Take(15).ToList();
        double qMax = (double)bb.Max(b => b.Quantity);
        double qMin = (double)bb.Min(b => b.Quantity);
        double qq = 65 / (qMax - qMin);
        foreach (var b in bb)
        {
            int q = (int)(qq * ((double)b.Quantity - qMin));
            int n = dgBook.Rows.Add();
            dgBook.Rows[n].Cells[2].Value = "";
            dgBook.Rows[n].Cells[1].Value = PriceFormated(b.Price);
            dgBook.Rows[n].Cells[0].Value = QuantFormated(b.Quantity);
            dgBook.Rows[n].Cells[1].Style.BackColor = Color.FromArgb(128, 240, 128);
            dgBook.Rows[n].Cells[0].Style.BackColor = Color.FromArgb(128 - q, 240 - q, 128 - q);
        }
    }

    private async void FrmOrderBook_FormClosing(object sender, FormClosingEventArgs e)
    {
        await book.StopAsync();
        Toolss.Utils.SaveFormPosition(this);
    }
    string PriceFormated(decimal price)
    {
        string[] ar = price.ToString().Split(',');

        if (ar.Length == 1) return price.ToString() + ",00";

        ar[1] = ar[1].TrimEnd('0');
        if (ar[1].Length == 0) ar[1] = "00";
        if (ar[1].Length == 1) ar[1] += "0";

        return ar[0] + "," + ar[1];
    }
    string QuantFormated(decimal quant)
    {
        string[] ar = quant.ToString().Split(',');

        if (ar.Length == 1) return quant.ToString();

        ar[1] = ar[1].TrimEnd('0');
        if (ar[1].Length == 0) return ar[0];

        return ar[0] + "," + ar[1];
    }
}

