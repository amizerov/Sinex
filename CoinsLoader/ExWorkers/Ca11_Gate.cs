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
        using HttpClient httpClient = new();
        var r = await httpClient.GetAsync($"{BASE_URL}{PREFIX}/spot/currencies");
        if (!r.IsSuccessStatusCode)
        {
            Log.Error(ID, "GetCoins", $"httpClient.SendAsync - {r.StatusCode}");
            return;
        }

        Log.Info(ID, "GetCoins()", "Start");

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

            string net = "";
            string net1 = p.GetProperty("chain").GetString() + "";

            net = ValidateChainCode(net1);

            coin.network = net;

            //coin.contract = p.GetProperty("contract").GetString() + "";
            coin.allowDeposit = !p.GetProperty("deposit_disabled").GetBoolean();
            coin.allowWithdraw = !p.GetProperty("withdraw_disabled").GetBoolean();

            Chain chain = new(coin.network);
            chain.name = net1;
            chain.name2 = $"[{ID}]";
            await chain.Save();

            coin.chainId = chain.id;
            await coin.Save();

            int cntChains = await GetChains(coin.id, coin.asset);

            Log.Info(ID, $"SaveCoin({coin.asset})", 
                $"{++cnt}/{cntCoins}/{cntChains}/{net}/{net1}");
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

                string net = "";
                string net1 = p.GetProperty("chain").GetString() + "";
                string net2 = p.GetProperty("name_en").GetString() + "";

                net = ValidateChainCode(net1);
                
                coinChain.chainName = net;

                coinChain.allowDeposit = p.GetProperty("is_deposit_disabled").GetInt32() == 0;
                coinChain.allowWithdraw = p.GetProperty("is_withdraw_disabled").GetInt32() == 0;
                coinChain.contractAddress = p.GetProperty("contract_address").GetString() + "";

                Chain chain = new(net);
                chain.name = net2;
                chain.name2 = $"[{ID}]";
                await chain.Save();

                coinChain.chainId = chain.id;
                await coinChain.Save();
                cntChains++;

                Log.Info(ID, "SaveChain", $"{net}/{net1}/{net2}");
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
