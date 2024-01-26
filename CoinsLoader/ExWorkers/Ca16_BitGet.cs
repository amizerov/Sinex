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

                    coin.asset = p.GetProperty("coin").GetString() + "";
                    coin.longName = p.GetProperty("coinId").GetString() + "";

                    try
                    {
                        JsonElement chains = p.GetProperty("chains");
                        bool first = true;
                        foreach (var c in chains.EnumerateArray())
                        {
                            string fee = c.GetProperty("withdrawFee").GetString()!;
                            if (fee == "") fee = "0";

                            if (first)
                            {
                                coin.network = c.GetProperty("chain").GetString() + "";
                                coin.contract = c.GetProperty("browserUrl").GetString() + "";

                                coin.allowDeposit = c.GetProperty("rechargeable").GetString() == "true";
                                coin.allowWithdraw = c.GetProperty("withdrawable").GetString() == "true";
                                coin.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);

                                await coin.Save();
                                Log.Info(ID, "SaveCoin", coin.asset);
                                first = false;
                            }

                            CoinChain chain = new CoinChain();

                            chain.coinId = coin.id;
                            chain.chainName = c.GetProperty("chain").GetString() + "";
                            chain.contractAddress = c.GetProperty("browserUrl").GetString() + "";
                            chain.allowDeposit = c.GetProperty("rechargeable").GetString() == "true";
                            chain.allowWithdraw = c.GetProperty("withdrawable").GetString() == "true";
                            chain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);

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
