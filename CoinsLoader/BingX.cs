using MexcDotNet;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;

namespace CoinsLoader;

public class BingX : AnExchange
{
    const int ID = 15;
    const string spotBaseUrl = "https://open-api.bingx.com";
    string apiKey = "C7rdd7H9e58tKhopY5pkMZGgzAO7xm6sYjqvyVF2DQ475xOke1RimIGXI2aFUUhE598QMbuaELz2axeWYd4Q";// CaSecrets.Secrets.MexcApiKey;
    string apiSecret = "Cjr6TK7Ve9lsfX5oqlHqiRA4be4ehOUsQa9fktrT5xcNr2ggbfFJbhP4qwyZ0UdNpXU4YKKnIvjrS9utAw";// CaSecrets.Secrets.MexcApiSecret;

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();
        var mexcService = new BingXService(apiKey, apiSecret, spotBaseUrl, httpClient);
        var res = await mexcService.SendSignedAsync("/openApi/wallets/v1/capital/config/getall", HttpMethod.Get);

        JsonDocument doc = JsonDocument.Parse(res);
        JsonElement ele = doc.RootElement;

        foreach (var p in ele.EnumerateArray())
        {
            Coin cd = new();

            cd.exchId = ID;
            cd.asset = p.GetProperty("coin").GetString() + "";
            cd.longName = p.GetProperty("name").GetString() + "";

            var nets = p.GetProperty("networkList");
            foreach (var n in nets.EnumerateArray())
            {
                cd.network = n.GetProperty("network").GetString() + "";
                cd.contract = n.GetProperty("contract").GetString() + "";
            }
            await cd.Save();
        }
    }
    /// <summary>Signs the given source with the given key using HMAC SHA256.</summary>
    public static string Sign(string source, string key)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (HMACSHA256 hmacsha256 = new HMACSHA256(keyBytes))
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(source);

            byte[] hash = hmacsha256.ComputeHash(sourceBytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
