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
        if (!r.IsSuccessStatusCode)
        {
            Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
            return;
        }

        var s = r.Content.ReadAsStringAsync().Result;
        JsonDocument j = JsonDocument.Parse(s);
        JsonElement e = j.RootElement;
        int cnt = 0;
        int cntCoins = e.EnumerateArray().Count();
        foreach (var p in e.EnumerateArray())
        {
            Coin coin = new();
            coin.exchId = ID;

            coin.asset = p.GetProperty("currency").GetString() + "";
            coin.network = p.GetProperty("chain").GetString() + "";
            //coin.contract = p.GetProperty("contract").GetString() + "";
            coin.allowDeposit = !p.GetProperty("deposit_disabled").GetBoolean();
            coin.allowWithdraw = !p.GetProperty("withdraw_disabled").GetBoolean();

            Chain chain = new(coin.network);
            await chain.Save();

            coin.chainId = chain.id;

            await coin.Save();
            cnt++;

            int cntChains = await GetChains(coin.id, coin.asset);

            Log.Info(ID, $"SaveCoin({coin.asset})", $"{cnt}/{cntCoins}/{cntChains}");
        }

        Log.Info(ID, "GetCoins()", "End");
    }

    public async Task<int> GetChains(int coinId, string asset)
    {
        int cntChains = 0;
        using HttpClient httpClient = new();
        string uri = $"{BASE_URL}{PREFIX}/wallet/currency_chains?currency={asset}";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        var r = await httpClient.SendAsync(req);
        if (!r.IsSuccessStatusCode)
        {
            Log.Error(ID, $"GetChains({asset})", $"httpClient.SendAsync - {r.StatusCode}");
            return cntChains;
        }

        var s = await r.Content.ReadAsStringAsync();
        JsonDocument j = JsonDocument.Parse(s);
        JsonElement e = j.RootElement;

        foreach (var p in e.EnumerateArray())
        {
            try
            {
                CoinChain coinChain = new();
                coinChain.coinId = coinId;

                coinChain.chainName = p.GetProperty("chain").GetString() + "";
                coinChain.allowDeposit = p.GetProperty("is_deposit_disabled").GetInt32() == 0;
                coinChain.allowWithdraw = p.GetProperty("is_withdraw_disabled").GetInt32() == 0;
                coinChain.contractAddress = p.GetProperty("contract_address").GetString() + "";

                Chain chain = new(coinChain.chainName);
                await chain.Save();

                coinChain.chainId = chain.id;
                await coinChain.Save();
                cntChains++;
            }
            catch (Exception ex)
            {
                Log.Error(ID, $"GetChains({asset})", ex.Message);
            }
        }

        Thread.Sleep(200);
        return cntChains;
    }
}
