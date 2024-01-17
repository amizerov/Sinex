using amLogger;
using CaSecrets;
using OKX.Api;

namespace CoinsLoader;

public class CaOKX : AnExchange
{
    public const int ID = 8;

    public override async Task GetCoins()
    {
        string apiKey = Secrets.OKXApiKey;
        string apiSecret = Secrets.OKXApiSecret;
        string passPhrase = Secrets.OKXPassPhrase;

        var api = new OKXRestApiClient(new OKXRestApiClientOptions
        {
            RawResponse = true,
        });
        api.SetApiCredentials(apiKey, apiSecret, passPhrase);

        var r = await api.FundingAccount.GetCurrenciesAsync();
        if (r.Success)
        {
            Log.Info(ID, "GetCoins()", "Start");

            string lastAsset = "";
            foreach(var c in r.Data)
            {

                Coin coin = new();
                coin.exchId = ID;
                coin.asset = c.Currency;

                try
                {                    
                    Chain chain = new Chain();
                    chain.coinId = coin.Find();
                    chain.chainName = c.Chain.Replace(c.Currency + "-", "");
                    chain.allowDeposit = c.AllowDeposit;
                    chain.allowWithdraw = c.AllowWithdrawal;
                    chain.withdrawFee = (double)c.MaximumWithdrawalFeeForNormalAddress;
                    await chain.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ID, "GetCoins() - 3", ex.Message);
                }

                if (coin.asset == lastAsset) continue;
                try
                {
                    coin.longName = c.Name;
                    coin.network = c.Chain.Replace(c.Currency + "-", "");
                    coin.logoPath = c.LogoLink;
                    coin.allowDeposit = c.AllowDeposit;
                    coin.allowWithdraw = c.AllowWithdrawal;
                    //cd.withdrawFee = c.;

                    await coin.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ID, "GetCoins() - 2", ex.Message);
                }

                lastAsset = coin.asset;
            }
        }
        else
        {
            Log.Error(ID, "GetCoins() - 0", r.Error.Message);
        }

        Log.Info(ID, "GetCoins()", "End");
    }
}
