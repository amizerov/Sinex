using amLogger;
using CaDb;
using Microsoft.EntityFrameworkCore;

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
    public static List<Arbitrage> GetArbitragesToShow()
    {
        using (CaDbContext db = new())
        {
            var prods = db.Database
                .SqlQuery<Arbitrage>(
                    @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage

                        select * from Sinex_Arbitrage 
                        where 
                            shotNumber = @n
                        and vol1 > 0
                        and vol2 > 0
                        and procDiffer > 1.5
                        order by procDiffer desc
                    "
            ).ToList<Arbitrage>();
            return prods;
        }
    }
    public static List<Arbitrage> GetArbitragesToSend()
    {
        using (CaDbContext db = new())
        {
            var prods = db.Database
                .SqlQuery<Arbitrage>(
                    @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage

                        select * from Sinex_Arbitrage 
                        where 
                            shotNumber = @n
                        and vol1 > 0
                        and vol2 > 0
                        and dtSentToBot is null
                        and procDiffer > 1.5
                        order by procDiffer desc
                    "
            ).ToList<Arbitrage>();
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
    public static async Task SaveFullStat(FullStat s) {

        if (s.exc1 == null || s.exc2 == null) return;

        using (CaDbContext db = new())
        {
            try
            {
                await db.Database.ExecuteSqlAsync(
                    @$"
                        declare @n int
                        select @n=max(shotNumber) from Sinex_Arbitrage
                        update Sinex_Arbitrage 
                            set procDiffer={s.proc},
                                exch1={s.exc1.Name}, 
                                exch2={s.exc2.Name},
                                vol1={s.vol1},
                                vol2={s.vol2}             
                        where 
                            shotNumber=@n 
                            and baseAsset={s.asset}
                            and quoteAsset='USDT'"
                );
            }
            catch (Exception e)
            {
                Log.Error(
                    @$"Stat Save 
                        {s.asset} {s.proc} {s.exc1.Name} {s.exc2.Name} {s.vol1} {s.vol2}"
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