using amLogger;
using System.Text.Json;

namespace CoinsLoader;

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
                    Coin cd = new();

                    cd.exchId = ID;

                    cd.asset = p.GetProperty("currency").GetString() + "";
                    cd.longName = p.GetProperty("fullName").GetString() + "";

                    await cd.Save();
                    try
                    {
                        JsonElement chains = p.GetProperty("chains");
                        bool first = true;
                        foreach (var c in chains.EnumerateArray())
                        {
                            Chain chain = new Chain();
                            chain.coinId = cd.id;

                            if (first)
                            {
                                cd.network = c.GetProperty("chainName").GetString() + "";
                                cd.contract = c.GetProperty("contractAddress").GetString() + "";

                                cd.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                                cd.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();

                                first = false;
                            }

                            chain.chainName = c.GetProperty("chainName").GetString() + "";
                            chain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                            chain.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                            chain.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();

                            await chain.Save();
                        }
                    }
                    catch(Exception ex) 
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
