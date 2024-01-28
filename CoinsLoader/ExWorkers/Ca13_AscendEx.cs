using amLogger;
using System.Globalization;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class AscendEx : AnExchange
{
    public int ID = 13;
    public const string BASE_URL = "https://ascendex.com";

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();

        string uri = $"{BASE_URL}/api/pro/v2/assets";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        var r = await httpClient.SendAsync(req);
        if (!r.IsSuccessStatusCode)
        {      
            Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
            return;
        }

        Log.Info(ID, "GetCoins", "Start");

        try
        {
            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            JsonElement coins = e.GetProperty("data");

            int cnt = 0;
            int cntCoins = coins.EnumerateArray().Count();
            foreach (var p in coins.EnumerateArray())
            {
                Coin coin = new(ID);
                coin.asset = p.GetProperty("assetCode").GetString() + "";
                coin.longName = p.GetProperty("assetName").GetString() + "";

                try
                {
                    bool first = true;
                    var bc = p.GetProperty("blockChain");
                    foreach (var c in bc.EnumerateArray())
                    {
                        try
                        {
                            string sfee = c.GetProperty("withdrawFee").GetString()!;
                            if (sfee == "") sfee = "0";
                            var fee = float.Parse(sfee, CultureInfo.InvariantCulture);

                            string chainCode = "";
                            string chainName = c.GetProperty("chainName").GetString() + "";
                            int i = chainName.IndexOf('(');
                            if (i > 1)
                                chainCode = chainName.Substring(0, i).Trim();
                            else
                                chainCode = chainName;

                            chainCode = ValidateChainCode(chainCode);

                            Chain chain = new(chainCode);
                            chain.name = chainName;
                            chain.name2 = $"[{ID}]";
                            int chainId = await chain.Save();

                            if (first)
                            {
                                coin.chainId = chainId;
                                coin.network = chainCode;
                                coin.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                                coin.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();

                                coin.withdrawFee = fee;

                                await coin.Save();
                                first = false;

                                Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}");
                            }

                            CoinChain coinChain = new CoinChain();
                            coinChain.coinId = coin.id;
                            coinChain.chainId = chainId;
                            coinChain.chainName = chainCode;
                            //chain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                            coinChain.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                            coinChain.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();
                            coinChain.minDepositAmt = float.Parse(c.GetProperty("minDepositAmt").GetString()!, CultureInfo.InvariantCulture);
                            coinChain.minWithdrawal = float.Parse(c.GetProperty("minWithdrawal").GetString()!, CultureInfo.InvariantCulture);

                            coinChain.withdrawFee = fee;

                            await coinChain.Save();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ID, "GetCoins 3", ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ID, "GetCoins 2", ex.Message);
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