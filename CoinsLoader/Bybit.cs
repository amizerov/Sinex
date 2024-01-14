using amLogger;
using MexcDotNet;
using RestSharp;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace CoinsLoader;

public class Bybit : AnExchange
{
    const int ID = 5;
    const string baseUrl = "https://api.bybit.com";
    string apiKey = CaSecrets.Secrets.BybitApiKey;
    string apiSecret = CaSecrets.Secrets.BybitApiSecret;

    public override async Task GetCoins()
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

        try
        {
            JsonDocument doc = JsonDocument.Parse(response.Content!);
            JsonElement ele = doc.RootElement;
            JsonElement data = ele.GetProperty("result");
            JsonElement rows = data.GetProperty("rows");

            foreach (var p in rows.EnumerateArray())
            {
                try
                {
                    Coin cd = new();

                    cd.exchId = ID;
                    cd.asset = p.GetProperty("coin").GetString() + "";
                    cd.longName = p.GetProperty("name").GetString() + "";

                    bool first = true;
                    var nets = p.GetProperty("chains");
                    foreach (var n in nets.EnumerateArray())
                    {
                        try
                        {
                            string fee = n.GetProperty("withdrawFee").GetString()!;
                            if (fee == "") fee = "0";

                            if (first)
                            {
                                cd.network = n.GetProperty("chainType").GetString() + "";
                                cd.allowDeposit = n.GetProperty("chainDeposit").GetString() == "1";
                                cd.allowWithdraw = n.GetProperty("chainWithdraw").GetString() == "1";
                                cd.contract = n.GetProperty("chain").GetString() + "";
                                cd.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                                await cd.Save();
                                
                                first = false;
                            }
                            Chain chain = new Chain();
                            chain.coinId = cd.id;
                            chain.chainName = n.GetProperty("chainType").GetString() + "";
                            chain.contractAddress = n.GetProperty("chain").GetString() + "";
                            chain.allowDeposit = n.GetProperty("chainDeposit").GetString() == "1";
                            chain.allowWithdraw = n.GetProperty("chainWithdraw").GetString() == "1";
                            chain.withdrawFee = float.Parse(fee, CultureInfo.InvariantCulture);
                            await chain.Save();
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
    }
}
