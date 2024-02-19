using amLogger;
using CaExch2;
using CryptoExchange.Net.CommonObjects;
using System.Runtime.CompilerServices;
using TelegramBot1;

namespace bot5;

class CoinExchStat
{
    public AnExchange? exchange { get; set; }
    public string? coin { get; set; }
    public decimal? price { get; set; }
    public decimal? volum { get; set; }
    public bool allowDeposit { get; set; }
    public bool allowWithraw { get; set; }
    public Double? withdrawaFee { get; set; }
    public List<CoinChain> coinChains { get; set; } = new();
    public CoinExchStat(AnExchange exc, string baseAsset)
    {
        exchange = exc; coin = baseAsset;
    }
    public async Task Init()
    {
        if(coin == null || exchange == null) return;
        string symbol = exchange.ValidateSymbol(coin, "USDT");
        var t = await exchange.GetTickerAsync(symbol);
        if (t == null) return;


        price = t.LastPrice;
        volum = t.Volume;

        //var ob = await exchange.GetOrderBook(symbol);
        //var pa = (decimal)ob.Asks.Min(a => a.Price);
        //var pb = (decimal)ob.Bids.Max(b => b.Price);
        //price = (pa + pb) / 2;
        //volum = ob.Asks.Sum(a => a.Quantity * a.Price) +
        //    ob.Bids.Sum(b => b.Quantity * b.Price);
    }
    public async Task LoadData()
    {
        allowDeposit = await Db.GetAllowDeposit(exchange!.ID, coin!);
        allowWithraw = await Db.GetAllowWithdra(exchange!.ID, coin!);
        withdrawaFee = await Db.GetWithdrawaFee(exchange!.ID, coin!);
        coinChains = await Db.GetCoinChains(exchange!.ID, coin!);
    }
}
class FullStat : List<CoinExchStat>
{
    TelegramBot1.Params pars = TelegramBot1.Db.LoadParams();
    public string? coin { get; set; }
    public AnExchange? excSell { get; set; }
    public AnExchange? excBuy { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal volSell { get; set; }
    public decimal volBuy { get; set; }

    public static async Task<FullStat> Calculate(
        string exchengesString, string baseAsset,
        Action<CoinExchStat>? onStep = null
    )
    {
        FullStat st = new();
        st.coin = baseAsset;
        var aex = exchengesString.Split(',');
        int[] arrExchangeIds = new int[aex.Length];
        for (int i = 0; i < aex.Length; i++)
            arrExchangeIds[i] = Convert.ToInt32(aex[i]);

        CaExchanges _exs = CaExchanges.List();
        foreach (var ex in arrExchangeIds)
        {
            AnExchange exch = _exs.Find(x => x.ID == ex)!;
            CoinExchStat coinStat = new(exch, baseAsset);
            await coinStat.Init();
            await coinStat.LoadData();
            st.Add(coinStat);
            onStep?.Invoke(coinStat);
        }
        st.Calc();
        try
        {
            await st.TryAddMeToBundles();
            await st.FindMoreBundles();
        }
        catch (Exception ex)
        {
            Log.Trace("TryAddMeToBundles", "Error: " + ex.Message);
        }
        return st;
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
                        $"{st.coin}({st.exchange!.ID}).price is null OR" + 
                        $"{j.coin}({j.exchange!.ID}).price is null");
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
    async Task TryAddMeToBundles()
    {
        if (coin == null || excSell == null || excBuy == null) return;
        Bandle b = new()
        {
            coin = coin,
            exchBuy = excBuy.Name,
            exchSell = excSell.Name
        };
        proc = Math.Round(proc, 2);
        if ((float)proc < pars.minProc)
        {
            Log.Trace("1", $"{coin}({excBuy.Name}|{excSell.Name}) proc({proc}) < minProc({pars.minProc})");
            await TelegramBot1.Db.CloseBandle(b);
            return;
        }

        bool allowDeposit = await Db.GetAllowDeposit(excSell.ID, coin);
        if (!allowDeposit)
        {
            Log.Trace("2", $"{coin}({excBuy.Name}|{excSell.Name}) allowDeposit({allowDeposit})");
            await TelegramBot1.Db.CloseBandle(b);
            return;
        }
        bool allowWithraw = await Db.GetAllowWithdra(excBuy.ID, coin);
        if (!allowWithraw)
        {
            Log.Trace("3", $"{coin}({excBuy.Name}|{excSell.Name}) allowWithraw({allowWithraw})");
            await TelegramBot1.Db.CloseBandle(b);
            return;
        }

        List<CoinChain> chainsBuy = await Db.GetCoinChains(excBuy.ID, coin!);
        List<CoinChain> chainsSell = await Db.GetCoinChains(excSell.ID, coin!);
        foreach (CoinChain chBuy in chainsBuy)
        {
            foreach (CoinChain chSell in chainsSell)
            {
                if (chBuy.chainId != chSell.chainId) continue;
                if (chBuy.chainId == null) continue;
                if (chBuy.chainId == 0) continue;

                Log.Trace("TryAddMeToBundles", 
                    $"{coin} {excBuy.Name}->({chSell.chainName})->{excSell.Name}");

                string symbolBuy = excBuy.ValidateSymbol(coin, "USDT");
                string symbolSell = excSell.ValidateSymbol(coin, "USDT");

                Ticker tBuy = await excBuy.GetTickerAsync(symbolBuy);
                Ticker tSell = await excSell.GetTickerAsync(symbolSell);
                if (tBuy.LastPrice == null || tSell.LastPrice == null)
                {
                    Log.Trace("4", "Ticker.LastPrice is null");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }
                decimal pb = (decimal)tBuy.LastPrice;
                decimal ps = (decimal)tSell.LastPrice;
                decimal proc = 100 * (ps - pb) / Math.Max(ps, pb);
                proc = Math.Round(proc, 2);

                if ((float)proc < pars.minProc)
                {
                    Log.Trace("5", $"{coin} proc({proc}) < minProc({pars.minProc})");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }

                CaOrderBook obBuy = await excBuy.GetOrderBook(symbolBuy);
                CaOrderBook obSell = await excSell.GetOrderBook(symbolSell);
                if (obBuy == null || obSell == null)
                {
                    Log.Trace("6", $"{coin} obBuy null or obSell null");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }
                Ticker tb = await excBuy.GetTickerAsync(symbolBuy);
                Ticker ts = await excSell.GetTickerAsync(symbolSell);
                var lastBuy = tb.LastPrice ?? 0;
                var lastSell = ts.LastPrice ?? 0;
                var lastVolBuy = tb.Volume ?? 0;
                var lastVolSell = ts.Volume ?? 0;
                if(lastBuy == 0 || lastSell == 0)
                {
                    Log.Trace("7", $"{coin} lastBuy 0 or lastSell 0");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }
                if (lastVolBuy == 0 || lastVolSell == 0)
                {
                    Log.Trace("8", $"{coin} lastVolBuy 0 or lastVolSell  0");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }

                volBuy = obBuy.Asks.Sum(a => a.Quantity*a.Price);
                volSell = obSell.Bids.Sum(b => b.Quantity*b.Price);
                volBuy = Math.Round(volBuy, 2);
                volSell = Math.Round(volSell, 2);

                if (volSell < (decimal)pars.minVolu || volBuy < (decimal)pars.minVolu)
                {
                    Log.Trace("9", $"{coin} volSell or volBuy < {pars.minVolu}");
                    await TelegramBot1.Db.CloseBandle(b);
                    continue;
                }

                Bandle band = new()
                {
                    coin = coin,
                    exchBuy = excBuy.Name,
                    exchSell = excSell.Name,
                    priceBuyBid = (float)obBuy.Bids.Max(b => b.Price),
                    priceBuyAsk = (float)obBuy.Asks.Min(b => b.Price),
                    priceSellBid = (float)obSell.Bids.Max(b => b.Price),
                    priceSellAsk = (float)obSell.Asks.Min(b => b.Price),
                    volBuy = (float)volBuy,
                    volSell = (float)volSell,
                    lastBuy = (float)lastBuy,
                    lastSell = (float)lastSell,
                    lastVolBuy = (float)lastVolBuy,
                    lastVolSell = (float)lastVolSell,
                    chain = chBuy.chainName!,
                    withdrawFee = (float)(chBuy.withdrawFee ?? 0),
                };
                await band.TryToPublish();
            }
        }

        double withdrawaFee = await Db.GetWithdrawaFee(excBuy.ID, coin) ?? 0;
    }
    async Task FindMoreBundles()
    {
        // Кроме максимального процента разницы цен
        // сохраняем все остальные варианты между биржами
        if (excBuy == null || excSell == null) return;
        string q1 = $"[{excSell!.ID}|{excBuy!.ID}]";
        string q2 = $"[{excBuy.ID}|{excSell.ID}]";
        string w = q1 + q2;// эти пары бирж уже сохранены
        foreach (CoinExchStat s1 in this)
        {
            foreach (CoinExchStat s2 in this.Where(s => s.exchange != s1.exchange))
            {
                q1 = $"[{s1.exchange!.ID}|{s2.exchange!.ID}]";
                q2 = $"[{s2.exchange!.ID}|{s1.exchange!.ID}]";
                // если эта пара бирж уже сохранена, пропускаем
                if (w.Contains(q1) || w.Contains(q2)) continue;
                w += q1 + q2;// добавляем пару в список сохраненных

                decimal p1 = s1.price ?? 0;
                decimal p2 = s2.price ?? 0;
                decimal v1 = s1.volum ?? 0;
                decimal v2 = s2.volum ?? 0;
                if(p1 == 0 || p2 == 0 || v1 == 0 || v2 == 0) continue;

                proc = Math.Abs(100 * (p1 - p2) / Math.Max(p1, p2));
                excBuy = p1 < p2 ? s1.exchange : s2.exchange;
                excSell = p1 < p2 ? s2.exchange : s1.exchange;
                volBuy = p1 < p2 ? v1 : v2;
                volSell = p1 < p2 ? v2 : v1;

                await TryAddMeToBundles();
            }
        }

    }
    public async Task Save()
    {
        await Db.SaveFullStat(this);
    }
    public async Task Update()
    {
        await Db.UpdateFullStat(this);
    }
}
