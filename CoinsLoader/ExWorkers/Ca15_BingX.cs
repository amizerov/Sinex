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

        Log.Info(ID, "GetCoins", "Start");

        try
        {
            JsonDocument doc = JsonDocument.Parse(res);
            JsonElement ele = doc.RootElement;
            JsonElement coins = ele.GetProperty("data");

            int cnt = 0;
            int cntCoins = coins.EnumerateArray().Count();
            foreach (var p in coins.EnumerateArray())
            {
                Coin coin = new(ID);
                coin.asset = p.GetProperty("coin").GetString() + "";
                coin.longName = p.GetProperty("name").GetString() + "";

                bool first = true;
                var nets = p.GetProperty("networkList");
                int cntChains = nets.EnumerateArray().Count();
                foreach (var n in nets.EnumerateArray())
                {
                    try
                    {
                        string fee = n.GetProperty("withdrawFee").GetString()!;
                        if (fee == "") fee = "0";

                        string chainName = n.GetProperty("network").GetString() + "";
                        string chainCode = ValidateChainCode(chainName);
                        coin.network = chainCode;
                        Chain chain = new Chain(chainCode);
                        chain.name = chainName;
                        chain.name2 = $"[{ID}]";
                        int chainId = await chain.Save();

                        if (first || n.GetProperty("isDefault").GetBoolean())
                        {
                            coin.chainId = chainId;
                            coin.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                            coin.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                            coin.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                            //cd.contract = n.GetProperty("contract").GetString() + "";

                            await coin.Save();
                            first = false;

                            Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}/{cntChains}");
                        }

                        CoinChain coinChain = new CoinChain();
                        coinChain.coinId = coin.id;
                        coinChain.chainId = chainId;
                        coinChain.chainName = chainCode;
                        //chain.contractAddress = n.GetProperty("contract").GetString() + "";
                        coinChain.allowDeposit = n.GetProperty("depositEnable").GetBoolean();
                        coinChain.allowWithdraw = n.GetProperty("withdrawEnable").GetBoolean();
                        coinChain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                        coinChain.minDepositAmt = float.Parse(n.GetProperty("depositMin").GetString()!, CultureInfo.InvariantCulture);
                        coinChain.minWithdrawal = float.Parse(n.GetProperty("withdrawMin").GetString()!, CultureInfo.InvariantCulture);
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

        Log.Info(ID, "GetCoins", "End");
    }
}
