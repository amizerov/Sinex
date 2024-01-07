using MexcDotNet;
using System.Text.Json;

namespace CoinsLoader;

public class BingX : AnExchange
{
    const int ID = 15;
    const string spotBaseUrl = "https://open-api.bingx.com";
    string apiKey = CaSecrets.Secrets.BingXApiKey;
    string apiSecret = CaSecrets.Secrets.BingXApiSecret;

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();
        var bingxService = new BingXService(apiKey, apiSecret, spotBaseUrl, httpClient);
        var res = await bingxService.SendSignedAsync("/openApi/wallets/v1/capital/config/getall", HttpMethod.Get);

        try
        {
            JsonDocument doc = JsonDocument.Parse(res);
            JsonElement ele = doc.RootElement;
            JsonElement data = ele.GetProperty("data");

            foreach (var p in data.EnumerateArray())
            {
                Coin cd = new();

                cd.exchId = ID;
                cd.asset = p.GetProperty("coin").GetString() + "";
                cd.longName = p.GetProperty("name").GetString() + "";

                bool first = true;
                var nets = p.GetProperty("networkList");
                foreach (var n in nets.EnumerateArray())
                {
                    if (first)
                    {
                        cd.network = n.GetProperty("network").GetString() + "";
                        //cd.contract = n.GetProperty("contract").GetString() + "";
                        
                        await cd.Save();
                        first = false;
                    }
                    Chain chain = new Chain();
                    chain.coinId = cd.id;
                    chain.chainName = n.GetProperty("network").GetString() + "";
                    //chain.contractAddress = n.GetProperty("contract").GetString() + "";
                    chain.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                    chain.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                    await chain.Save();
                }
                //await cd.Save();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
