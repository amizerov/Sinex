using amLogger;
using MexcDotNet;
using System.Text.Json;

namespace CoinsLoader;

public class Mexc : AnExchange
{
    const int ID = 10;
    const string spotBaseUrl = "https://api.mexc.com";
    string apiKey = CaSecrets.Secrets.MexcApiKey;
    string apiSecret = CaSecrets.Secrets.MexcApiSecret;

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();
        var mexcService = new MexcService(apiKey, apiSecret, spotBaseUrl, httpClient);
        var res = await mexcService.SendSignedAsync("/api/v3/capital/config/getall", HttpMethod.Get);

        try
        {
            JsonDocument doc = JsonDocument.Parse(res);
            JsonElement ele = doc.RootElement;

            foreach (var p in ele.EnumerateArray())
            {
                Coin coin = new();

                coin.exchId = ID;
                coin.asset = p.GetProperty("coin").GetString() + "";
                coin.longName = p.GetProperty("name").GetString() + "";

                bool first = true;
                var nets = p.GetProperty("networkList");
                foreach (var n in nets.EnumerateArray()) 
                {
                    try
                    {
                        if (first)
                        {
                            coin.network = n.GetProperty("network").GetString() + "";
                            coin.contract = n.GetProperty("contract").GetString() + "";
                            coin.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                            coin.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();

                            await coin.Save();
                            Log.Info(ID, "SaveCoin", coin.asset);
                            first = false;
                        }
                        Chain chain = new Chain();
                        chain.coinId = coin.id;
                        chain.chainName = n.GetProperty("network").GetString() + "";
                        chain.contractAddress = n.GetProperty("contract").GetString() + "";
                        chain.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                        chain.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                        await chain.Save();
                        Log.Trace(coin.id, $"SaveChain({coin.asset})", chain.chainName);
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

