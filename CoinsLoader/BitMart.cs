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
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

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
                Coin cd = new();

                cd.exchId = ID;

                cd.asset = p.GetProperty("currency").GetString() + "";
                cd.longName = p.GetProperty("name").GetString() + "";
                cd.network = p.GetProperty("network").GetString() + "";
                cd.contract = p.GetProperty("contract_address").GetString() + "";

                await cd.Save();
            }
        }
    }
}
