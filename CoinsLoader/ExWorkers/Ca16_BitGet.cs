using amLogger;
using System.Globalization;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class BitGet : AnExchange
{
    public const int ID = 16;
    public const string BASE_URL = "https://api.bitget.com";

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();

        string uri = $"{BASE_URL}/api/v2/spot/public/coins";
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
                coin.asset = p.GetProperty("coin").GetString() + "";
                coin.longName = p.GetProperty("coinId").GetString() + "";

                try
                {
                    JsonElement chains = p.GetProperty("chains");
                    bool first = true;
                    int cntChains = chains.EnumerateArray().Count();
                    foreach (var c in chains.EnumerateArray())
                    {
                        string fee = c.GetProperty("withdrawFee").GetString()!;
                        if (fee == "") fee = "0";

                        coin.network = c.GetProperty("chain").GetString() + "";
                        Chain chain = new(coin.network);
                        chain.name2 = $"[{ID}]";
                        int chainId = await chain.Save();

                        if (first)
                        {
                            coin.chainId = chainId;
                            coin.contract = c.GetProperty("browserUrl").GetString() + "";
                            coin.allowDeposit = c.GetProperty("rechargeable").GetString() == "true";
                            coin.allowWithdraw = c.GetProperty("withdrawable").GetString() == "true";
                            coin.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);

                            await coin.Save();
                            first = false;

                            Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}/{cntChains}");
                        }

                        CoinChain coinChain = new CoinChain();

                        coinChain.coinId = coin.id;
                        coinChain.chainId = chainId;
                        coinChain.chainName = c.GetProperty("chain").GetString() + "";
                        coinChain.contractAddress = c.GetProperty("browserUrl").GetString() + "";
                        coinChain.allowDeposit = c.GetProperty("rechargeable").GetString() == "true";
                        coinChain.allowWithdraw = c.GetProperty("withdrawable").GetString() == "true";
                        coinChain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);

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

        Log.Info(ID, "GetCoins", "End");
    }
}
