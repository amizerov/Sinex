using CaDb;
using Microsoft.EntityFrameworkCore;

namespace CoinsLoader;

public class Db : CaDbContext
{
    public DbSet<Coin> Sinex_Coins { get; set; }

    public static async Task SaveCoin(Coin coin)
    {
        using var db = new Db();
        try
        {
            var existing = await db.Sinex_Coins.FirstOrDefaultAsync(c => c.asset == coin.asset);
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
