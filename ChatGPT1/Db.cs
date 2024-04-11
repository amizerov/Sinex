using CaDb;
using Microsoft.EntityFrameworkCore;

namespace ChatGPT1
{
    public class Db
    {
        public static void SaveQA(string q, string a, int u)
        {
            using (CaDbContext db = new())
            {
                db.Database
                    .ExecuteSql(
                        @$"
                            insert Sinex_ChatGPT_QA(question, answer, uid)
                            values({q}, {a}, {u})
                        "
                    );
            }
        }
    }
}
