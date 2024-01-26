using amLogger;
using MexcDotNet;
using System.Text.Json;

namespace CoinsLoader.Worker;

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
                        string net = n.GetProperty("network").GetString() + "";
                        if (first)
                        {
                            int i1 = net.IndexOf('(') + 1;
                            int i2 = net.IndexOf(')');
                            if (i1 > 1 && i2 > 0)
                                net = net.Substring(i1, i2 - i1);

                            coin.network = net;
                            coin.contract = n.GetProperty("contract").GetString() + "";
                            coin.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                            coin.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();

                            await coin.Save();
                            Log.Info(ID, "SaveCoin", coin.asset);
                            first = false;
                        }
                        CoinChain chain = new CoinChain();
                        chain.coinId = coin.id;
                        chain.chainName = net;
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

