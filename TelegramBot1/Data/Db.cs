﻿using CaDb;
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
}