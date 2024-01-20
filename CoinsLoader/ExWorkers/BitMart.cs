using amLogger;
using System.Globalization;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class BitMart : AnExchange
{
    public const int ID = 12;
    public const string BASE_URL = "https://api-cloud.bitmart.com";

    public override async Task GetCoins()
    {
        using HttpClient clt = new();

        string uri = $"{BASE_URL}/account/v1/currencies";
        //string uri = $"{BASE_URL}/spot/v1/currencies";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        try
        {
            var r = await clt.SendAsync(req);
            if (r.IsSuccessStatusCode)
            {
                var s = await r.Content.ReadAsStringAsync();
                JsonDocument j = JsonDocument.Parse(s);
                JsonElement e = j.RootElement;
                JsonElement data = e.GetProperty("data");
                JsonElement ccs = data.GetProperty("currencies");
                foreach (var c in ccs.EnumerateArray())
                {
                    try
                    {
                        Coin coin = new();

                        coin.exchId = ID;

                        var name = coin.asset = c.GetProperty("currency").GetString() + "";
                        coin.longName = c.GetProperty("name").GetString() + "";
                        var netw = coin.network = c.GetProperty("network").GetString() + "";
                        var cont = coin.contract = c.GetProperty("contract_address").GetString() + "";
                        var adep = coin.allowDeposit = c.GetProperty("deposit_enabled").GetBoolean();
                        var awit = coin.allowWithdraw = c.GetProperty("withdraw_enabled").GetBoolean();

                        var fee = c.GetProperty("withdraw_minfee").GetString() + "";
                        if (fee.Length == 0) fee = "0";
                        coin.withdrawFee = double.Parse(fee, NumberStyles.Currency);

                        coin.asset = name.Replace("-" + netw, "");

                        int id1 = coin.Find();
                        int id2 = coin.FindLong();
                        int id = id1 > 0 ? id1 : id2;

                        if (id > 0)
                        {
                            Chain chain = new();
                            chain.coinId = id;
                            chain.chainName = netw;
                            chain.contractAddress = cont;
                            chain.allowDeposit = adep;
                            chain.allowWithdraw = awit;
                            chain.withdrawFee = coin.withdrawFee;
                            chain.minWithdrawal =
                                double.Parse(c.GetProperty("withdraw_minsize").GetString()!,
                                                                CultureInfo.InvariantCulture);
                            await chain.Save();

                            Log.Trace(id, $"SaveChain({coin.asset})", chain.chainName);
                        }
                        else
                        {
                            await coin.Save();
                            Log.Trace(ID, "SaveCoin", coin.asset);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ID, "GetCoins 2", ex.Message);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ID, "GetCoins 1", ex.Message);
        }
    }
}
