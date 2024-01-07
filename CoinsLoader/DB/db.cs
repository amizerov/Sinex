using CaDb;
using Microsoft.EntityFrameworkCore;

namespace CoinsLoader;

public class Db : CaDbContext
{
    public DbSet<Coin> Sinex_Coins { get; set; }
    public DbSet<Chain> Sinex_Chains { get; set; }

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
                coin.id = existing.id;

                existing.asset = coin.asset;
                existing.network = coin.network;
                existing.contract = coin.contract;
                existing.logoPath = coin.logoPath;
                existing.longName = coin.longName;
                existing.description = coin.description;
                existing.allowDeposit = coin.allowDeposit;
                existing.allowWithdraw = coin.allowWithdraw;

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
    public static async Task SaveChain(Chain chain)
    {
        using var db = new Db();
        try
        {
            var existing = await db.Sinex_Chains
                .FirstOrDefaultAsync(c => c.coinId == chain.coinId
                                       && c.chainName == chain.chainName);

            if (existing == null)
            {
                await db.Sinex_Chains.AddAsync(chain);
            }
            else
            {
                existing.contractAddress = chain.contractAddress;
                existing.withdrawFee = chain.withdrawFee;
                existing.allowDeposit = chain.allowDeposit;
                existing.allowWithdraw = chain.allowWithdraw;
                existing.minDepositAmt = chain.minDepositAmt;
                existing.minWithdrawal = chain.minWithdrawal;
                existing.dtu = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        try
        {
            var r = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
