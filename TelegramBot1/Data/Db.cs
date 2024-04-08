using amLogger;
using CaDb;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot1;

public class Db
{
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
    public static Params LoadParams()
    {
        using CaDbContext db = new();
        Params? p = db.Database
            .SqlQuery<Params>(@$"
                select top 1 minProc, minProf, minVolu, maxVolu 
                from Sinex_Params order by dtc desc
            ").FirstOrDefault();
        return p ?? new Params();
    }
    public static async Task AddCaTeleBotUser(long chatId, string userName="")
    {
        using (CaDbContext db = new())
        {
            await db.Database
                .ExecuteSqlAsync(
                    @$"
                        declare @id int
                        select @id=ID from Sinex_CaTeleBotUsers where chatId={chatId}

                        if @id is null
                            insert Sinex_CaTeleBotUsers(chatId,userName) 
                            values({chatId},{userName})
                        else
                            update Sinex_CaTeleBotUsers 
                            set userName={userName}, dtu=getdate() where ID=@id
                    "
                 );
        }
    }
    public static async Task SaveBandle(Bandle b)
    {
        using CaDbContext db = new();
        
        await db.Database
            .ExecuteSqlAsync(@$"

                INSERT Sinex_Bundles
                (   coin
                    ,exchBuy
                    ,exchSell
                    ,priceBuyBid
                    ,priceBuyAsk
                    ,priceSellBid
                    ,priceSellAsk
                    ,volBuy
                    ,volSell
                    ,lastBuy
                    ,lastSell
                    ,lastVolBuy
                    ,lastVolSell
                    ,chain
                    ,withdrawFee
                )
                VALUES(
                    {b.coin},
                    {b.exchBuy},{b.exchSell},
                    {b.priceBuyBid},{b.priceBuyAsk},
                    {b.priceSellBid},{b.priceSellAsk},
                    {b.volBuy},{b.volSell},
                    {b.lastBuy},{b.lastSell},{b.lastVolBuy},{b.lastVolSell},
                    {b.chain},{b.withdrawFee}
                )

            ");
    }
    public static async Task CloseBandle(Bandle b)
    {
        using CaDbContext db = new();
        try
        {
            List<int> ids = db.Database
                .SqlQuery<int>(@$"
                    select top 1 ID from Sinex_Bundles 
                    where coin={b.coin} 
                      and exchBuy={b.exchBuy} 
                      and exchSell={b.exchSell}
                      and dtu is null
                    order by dtc desc
                ").ToList();
            if (ids.Count > 0)
            {
                int id = ids[0];
                await db.Database
                    .ExecuteSqlAsync(@$"
                    update Sinex_Bundles set dtu=getdate() where id={id}
                ");
            }
        } catch (Exception ex)
        {
            Log.Error("CloseBandle", ex.Message);
        }
    }
    public static List<string> GetCurBandles()
    {
        using CaDbContext db = new();

        List<string> bandles = db.Database
            .SqlQuery<string>(@$"	
                    SELECT distinct coin FROM Sinex_Bundles 
	                where datediff(HOUR, dtc, getdate()) < 5 and dtu is null"
                ).ToList();

        return bandles;
    }
    public static async Task UpdateSpread(double val)
    {
        using CaDbContext db = new();

        await db.Database
            .ExecuteSqlAsync(@$"
                declare @id int
                select top 1 @id=ID from Sinex_Params order by dtc desc
                update Sinex_Params set minProc={val} where id = @id");
    }
    public static async Task UpdateMinvol(double val)
    {
        using CaDbContext db = new();

        await db.Database
            .ExecuteSqlAsync(@$"
                declare @id int
                select top 1 @id=ID from Sinex_Params order by dtc desc
                update Sinex_Params set minVolu={val} where id = @id");
    }
    public static async Task UpdateMaxvol(double val)
    {
        using CaDbContext db = new();

        await db.Database
            .ExecuteSqlAsync(@$"
                declare @id int
                select top 1 @id=ID from Sinex_Params order by dtc desc
                update Sinex_Params set maxVolu={val} where id = @id");
    }
    public static async Task UpdateMinpro(double val)
    {
        using CaDbContext db = new();

        await db.Database
            .ExecuteSqlAsync(@$"
                declare @id int
                select top 1 @id=ID from Sinex_Params order by dtc desc
                update Sinex_Params set minProf={val} where id = @id");
    }
}
