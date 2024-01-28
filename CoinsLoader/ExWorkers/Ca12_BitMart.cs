using amLogger;
using System.Globalization;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class BitMart : AnExchange
{
    public const int ID = 12;
    public const string BASE_URL = "https://api-cloud.bitmart.com";

    // https://developer-pro.bitmart.com/en/spot/#get-currencies
    public override async Task GetCoins()
    {
        using HttpClient clt = new();

        string uri = $"{BASE_URL}/account/v1/currencies";
        //string uri = $"{BASE_URL}/spot/v1/currencies";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        try
        {
            var r = await clt.SendAsync(req);
            if (!r.IsSuccessStatusCode)
            {
                Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
                return;
            }

            Log.Info(ID, "GetCoins", "Start");

            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            JsonElement data = e.GetProperty("data");
            JsonElement coins = data.GetProperty("currencies");

            int cnt = 0;
            int cntCoins = coins.EnumerateArray().Count();
            string asset = "";
            foreach (var c in coins.EnumerateArray())
            {
                try
                {
                    Coin coin = new(ID);

                    var name = coin.asset = c.GetProperty("currency").GetString() + "";
                    coin.longName = c.GetProperty("name").GetString() + "";
                    var netName = c.GetProperty("network").GetString() + "";
                    var cont = coin.contract = c.GetProperty("contract_address").GetString() + "";
                    var adep = coin.allowDeposit = c.GetProperty("deposit_enabled").GetBoolean();
                    var awit = coin.allowWithdraw = c.GetProperty("withdraw_enabled").GetBoolean();

                    string netCode = ValidateChainCode(netName);
                    coin.network = netCode;

                    var fee = c.GetProperty("withdraw_minfee").GetString() + "";
                    if (fee.Length == 0) fee = "0";
                    coin.withdrawFee = double.Parse(fee, NumberStyles.Currency);

                    coin.asset = name.Replace("-" + netName, "")
                                        .Replace("_" + netName, "");

                    Chain chain = new(netCode);
                    chain.name = netName;
                    chain.name2 = $"[{ID}]";
                    int chainId = await chain.Save();

                    // если предыдущий был таким же, то просто новая сеть
                    if (asset != coin.asset)
                    {
                        coin.chainId = chainId;

                        await coin.Save();
                        asset = coin.asset;

                        Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}");
                    }

                    int id = coin.Find();
                    if (id > 0)
                    {
                        CoinChain coinChain = new();
                        coinChain.coinId = id;
                        coinChain.chainId = chainId;
                        coinChain.chainName = netCode;
                        coinChain.contractAddress = cont;
                        coinChain.allowDeposit = adep;
                        coinChain.allowWithdraw = awit;
                        coinChain.withdrawFee = coin.withdrawFee;
                        coinChain.minWithdrawal =
                            double.Parse(c.GetProperty("withdraw_minsize").GetString()!,
                                                            CultureInfo.InvariantCulture);
                        await coinChain.Save();
                        Log.Trace(ID, $"More nets for {coin.asset}", netName);
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

        Log.Info(ID, "GetCoins", "Done");
    }
}
