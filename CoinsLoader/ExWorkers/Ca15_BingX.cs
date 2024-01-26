using amLogger;
using MexcDotNet;
using System.Globalization;
using System.Text.Json;

namespace CoinsLoader.Worker;

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
                    try
                    {
                        string fee = n.GetProperty("withdrawFee").GetString()!;
                        if (fee == "") fee = "0";

                        if (first || n.GetProperty("isDefault").GetBoolean())
                        {
                            cd.network = n.GetProperty("network").GetString() + "";
                            cd.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                            cd.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                            cd.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                            //cd.contract = n.GetProperty("contract").GetString() + "";

                            await cd.Save();
                            first = false;
                        }
                        CoinChain chain = new CoinChain();
                        chain.coinId = cd.id;
                        chain.chainName = n.GetProperty("network").GetString() + "";
                        //chain.contractAddress = n.GetProperty("contract").GetString() + "";
                        chain.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                        chain.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                        chain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                        chain.minDepositAmt = float.Parse(n.GetProperty("depositMin").GetString()!, CultureInfo.InvariantCulture);
                        chain.minWithdrawal = float.Parse(n.GetProperty("withdrawMin").GetString()!, CultureInfo.InvariantCulture);
                        await chain.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ID, "GetCoins 2", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ID, "GetCoins 1", ex.Message);
        }
    }
}
