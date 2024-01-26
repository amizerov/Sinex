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
        if (r.IsSuccessStatusCode)
        {
            try
            {
                var s = await r.Content.ReadAsStringAsync();
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                JsonElement data = e.GetProperty("data");

                foreach (var p in data.EnumerateArray())
                {
                    Coin coin = new();

                    coin.exchId = ID;

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

                                string net = c.GetProperty("chainName").GetString() + "";
                                int i = net.IndexOf('(');
                                if (i > 1)
                                    net = net.Substring(0, i).Trim();

                                if (first)
                                {
                                    coin.network = net;
                                    coin.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                                    coin.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();

                                    coin.withdrawFee = fee;

                                    await coin.Save();
                                    first = false;
                                }
                                CoinChain chain = new CoinChain();
                                chain.coinId = coin.id;
                                chain.chainName = net;
                                //chain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                                chain.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                                chain.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();
                                chain.minDepositAmt = float.Parse(c.GetProperty("minDepositAmt").GetString()!, CultureInfo.InvariantCulture);
                                chain.minWithdrawal = float.Parse(c.GetProperty("minWithdrawal").GetString()!, CultureInfo.InvariantCulture);

                                chain.withdrawFee = fee;

                                await chain.Save();
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
        }
    }
}