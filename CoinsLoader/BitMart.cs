using amLogger;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CoinsLoader;

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
                foreach (var p in ccs.EnumerateArray())
                {
                    try { 
                        Coin cd = new();

                        cd.exchId = ID;

                        var name = cd.asset = p.GetProperty("currency").GetString() + "";
                        cd.longName = p.GetProperty("name").GetString() + "";
                        var netw = cd.network = p.GetProperty("network").GetString() + "";
                        var cont = cd.contract = p.GetProperty("contract_address").GetString() + "";
                        var adep = cd.allowDeposit = p.GetProperty("deposit_enabled").GetBoolean();
                        var awit = cd.allowWithdraw = p.GetProperty("withdraw_enabled").GetBoolean();

                        var fee = p.GetProperty("withdraw_minfee").GetString() + "";
                        if(fee.Length == 0) fee = "0";
                        cd.withdrawFee = double.Parse(fee, System.Globalization.NumberStyles.Currency);

                        cd.asset = name.Replace("-" + netw, "");

                        if (cd.Find() > 0 || cd.FindLong() > 0)
                        {
                            int id = cd.Find();

                            Chain ch = new();
                            ch.coinId = cd.id;
                            ch.chainName = netw;
                            ch.contractAddress = cont;
                            ch.allowDeposit = adep;
                            ch.allowWithdraw = awit;
                            ch.withdrawFee = cd.withdrawFee;
                            await ch.Save();

                            Log.Trace(id, $"SaveChain({cd.asset})", ch.chainName);
                        }
                        else
                        {
                            await cd.Save();
                            Log.Trace(ID, "SaveCoin", cd.asset);
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
