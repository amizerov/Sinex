using amLogger;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace CoinsLoader;

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
        if (r.StatusCode == HttpStatusCode.OK)
        {
            try { 
                var s = r.Content.ReadAsStringAsync().Result;
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                JsonElement data = e.GetProperty("data");

                string lastAsset = "";
                foreach (var o in data.EnumerateObject())
                {
                    try
                    {
                        JsonElement p = o.Value;

                        Coin coin = new();
                        coin.exchId = ID;
                        coin.asset = p.GetProperty("asset").GetString() + "";
                        string fee = p.GetProperty("withdraw_tx_fee").GetString()!;

                        if (lastAsset == coin.asset)
                        {
                            try
                            {
                                Chain chain = new Chain();
                                chain.coinId = coin.Find();

                                chain.chainName = p.GetProperty("chain").GetString() + "";
                                chain.allowDeposit = p.GetProperty("can_deposit").GetBoolean();
                                chain.allowWithdraw = p.GetProperty("can_withdraw").GetBoolean();
                                chain.withdrawFee = double.Parse(fee, CultureInfo.InvariantCulture);

                                await chain.Save();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ID, "GetCoins() - 3", ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                coin.network = p.GetProperty("chain").GetString() + "";
                                coin.allowDeposit = p.GetProperty("can_deposit").GetBoolean();
                                coin.allowWithdraw = p.GetProperty("can_withdraw").GetBoolean();
                                coin.withdrawFee = double.Parse(fee, CultureInfo.InvariantCulture);

                                await coin.Save();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ID, "GetCoins() - 4", ex.Message);
                            }
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
        }
        else
        {
            Log.Error(ID, $"GetCoins() - 0", r.StatusCode.ToString());
        }

        Log.Info(ID, "GetCoins()", "End");
    }
}
