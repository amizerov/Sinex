using amLogger;
using System.Globalization;
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

        if (!r.IsSuccessStatusCode)
        {
            Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
            return;
        }

        Log.Info(ID, "GetCoins()", "Start");

        try
        {
            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            JsonElement data = e.GetProperty("data");
            int cnt = 0;
            int cntCoins = data.EnumerateArray().Count();
            foreach (var p in data.EnumerateArray())
            {
                Coin coin = new(ID);
                coin.asset = p.GetProperty("currency").GetString() + "";
                coin.longName = p.GetProperty("fullName").GetString() + "";

                await coin.Save();
                try
                {
                    JsonElement chains = p.GetProperty("chains");
                    if(chains.ValueKind == JsonValueKind.Null)
                    {
                        //coin.Delete();
                        continue;
                    }
                    bool first = true;
                    int cntChains = chains.EnumerateArray().Count();
                    foreach (var c in chains.EnumerateArray())
                    {
                        string net = c.GetProperty("chainName").GetString() + "";
                        Chain chain = new Chain(net);
                        chain.name2 = $"[{ID}]";
                        await chain.Save();

                        CoinChain coinChain = new CoinChain();
                        coinChain.coinId = coin.id;
                        coinChain.chainId = chain.id;

                        if (first)
                        {
                            coin.network = net;
                            coin.chainId = chain.id;
                            coin.contract = c.GetProperty("contractAddress").GetString() + "";

                            coin.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                            coin.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();

                            await coin.Save();
                            first = false;

                            Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}/{cntChains}");
                        }

                        coinChain.chainName = net;
                        coinChain.contractAddress = c.GetProperty("contractAddress").GetString() + "";
                        coinChain.allowDeposit = c.GetProperty("isDepositEnabled").GetBoolean();
                        coinChain.allowWithdraw = c.GetProperty("isWithdrawEnabled").GetBoolean();
                        coinChain.withdrawFee = float.Parse(c.GetProperty("withdrawalMinFee").GetString()!, CultureInfo.InvariantCulture);

                        await coinChain.Save();
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
