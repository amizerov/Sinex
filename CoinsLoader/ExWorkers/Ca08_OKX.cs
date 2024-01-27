using amLogger;
using CaSecrets;
using OKX.Api;

namespace CoinsLoader.Worker;

public class CaOKX : AnExchange
{
    public const int ID = 8;
    string apiKey = Secrets.OKXApiKey;
    string apiSecret = Secrets.OKXApiSecret;
    string passPhrase = Secrets.OKXPassPhrase;

    public override async Task GetCoins()
    {
        var api = new OKXRestApiClient(new OKXRestApiClientOptions
        {
            RawResponse = true,
        });
        api.SetApiCredentials(apiKey, apiSecret, passPhrase);

        var r = await api.FundingAccount.GetCurrenciesAsync();
        if (!r.Success)
        {
            Log.Error(ID, "GetCoins() - 1", r.Error.Message);
            return;
        }

        Log.Info(ID, "GetCoins()", "Start");

        int cnt = 0;
        int cntCoins = r.Data.Count();
        string lastAsset = "";
        foreach (var c in r.Data)
        {
            Coin coin = new(ID);
            coin.asset = c.Currency;

            int chainId = 0;
            string chainName = c.Chain;
            string chainCode = c.Chain.Replace(c.Currency + "-", "");
            try
            {
                CoinChain coinChain = new CoinChain();
                coinChain.coinId = coin.Find();
                coinChain.chainName = chainCode;
                coinChain.allowDeposit = c.AllowDeposit;
                coinChain.allowWithdraw = c.AllowWithdrawal;
                coinChain.withdrawFee = (double)c.MaximumWithdrawalFeeForNormalAddress;

                Chain chain = new Chain(chainCode);
                chain.name = chainName;
                chain.name2 = $"[{ID}]";
                await chain.Save();

                coinChain.chainId = chainId = chain.id;
                await coinChain.Save();
            }
            catch (Exception ex)
            {
                Log.Error(ID, "GetCoins() - 3", ex.Message);
            }

            if (coin.asset == lastAsset) continue;
            try
            {
                coin.longName = c.Name;
                coin.chainId = chainId;
                coin.network = chainCode;
                coin.logoPath = c.LogoLink;
                coin.allowDeposit = c.AllowDeposit;
                coin.allowWithdraw = c.AllowWithdrawal;
                //cd.withdrawFee = c.;

                await coin.Save();

                Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}");
            }
            catch (Exception ex)
            {
                Log.Error(ID, "GetCoins() - 2", ex.Message);
            }

            lastAsset = coin.asset;
        }

        Log.Info(ID, "GetCoins()", "End");
    }
}
