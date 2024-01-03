using CaSecrets;
using Kraken.Net.Clients;
using CryptoExchange.Net.Authentication;

namespace CoinsLoader;

public class CaKraken : AnExchange
{
    public const int ID = 7;

    public override async Task GetCoins()
    {
        try
        {
            string apiKey = Secrets.KrakenApiKey;
            string apiSecret = Secrets.KrakenApiSecret;

            var api = new KrakenRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(apiKey, apiSecret);
            });

            var r = await api.SpotApi.ExchangeData.GetAssetsAsync();
            if (r.Success)
            {
                foreach (var c in r.Data)
                {
                    Coin cd = new();

                    cd.exchId = ID;

                    //cd.asset = c.Value.;
                    //cd.longName = c.Name;
                    //cd.network = c.Chain.Replace(c.Currency + "-", "");
                    //cd.logoPath = c.LogoLink;

                    //await cd.Save();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }


}
