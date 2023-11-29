using amLogger;
using CaDb;
using CaExch;
using Microsoft.EntityFrameworkCore;

namespace bot5;

class ProdEx
{
    public string? symbol { get; set; }
    public string? exc { get; set; }
    public string? d { get; set; }
    public int c { get; set; }
    public DateTime dmin { get; set; }
    public DateTime dmax { get; set; }
}

class PriceSt
{
    public AnExchange? exchange { get; set; }
    public string? symbol { get; set; }
    public decimal? price { get; set; }
    public decimal? volum { get; set; }
}
class Stat : List<PriceSt>
{
    public string? asset { get; set; }
    public AnExchange? exc1 { get; set; }
    public AnExchange? exc2 { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal vol1 { get; set; }
    public decimal vol2 { get; set; }

    public static async Task<Stat> Init(
        string exchengesString, string baseAsset,
        Action<PriceSt>? onStep = null
    )
    {
        Stat st = new();
        st.asset = baseAsset;
        var aex = exchengesString.Split(',');
        int[] arrExchangeIds = new int[aex.Length];
        for (int i = 0; i < aex.Length; i++)
            arrExchangeIds[i] = Convert.ToInt32(aex[i]);

        CaExchanges _exs = new();
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
    static async Task<PriceSt> GetPriceVolum(AnExchange exc, string ass)
    {
        PriceSt pv = new() { exchange = exc, symbol = ass };
        string symbol = "";
        switch (exc.ID)
        {
            case 1:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 2:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 3:
                symbol = ass.ToLower() + "usdt";
                break;
            case 4:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 5:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 6:
                symbol = ass.ToLower() + "usdt";
                break;
            case 7:
                symbol = ass.ToUpper() + "USDT";
                break;
            case 8:
                symbol = ass.ToUpper() + "-USDT";
                break;
            case 9:
                symbol = ass.ToUpper() + "USDT";
                break;
        }
        var t = await exc.GetTickerAsync(symbol);
        if (t == null) return pv;

        //var k = await exc.GetKlines(symbol, "1m", 10);
        //Task.Delay(1000).Wait();

        pv.price = t.LastPrice;
        pv.volum = t.Volume;

        return pv;
    }
    void Calc()
    {
        foreach (var i in
            this.Where(e => 1 == 1
                         && e.exchange!.ID != 4
                         && e.exchange!.ID != 1))
        {
            foreach (var j in
                this.Where(e => e.exchange!.ID != i.exchange!.ID
                             && e.exchange!.ID != 4
                             && e.exchange!.ID != 1))
            {
                if (i.price == null || j.price == null) continue;

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
    public void Save()
    {
        if (exc1 == null || exc2 == null) return;
        using (CaDbContext db = new())
        {
            try
            {
                db.Database.ExecuteSql(
                    @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage
                        update Sinex_Arbitrage 
                            set procDiffer={proc},
                                exch1={exc1.Name}, 
                                exch2={exc2.Name},
                                vol1={vol1},
                                vol2={vol2}             
                        where 
                            shotNumber=@n 
                            and baseAsset={asset}
                            and quoteAsset='USDT'"
                );
            }
            catch (Exception e)
            {
                Log.Error(
                    $"Stat Save {asset} {proc} {exc1.Name} {exc2.Name} {vol1} {vol2}"
                    , e.Message);
            }
        }
    }
}
