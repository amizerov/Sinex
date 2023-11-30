using CaDb;
using Microsoft.EntityFrameworkCore;

namespace bot5;

class Data
{
    public static List<ProdEx> GetProds(string seatch)
    {
        using (CaDbContext db = new())
        {
            var prods = db.Database
                .SqlQuery<ProdEx>($"Sinex_GetProductsExch {seatch}");

            return prods.ToList();
        }
    }
    public static List<Arbitrage> GetArbitrages()
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
    public static void UpdateSentArbitr(int id)
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