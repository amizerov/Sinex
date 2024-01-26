using amLogger;
using CaDb;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CoinsLoader;

public class Db : CaDbContext
{
    public DbSet<Coin> Sinex_Coins { get; set; }
    public DbSet<CoinChain> Sinex_CoinChains { get; set; }
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
                existing.chainId = coin.chainId;
                existing.network = coin.network;
                existing.contract = coin.contract;
                existing.logoPath = coin.logoPath;
                existing.longName = coin.longName;
                existing.description = coin.description;
                existing.allowDeposit = coin.allowDeposit;
                existing.allowWithdraw = coin.allowWithdraw;
                existing.withdrawFee = coin.withdrawFee;

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
    public static async Task SaveCoinChain(CoinChain coinChain)
    {
        using var db = new Db();
        try
        {
            var existing = await db.Sinex_CoinChains
                .FirstOrDefaultAsync(c => c.coinId == coinChain.coinId
                                       && c.chainName == coinChain.chainName);

            if (existing == null)
            {
                await db.Sinex_CoinChains.AddAsync(coinChain);
            }
            else
            {
                existing.chainId = coinChain.chainId;
                existing.contractAddress = coinChain.contractAddress;
                existing.withdrawFee = coinChain.withdrawFee;
                existing.allowDeposit = coinChain.allowDeposit;
                existing.allowWithdraw = coinChain.allowWithdraw;
                existing.minDepositAmt = coinChain.minDepositAmt;
                existing.minWithdrawal = coinChain.minWithdrawal;
                existing.withdrawFee = coinChain.withdrawFee;
                existing.dtu = DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"SaveCoinChain({coinChain.chainName}) - 1", ex.Message);
        }
        try
        {
            var r = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error($"SaveCoinChain({coinChain.chainName}) - 2", ex.Message);
        }
    }
    public static async Task SaveChain(Chain chain)
    {
        using var db = new Db();
        try
        {
            var existing = await db.Sinex_Chains
                .FirstOrDefaultAsync(c => c.code == chain.code);

            if (existing == null)
            {
                await db.Sinex_Chains.AddAsync(chain);
            }
            else
            {
                existing.code = chain.code;
                existing.name = chain.name;
                existing.name1 = chain.name1;
                existing.name2 = chain.name2;
                existing.dtu = DateTime.Now;

                chain.id = existing.id;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"SaveCoinChain({chain.name}) - 1", ex.Message);
        }
        try
        {
            var r = await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Log.Error($"SaveCoinChain({chain.name}) - 2", ex.Message);
        }
    }
    public static int FindCoinByName(Coin coin)
    {
        using var db = new Db();
        try
        {
            var existing = db.Sinex_Coins
                .FirstOrDefault(c => c.asset == coin.asset
                                  && c.exchId == coin.exchId);

            if (existing == null)
            {
                return 0;
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
                existing.withdrawFee = coin.withdrawFee;

                return existing.id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
    public static int FindCoinByLongName(Coin coin)
    {
        using var db = new Db();
        try
        {
            var existing = db.Sinex_Coins
                .FirstOrDefault(c => c.longName == coin.longName
                                  && c.exchId == coin.exchId);

            if (existing == null)
            {
                return 0;
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
                existing.withdrawFee = coin.withdrawFee;

                return existing.id;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }
}
