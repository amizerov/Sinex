using amLogger;
using CaExch2;

namespace bot5;

class PriceSt
{
    public AnExchange? exchange { get; set; }
    public string? asset { get; set; }
    public decimal? price { get; set; }
    public decimal? volum { get; set; }
}
class FullStat : List<PriceSt>
{
    public string? asset { get; set; }
    public AnExchange? excSell { get; set; }
    public AnExchange? excBuy { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal volSell { get; set; }
    public decimal volBuy { get; set; }

    public static async Task<FullStat> Calculate(
        string exchengesString, string baseAsset,
        Action<PriceSt>? onStep = null
    )
    {
        FullStat st = new();
        st.asset = baseAsset;
        var aex = exchengesString.Split(',');
        int[] arrExchangeIds = new int[aex.Length];
        for (int i = 0; i < aex.Length; i++)
            arrExchangeIds[i] = Convert.ToInt32(aex[i]);

        CaExchanges _exs = CaExchanges.List();
        foreach (var ex in arrExchangeIds)
        {
            AnExchange exch = _exs.Find(x => x.ID == ex)!;
            var p = await GetPriceVolum(exch, st.asset);
            st.Add(p);
            onStep?.Invoke(p);
        }
        st.Calc();
        return st;
    }
    static async Task<PriceSt> GetPriceVolum(AnExchange exc, string baseAsset)
    {
        PriceSt pv = new() { exchange = exc, asset = baseAsset };
        string symbol = exc.ValidateSymbol(baseAsset, "USDT");
        var t = await exc.GetTickerAsync(symbol);
        if (t == null) return pv;

        pv.price = t.LastPrice;
        pv.volum = t.Volume;

        return pv;
    }
    private void Calc()
    {
        foreach (var st in this)
        {
            foreach (var j in this.Where(s => s.exchange!.ID != st.exchange!.ID))
            {
                if (st.price == null || j.price == null)
                { 
                    Log.Warn("Calc", 
                        $"{st.asset}({st.exchange!.ID}).price is null OR" + 
                        $"{j.asset}({j.exchange!.ID}).price is null");
                    continue; 
                }

                var d = (decimal)(st.price - j.price)!;
                if (proc < Math.Abs(d) && st.volum > 0 && j.volum > 0)
                {
                    proc = d;
                    if (st.price > j.price)
                    {
                        excSell = st.exchange; volSell = (decimal)st.volum!;
                        excBuy = j.exchange; volBuy = (decimal)j.volum!;
                    }
                    else
                    {
                        excBuy = st.exchange; volBuy = (decimal)j.volum!;
                        excSell = j.exchange; volSell = (decimal)st.volum!;
                    }
                }
            }
        }
        proc = 100 * proc / (decimal)this.Max(s => s.price)!;
        proc = Math.Round(proc, 2);
    }
    public async Task Save()
    {
        await Data.SaveFullStat(this);
    }
    public async Task Update()
    {
        await Data.UpdateFullStat(this);
    }
}
