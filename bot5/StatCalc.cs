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
    public AnExchange? exc1 { get; set; }
    public AnExchange? exc2 { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal vol1 { get; set; }
    public decimal vol2 { get; set; }

    public static async Task<FullStat> Init(
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

        //var k = await exc.GetKlines(symbol, "1m", 10);
        //Task.Delay(1000).Wait();

        pv.price = t.LastPrice;
        pv.volum = t.Volume;

        return pv;
    }
    private void Calc()
    {
        foreach (var i in
            this.Where(e => 1 == 1
                         //&& e.exchange!.ID != 3
                         //&& e.exchange!.ID != 4
                         //&& e.exchange!.ID != 1
                      ))
        {
            foreach (var j in
                this.Where(e => e.exchange!.ID != i.exchange!.ID
                             //&& e.exchange!.ID != 3
                             //&& e.exchange!.ID != 4
                             //&& e.exchange!.ID != 1
                          ))
            {
                if (i.price == null || j.price == null)
                { 
                    Log.Warn("Calc", 
                        @$"{i.asset}({i.exchange!.ID}).price == null 
                        || {j.asset}({j.exchange!.ID}).price == null");
                    continue; 
                }

                var d = (decimal)(i.price - j.price)!;
                if (proc < Math.Abs(d) && i.volum > 0 && j.volum > 0)
                {
                    proc = d;
                    if (i.price > j.price)
                    {
                        exc1 = i.exchange; vol1 = (decimal)i.volum!;
                        exc2 = j.exchange; vol2 = (decimal)j.volum!;
                    }
                    else
                    {
                        exc2 = i.exchange; vol2 = (decimal)j.volum!;
                        exc1 = j.exchange; vol1 = (decimal)i.volum!;
                    }
                }
            }
        }
        proc = 100 * proc / (decimal)this.Max(a => a.price)!;
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
