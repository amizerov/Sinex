using amLogger;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class Kucoin : AnExchange
{
    public const int ID = 2;
    public const string BASE_URL = "https://api.kucoin.com";

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();

        string uri = $"{BASE_URL}/api/v3/currencies";
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

                    coin.asset = p.GetProperty("currency").GetString() + "";
                    coin.longName = p.GetProperty("fullName").GetString() + "";

                    await coin.Save();
                    try
                    {
                        JsonElement chains = p.GetProperty("chains");
                        bool first = true;
                        foreach (var c in chains.EnumerateArray())
                        {
                            Chain chain = new Chain();
                            chain.coinId = coin.id;

                            if (first)
                            {
                                coin.network = c.GetProperty("chainName").GetString() + "";
                                coin.contract = c.GetProperty("contractAddress").GetString() + "";

                                coin.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                                coin.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();

                                await coin.Save();
                                first = false;
                            }

                            chain.chainName = c.GetProperty("chainName").GetString() + "";
                            chain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                            chain.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                            chain.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();

                            await chain.Save();
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
