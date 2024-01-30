using amLogger;
using CaExch2;
using CryptoExchange.Net.CommonObjects;

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
        string symbol = exchange!.ValidateSymbol(coin!, "USDT");
        var t = await exchange.GetTickerAsync(symbol);
        if (t == null) return;

        price = t.LastPrice;
        volum = t.Volume;
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
    const float minProc = 1;
    public string? coin { get; set; }
    public AnExchange? excSell { get; set; }
    public AnExchange? excBuy { get; set; }
    public decimal proc { get; set; } = 0;
    public decimal volSell { get; set; }
    public decimal volBuy { get; set; }
    public List<Bandle> Bandles { get; set; } = new();

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
        await st.TryAddMeToBundles();

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
        if ((float)proc < minProc) return;

        bool allowDeposit = await Db.GetAllowDeposit(excSell.ID, coin!);
        if (!allowDeposit) return;

        bool allowWithraw = await Db.GetAllowWithdra(excBuy!.ID, coin!);
        if (!allowWithraw) return;

        List<CoinChain> chainsBuy = await Db.GetCoinChains(excBuy.ID, coin!);
        List<CoinChain> chainsSell = await Db.GetCoinChains(excSell.ID, coin!);
        foreach (CoinChain chBuy in chainsBuy)
        {
            foreach (CoinChain chSell in chainsSell)
            {
                if (chBuy.chainId != chSell.chainId) continue;
                if (chBuy.chainId == null) continue;
                if (chBuy.chainId == 0) continue;

                Ticker tBuy = await excBuy.GetTickerAsync(coin);
                Ticker tSell = await excSell.GetTickerAsync(coin);
                float 

                if(tBuy.LastPrice - tSell.LastPrice)

                Bandle b = new()
                {
                    coin = coin!,
                    exchBuy = excBuy.Name,
                    exchSell = excSell.Name,
                    priceBuyBid = t.,
                    priceBuyAsk = (float)excBuy!.GetAskPrice(coin!),
                    priceSellBid = (float)excSell!.GetBidPrice(coin!),
                    priceSellAsk = (float)excSell!.GetAskPrice(coin!),
                    volBuy = (float)volBuy,
                    volSell = (float)volSell,
                    chain = chBuy.chainName!,
                    withdrawFee = (float)chBuy.withdrawFee!,
                };
                Bandles.Add(b);
            }
        }

        double? withdrawaFee = await Db.GetWithdrawaFee(excBuy!.ID, coin!);
    }
    void FindMoreBundles()
    {


        // Кроме максимального процента разницы цен
        // сохраняем все остальные варианты между биржами
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

                decimal proc = Math.Abs(
                    100 * (decimal)(s1.price - s2.price)!
                        / Math.Max((decimal)s1.price!, (decimal)s2.price!));
                string excBuy = s1.price < s2.price ? s1.exchange!.Name : s2.exchange!.Name;
                string excSell = s1.price < s2.price ? s2.exchange!.Name : s1.exchange!.Name;
                decimal? volBuy = s1.price < s2.price ? s1.volum : s2.volum;
                decimal? volSell = s1.price < s2.price ? s2.volum : s1.volum;


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
