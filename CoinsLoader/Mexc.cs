using MexcDotNet;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CoinsLoader;

public class Mexc : AnExchange
{
    const int ID = 10;
    const string spotBaseUrl = "https://api.mexc.com";
    string apiKey = CaSecrets.Secrets.MexcApiKey;
    string apiSecret = CaSecrets.Secrets.MexcApiSecret;

    public override async Task GetCoins()
    {
        using HttpClient httpClient = new();
        var mexcService = new MexcService(apiKey, apiSecret, spotBaseUrl, httpClient);
        var res = await mexcService.SendSignedAsync("/api/v3/capital/config/getall", HttpMethod.Get);

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

