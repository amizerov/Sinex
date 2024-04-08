using amLogger;
using CaDb;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot2;

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
}
