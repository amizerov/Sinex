using amLogger;
using MexcDotNet;
using RestSharp;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace CoinsLoader.Worker;

public class Bybit : AnExchange
{
    const int ID = 5;
    const string baseUrl = "https://api.bybit.com";
    string apiKey = CaSecrets.Secrets.BybitApiKey;
    string apiSecret = CaSecrets.Secrets.BybitApiSecret;

    public override async Task GetCoins()
    {
        Log.Info(ID, "GetCoins", "Start");

        string coinsData = GetCoinsDataFromExchange();

        try
        {
            JsonDocument doc = JsonDocument.Parse(coinsData);
            JsonElement ele = doc.RootElement;
            JsonElement res = ele.GetProperty("result");
            JsonElement coins = res.GetProperty("rows");

            int cnt = 0;
            int cntCoins = coins.EnumerateArray().Count();
            foreach (var p in coins.EnumerateArray())
            {
                try
                {
                    Coin coin = new();

                    coin.exchId = ID;
                    coin.asset = p.GetProperty("coin").GetString() + "";
                    coin.longName = p.GetProperty("name").GetString() + "";

                    bool first = true;
                    var nets = p.GetProperty("chains");

                    int cntChains = nets.EnumerateArray().Count();
                    foreach (var n in nets.EnumerateArray())
                    {
                        try
                        {
                            string fee = n.GetProperty("withdrawFee").GetString()!;
                            if (fee == "") fee = "0";

                            string netName = n.GetProperty("chainType") + "";
                            string net = netName;
                            int i1 = net.IndexOf('(') + 1;
                            int i2 = net.IndexOf(')');
                            if (i1 > 1 && i2 > 0)
                                net = net.Substring(i1, i2 - i1);

                            net = ValidateChainCode(net);

                            Chain chain = new Chain(net);
                            chain.name = netName;
                            chain.name2 = $"[{ID}]";
                            await chain.Save();

                            if (first)
                            {
                                coin.chainId = chain.id;
                                coin.network = net;
                                coin.allowDeposit = n.GetProperty("chainDeposit").GetString() == "1";
                                coin.allowWithdraw = n.GetProperty("chainWithdraw").GetString() == "1";
                                coin.contract = n.GetProperty("chain").GetString() + "";
                                coin.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                                await coin.Save();

                                first = false;
                                Log.Info(ID, $"SaveCoin({coin.asset})", $"{++cnt}/{cntCoins}/{cntChains}");
                            }
                            CoinChain cChain = new CoinChain();
                            cChain.coinId = coin.id;
                            cChain.chainId = chain.id;
                            cChain.chainName = net;
                            cChain.contractAddress = n.GetProperty("chain").GetString() + "";
                            cChain.allowDeposit = n.GetProperty("chainDeposit").GetString() == "1";
                            cChain.allowWithdraw = n.GetProperty("chainWithdraw").GetString() == "1";
                            cChain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                            await cChain.Save();
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ID, "GetCoins 3", ex.Message);
                        }
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

    string GetCoinsDataFromExchange()
    {
        string responseString = "";
        try
        {
            var client = new RestClient($"{baseUrl}/v5/asset/coin/query-info");
            var request = new RestRequest();

            request.AddHeader("X-BAPI-API-KEY", apiKey);

            string recvWindow = "5000"; //server_time - recv_window <= timestamp < server_time + 1000
            StringBuilder queryStringBuilder = new StringBuilder();
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            queryStringBuilder.Append(timestamp).Append(apiKey).Append(recvWindow);
            string signature = SignatureHelper.Sign(queryStringBuilder.ToString(), apiSecret);

            request.AddHeader("X-BAPI-TIMESTAMP", timestamp);
            request.AddHeader("X-BAPI-RECV-WINDOW", recvWindow);
            request.AddHeader("X-BAPI-SIGN", signature);

            RestResponse response = client.Execute(request);
            responseString = response.Content!;
        }
        catch (Exception ex)
        {
            Log.Error(ID, "GetCoinsDataFromExchange", ex.Message);
        }

        return responseString;
    }
}
