using CaDb;
using Microsoft.EntityFrameworkCore;

namespace caLibPairsStat6;

public class Db : CaDbContext
{
    public DbSet<Product>? Products { get; set; }

    public DbSet<Coin> Sinex_Coins { get; set; }

    public static async Task SaveCoin(Coin coin)
    {
        using var db = new Db();
        try
        {
            var existing = await db.Sinex_Coins
                .FirstOrDefaultAsync(c => c.asset == coin.asset 
                                  && c.exchId == coin.exchId);

            if (existing == null)
            {
                await db.Sinex_Coins.AddAsync(coin);
            }
            else
            {
                existing.asset = coin.asset;
                existing.network = coin.network;
                existing.contract = coin.contract;
                existing.logoPath = coin.logoPath;
                existing.longName = coin.longName;
                existing.description = coin.description;
                existing.dtu = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        try {
            var r = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
