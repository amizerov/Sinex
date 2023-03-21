using System;
using System.Collections.Generic;
public class Product
{
    public int Id { get; set; }
    public string symbol { get; set; }
    public int exchange { get; set; }
    public string baseasset { get; set; }
    public string quoteasset { get; set; }
    public double volatility { get; set; }
    public double liquidity { get; set; }
    public int cnt1 { get; set; }
    public int cnt2 { get; set; }
    public int cnt3 { get; set; }
    public int? Version { get; set; } = 1;
    public DateTime? dtFrom { get; set; }
    public DateTime? dtTo { get; set; }
    public string? KlineInterval { get; set; }
    public int? KlinesCount { get; set; }
    public Product() { symbol = baseasset = quoteasset = ""; }

    public bool IsTradingEnabled;

}
