using CaDb;
using Microsoft.EntityFrameworkCore;

namespace ChatGPT1
{
    public class Db
    {
        public static void SaveQAToDb(string q, string a)
        {
            using (CaDbContext db = new())
            {
                db.Database
                    .ExecuteSql(
                        @$"
                            insert Sinex_ChatGPT_QA(question, answer)
                            values({q}, {a})
                        "
                    );
            }
        }
    }
}
