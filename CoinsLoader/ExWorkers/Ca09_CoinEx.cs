using amLogger;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class CoinEx : AnExchange
{
    public const int ID = 9;
    public const string BASE_URL = "https://api.coinex.com";
    public const string PREFIX = "/v1";

    public override async Task GetCoins()
    {
        Log.Info(ID, "GetCoins()", "Start");

        using HttpClient httpClient = new();
        var r = await httpClient.GetAsync($"{BASE_URL}{PREFIX}/common/asset/config");
        if (!r.IsSuccessStatusCode)
        {      
            Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
            return;
        }
     
        try
        {
            var s = r.Content.ReadAsStringAsync().Result;
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            JsonElement coins = e.GetProperty("data");

            string lastAsset = "";
            int cnt = 0;
            int cntCoins = coins.EnumerateObject().Count();
            foreach (var o in coins.EnumerateObject())
            {
                try
                {
                    JsonElement p = o.Value;

                    Coin coin = new(ID);
                    coin.asset = p.GetProperty("asset").GetString() + "";
                    string fee = p.GetProperty("withdraw_tx_fee").GetString()!;

                    string chainCode = p.GetProperty("chain").GetString() + "";
                    int chainId = 0;
                    try
                    {
                        CoinChain coinChain = new CoinChain();
                        coinChain.coinId = coin.Find();

                        coinChain.chainName = chainCode;
                        coinChain.allowDeposit = p.GetProperty("can_deposit").GetBoolean();
                        coinChain.allowWithdraw = p.GetProperty("can_withdraw").GetBoolean();
                        coinChain.withdrawFee = double.Parse(fee, CultureInfo.InvariantCulture);

                        Chain chain = new Chain(chainCode);
                        chain.name2 = $"[{ID}]";
                        await chain.Save();

                        coinChain.chainId = chainId = chain.id;
                        await coinChain.Save();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ID, "GetCoins() - 3", ex.Message);
                    }

                    if (lastAsset == coin.asset) continue;

                    try
                    {
                        coin.chainId = chainId;
                        coin.network = chainCode;
                        coin.allowDeposit = p.GetProperty("can_deposit").GetBoolean();
                        coin.allowWithdraw = p.GetProperty("can_withdraw").GetBoolean();
                        coin.withdrawFee = double.Parse(fee, CultureInfo.InvariantCulture);

                        await coin.Save();
                        Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}");
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ID, "GetCoins() - 4", ex.Message);
                    }

                    lastAsset = coin.asset;
                }
                catch (Exception ex)
                {
                    Log.Error(ID, "GetCoins() - 2", ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ID, "GetCoins() - 1", ex.Message);
        }

        Log.Info(ID, "GetCoins()", "End");
    }
}
