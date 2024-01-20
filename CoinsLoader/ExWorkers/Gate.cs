using amLogger;
using System.Net;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class Gate : AnExchange
{
    public const int ID = 11;
    public const string BASE_URL = "https://api.gateio.ws";
    public const string PREFIX = "/api/v4";

    public override async Task GetCoins()
    {
        Log.Info(ID, "GetCoins()", "Start");

        using HttpClient httpClient = new();
        var r = await httpClient.GetAsync($"{BASE_URL}{PREFIX}/spot/currencies");
        if (r.StatusCode == HttpStatusCode.OK)
        {
            var s = r.Content.ReadAsStringAsync().Result;
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            foreach (var p in e.EnumerateArray())
            {
                Coin coin = new();
                coin.exchId = ID;

                coin.asset = p.GetProperty("currency").GetString() + "";
                coin.network = p.GetProperty("chain").GetString() + "";
                //coin.contract = p.GetProperty("contract").GetString() + "";
                coin.allowDeposit = !p.GetProperty("deposit_disabled").GetBoolean();
                coin.allowWithdraw = !p.GetProperty("withdraw_disabled").GetBoolean();

                await coin.Save();

                await GetChains(coin.id, coin.asset);
            }
        }
        else
        {
            Log.Error(ID, $"GetProduct()", r.StatusCode.ToString());
        }
    }

    public async Task GetChains(int coinId, string asset)
    {
        using HttpClient httpClient = new();
        string uri = $"{BASE_URL}{PREFIX}/wallet/currency_chains?currency={asset}";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        var r = await httpClient.SendAsync(req);
        if (r.IsSuccessStatusCode)
        {
            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;

            foreach (var p in e.EnumerateArray())
            {
                try
                {
                    Chain chain = new();
                    chain.coinId = coinId;

                    chain.chainName = p.GetProperty("chain").GetString() + "";
                    chain.allowDeposit = p.GetProperty("is_deposit_disabled").GetInt32() == 0;
                    chain.allowWithdraw = p.GetProperty("is_withdraw_disabled").GetInt32() == 0;
                    chain.contractAddress = p.GetProperty("contract_address").GetString() + "";

                    await chain.Save();
                }
                catch (Exception ex)
                {
                    Log.Error(ID, $"GetChains({asset})", ex.Message);
                }
            }

            Thread.Sleep(200);
        }
    }
}
