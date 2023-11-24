using System.Text.Json;

namespace CaSecrets;

public static class Secrets
{
    static List<string> _keys = new();
    public static string BinanceApiKey
    {
        get
        {
            _keys = ReadKeysFromFile();
            if (_keys.Count >= 2)
                return _keys[0];
            else
                return "";
        }
    }
    public static string BinanceApiSecret
    {
        get
        {
            if (_keys.Count >= 2)
                return _keys[1];
            else
                return "";
        }
    }
    public static string OKXApiKey = "";
    public static string OKXApiSecret = "";


    static List<string> ReadKeysFromFile()
    {
        /*** формат файла BinanceApiKey.txt ******>
         {
           "apiKey":"xxxxxxxxxxxxxxxxxxxxxx",
           "secretKey":"xxxxxxxxxxxxxxxxxxxxxxx",
           "comment":"SinexTradingBot_BinaApiKey21042023"
         }
         *********************************************/
        List<string> keys = new();
        string path = "D:\\Projects\\Common\\Secrets\\BinanceApiKey.txt";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var kks = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (kks == null)
            {
                throw new Exception("File with Api Keys is bad");
            }
            foreach (var key in kks.Keys)
                keys.Add(kks[key]);
        }
        else
            throw new Exception("File with Api Keys is not found");

        return keys;
    }

    static public string SqlConnectionString
    {
        get
        {
            string cs = "";
            string path = "D:\\Projects\\Common\\Secrets\\SqlConnectionStringForCaProgerX.txt";
            if (File.Exists(path))
            {
                cs = File.ReadAllText(path);
            }
            else
                throw new Exception("File with Sql Connection is not found");

            return cs;
        }
    }
}