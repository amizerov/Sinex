using amLogger;
using CaDb;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace bot5;

class Data
{
    public static List<ProdEx> GetProdsWithExchanges(string seatch)
    {
        using (CaDbContext db = new())
        {
            var prods = db.Database
                .SqlQuery<ProdEx>($"Sinex_GetProductsExch {seatch}");

            return prods.ToList();
        }
    }

    public static List<Arbitrage> GetArbitrages(string filterExc, string filterMon, bool toSend = false)
    {
        string q = @$"
                declare @n int
                select @n=max(shotNumber) from Sinex_Arbitrage

                select * from Sinex_Arbitrage 
                where 
                    shotNumber = @n
                    and vol1 > 100
                    and vol2 > 100
                    and procDiffer > 1.5
                    and not exch1 in ({filterExc})
                    and not exch2 in ({filterExc})
                    and not baseAsset in ({filterMon})
                    {(toSend ? "and dtSentToBot is null" : "")}
                order by procDiffer desc";

        using (CaDbContext db = new())
        {
            var fs = FormattableStringFactory.Create(q);
            var prods = db.Database
                .SqlQuery<Arbitrage>(fs).ToList<Arbitrage>();
            return prods;
        }
    }
    public static void SetSentFlagForArbitrage(int id)
    {
        using (CaDbContext db = new())
        {
            db.Database
                .ExecuteSql(
                    @$"
                        update Sinex_Arbitrage 
                            set dtSentToBot=getdate() 
                        where id={id}
                    "
                 );
        }
    }
    public static List<long> GetCaTeleBotUsers()
    {
        using (CaDbContext db = new())
        {
            List<long> chatIds = db.Database
                .SqlQuery<long>(
                    $"select chatId from Sinex_CaTeleBotUsers"
                 ).ToList();
            
            return chatIds;
        }

    }
    public static async Task AddCaTeleBotUser(long chatId)
    {
        using (CaDbContext db = new())
        {
            await db.Database
                .ExecuteSqlAsync(
                    @$"
                        if not exists(
                            select * from 
                            Sinex_CaTeleBotUsers 
                            where chatId={chatId}
                        )
                            insert Sinex_CaTeleBotUsers(chatId) 
                            values({chatId})"
                 );
        }
    }
    public static async Task SaveFullStat(FullStat ss) {

        if (ss.exc1 == null || ss.exc2 == null) return;

        using (CaDbContext db = new())
        {
            try
            {
                await db.Database.ExecuteSqlAsync(@$"
                    declare @n int
                    select @n=max(shotNumber) from Sinex_Arbitrage

                    update Sinex_Arbitrage 
                        set procDiffer={ss.proc},
                            exch1={ss.exc1.Name}, 
                            exch2={ss.exc2.Name},
                            vol1={ss.vol1},
                            vol2={ss.vol2},
                            dtu=getdate()
                    where 
                        shotNumber=@n 
                        and baseAsset={ss.asset}
                        and quoteAsset='USDT'
                ");

                if (ss.proc < 3/2) return;
                string q1 = $"[{ss.exc1.ID}|{ss.exc2.ID}]";
                string q2 = $"[{ss.exc2.ID}|{ss.exc1.ID}]";
                string w = q1 + q2;
                foreach (PriceSt s1 in ss)
                {
                    foreach (PriceSt s2 in ss.Where(s => s.exchange != s1.exchange))
                    {
                        q1 = $"[{s1.exchange!.ID}|{s2.exchange!.ID}]";
                        q2 = $"[{s2.exchange!.ID}|{s1.exchange!.ID}]";
                        if (w.Contains(q1) || w.Contains(q2)) continue;
                        w += q1 + q2;

                        await db.Database.ExecuteSqlAsync(@$"
                            declare @n int
                            select @n=max(shotNumber) from Sinex_Arbitrage

                            insert Sinex_Arbitrage(
                                shotNumber, 
                                baseAsset, 
                                quoteAsset, 
                                procDiffer,
                                exch1, 
                                exch2, 
                                vol1, 
                                vol2
                            ) values(
                                @n, 
                                {s1.asset}, 
                                'USDT', 
                                {Math.Abs(
                                    100*(decimal)(s1.price - s2.price)!)
                                       / Math.Max((decimal)s1.price!, (decimal)s2.price!)},
                                {(s1.price > s2.price ? s1.exchange!.Name : s2.exchange!.Name)}, 
                                {(s1.price > s2.price ? s2.exchange!.Name : s1.exchange!.Name)}, 
                                {(s1.price > s2.price ? s1.volum : s2.volum)}, 
                                {(s1.price > s2.price ? s2.volum : s1.volum)} 
                            )"
                        );
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(
                    @$"Stat Save 
                        {ss.asset} {ss.proc} {ss.exc1.Name} {ss.exc2.Name} {ss.vol1} {ss.vol2}"
                    , e.Message);
            }
        }
    }
    public static async Task UpdateFullStat(FullStat ss)
    {

        if (ss.exc1 == null || ss.exc2 == null) return;

        using (CaDbContext db = new())
        {
            try
            {
                await db.Database.ExecuteSqlAsync(@$"
                    declare @n int
                    select @n=max(shotNumber) from Sinex_Arbitrage

                    update Sinex_Arbitrage 
                        set procDiffer={ss.proc},
                            exch1={ss.exc1.Name}, 
                            exch2={ss.exc2.Name},
                            vol1={ss.vol1},
                            vol2={ss.vol2},
                            dtu=getdate()
                    where 
                        shotNumber=@n 
                        and baseAsset={ss.asset}
                        and quoteAsset='USDT'
                ");
            }
            catch (Exception e)
            {
                Log.Error(
                    @$"Stat Update 
                        {ss.asset} {ss.proc} {ss.exc1.Name} {ss.exc2.Name} {ss.vol1} {ss.vol2}"
                    , e.Message);
            }
        }
    }
}

class ProdEx
{
    public string? symbol { get; set; }
    public string? exc { get; set; }
    public string? d { get; set; }
    public int c { get; set; }
    public DateTime dmin { get; set; }
    public DateTime dmax { get; set; }
}

class Arbitrage
{
    public int ID { get; set; }
    public int shotNumber { get; set; }
    public string? baseAsset { get; set; }
    public string? exchanges { get; set; }
    public string? exch1 { get; set; }
    public string? exch2 { get; set; }
    public double? procDiffer { get; set; }
    public double? vol1 { get; set; }
    public double? vol2 { get; set; }
    public DateTime? dtSentToBot { get; set; }
}