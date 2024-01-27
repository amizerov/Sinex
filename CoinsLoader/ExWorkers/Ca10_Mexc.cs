using amLogger;
using MexcDotNet;
using System.Drawing;
using System.Globalization;
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
        Log.Info(ID, "GetCoins", "Start");

        using HttpClient httpClient = new();
        var mexcService = new MexcService(apiKey, apiSecret, spotBaseUrl, httpClient);
        var res = await mexcService.SendSignedAsync("/api/v3/capital/config/getall", HttpMethod.Get);

        try
        {
            JsonDocument doc = JsonDocument.Parse(res);
            JsonElement coins = doc.RootElement;

            int cnt = 0;
            int cntCoins = coins.EnumerateArray().Count();

            foreach (var p in coins.EnumerateArray())
            {
                cnt++;
                Coin coin = new();

                coin.exchId = ID;
                coin.asset = p.GetProperty("coin").GetString() + "";
                coin.longName = p.GetProperty("name").GetString() + "";

                bool first = true;
                var nets = p.GetProperty("networkList");
                int cntChains = nets.EnumerateArray().Count();
                string chainCode = "";
                string chainName = "";

                foreach (var n in nets.EnumerateArray())
                {
                    try
                    {
                        chainName = n.GetProperty("network").GetString() + "";
                        int i1 = chainName.IndexOf('(') + 1;
                        int i2 = chainName.IndexOf(')');
                        if (i1 > 1 && i2 > 0)
                            chainCode = chainName.Substring(i1, i2 - i1);
                        else
                            chainCode = chainName;

                        Chain chain = new Chain(chainCode);
                        chain.name = chainName;
                        await chain.Save();

                        if (first)
                        {
                            coin.chainId = chain.id;
                            coin.network = chainCode;
                            coin.contract = n.GetProperty("contract").GetString() + "";
                            coin.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                            coin.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();

                            await coin.Save();
                            first = false;

                            Log.Info(ID, $"SaveCoin({coin.asset})", $"{cnt}/{cntCoins}/{cntChains}");
                        }
                        CoinChain coinChain = new CoinChain();
                        coinChain.coinId = coin.id;
                        coinChain.chainId = chain.id;
                        coinChain.chainName = chainCode;
                        coinChain.contractAddress = n.GetProperty("contract").GetString() + "";
                        coinChain.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                        coinChain.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                        coinChain.withdrawFee = float.Parse(n.GetProperty("withdrawFee").GetString()!, CultureInfo.InvariantCulture);
                        await coinChain.Save();
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
        
        Log.Info(ID, "GetCoins()", "Done");
    }
}

