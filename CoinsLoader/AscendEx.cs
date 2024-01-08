using System.Globalization;
using System.Text.Json;

namespace CoinsLoader;

public class AscendEx : AnExchange
{
    public const int ID = 13;
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

                    try { 
                        bool first = true;
                        var bc = p.GetProperty("blockChain");
                        foreach (var c in bc.EnumerateArray())
                        {
                            if (first)
                            {
                                coin.network = c.GetProperty("chainName").GetString() + "";
                                coin.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                                coin.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();

                                string fee = c.GetProperty("withdrawFee").GetString()!;
                                coin.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);

                                await coin.Save();
                                first = false;
                            }
                            Chain chain = new Chain();
                            chain.coinId = coin.id;
                            chain.chainName = c.GetProperty("chainName").GetString() + "";
                            //chain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                            chain.allowDeposit = c.GetProperty("allowDeposit").GetBoolean();
                            chain.allowWithdraw = c.GetProperty("allowWithdraw").GetBoolean();
                            
                            string wfee = c.GetProperty("withdrawFee").GetString()!;
                            chain.withdrawFee = float.Parse(wfee, CultureInfo.InvariantCulture);

                            await chain.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}