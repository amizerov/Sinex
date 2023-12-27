using System.Text.Json;

namespace CoinsLoader;

public class Mexc : AnExchange
{
    const int ID = 10;
    const string BASE_URL = "https://api.mexc.com";
    string API_KEY = CaSecrets.Secrets.MexcApiKey;
    string SEC_KEY = CaSecrets.Secrets.MexcApiSecret;

    public override async Task GetCoins()
    {
        using HttpClient clt = new();

        string uri = $"{BASE_URL}/api/v3/capital/config/getall";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        req.Headers.Add("ApiKey", API_KEY);
        req.Headers.Add("secretKey", SEC_KEY);

        var r = await clt.SendAsync(req);
        if (r.IsSuccessStatusCode)
        {
            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            foreach (var p in e.EnumerateArray())
            {
                Coin cd = new();

                cd.exchId = ID;
                cd.asset = p.GetProperty("coin").GetString() + "";
                cd.longName = p.GetProperty("name").GetString() + "";

                var nets = p.GetProperty("networkList");
                foreach (var n in nets.EnumerateArray()) 
                { 
                    cd.network = p.GetProperty("network").GetString() + "";
                    cd.contract = p.GetProperty("contract_address").GetString() + "";
                }
                await cd.Save();
            }
        }
    }
}

