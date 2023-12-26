using System.Text.Json;

namespace CoinsLoader;

public class AscendEx : AnExchange
{
    public const int ID = 13;
    public const string BASE_URL = "https://ascendex.com";

    public override async Task GetCoins()
    {
        using HttpClient clt = new();

        string uri = $"{BASE_URL}/api/pro/v2/assets";
        var req = new HttpRequestMessage(HttpMethod.Get, uri);

        var r = await clt.SendAsync(req);
        if (r.IsSuccessStatusCode)
        {
            var s = await r.Content.ReadAsStringAsync();
            JsonDocument j = JsonDocument.Parse(s);
            JsonElement e = j.RootElement;
            JsonElement data = e.GetProperty("data");

            foreach (var p in data.EnumerateArray())
            {
                Coin cd = new();

                cd.exchId = ID;

                cd.asset = p.GetProperty("assetCode").GetString() + "";
                cd.longName = p.GetProperty("assetName").GetString() + "";

                var bc = p.GetProperty("blockChain");
                foreach(var b in bc.EnumerateArray())
                {
                    if (b.GetProperty("chainName").GetString() == "ERC20")
                    {
                        cd.network = b.GetProperty("chainName").GetString() + "";
                    }
                }

                await cd.Save();
            }
        }
    }
}

