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
            foreach(var c in r.Data)
            {
                Coin cd = new();

                cd.exchId = ID;

                cd.asset = c.Currency;
                cd.longName = c.Name;
                cd.network = c.Chain.Replace(c.Currency + "-", "");
                cd.logoPath = c.LogoLink;
                cd.allowDeposit = c.AllowDeposit;
                cd.allowWithdraw = c.AllowWithdrawal;
                //cd.withdrawFee = c.;

                await cd.Save();
            }
        }
    }

}
