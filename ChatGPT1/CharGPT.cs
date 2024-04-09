using CaSecrets;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ChatGPT1;

public class CharGPT
{
    public static async Task<string> GetAnswer(string question)
    {
        string tmp = @"{
            ""model"": ""gpt-3.5-turbo"",
            ""messages"": [
                {
                    ""role"": ""system"",
                    ""content"": ""You are a helpful assistant.""
                },
                {
                    ""role"": ""user"",
                    ""content"": ""{0}""
                }
            ]
        }";
        string jsonContent = tmp.Replace("{0}", question);

        var httpClient = new HttpClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.openai.com/v1/chat/completions"),
            Headers =
        {
            { HttpRequestHeader.ContentType.ToString(), "application/json" },
            { HttpRequestHeader.Authorization.ToString(), "Bearer " + Secrets.OPENAI_API_KEY},
        },
            Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JObject.Parse(responseContent);

            var choices = parsedResponse["choices"];
            if(choices == null) return "*** 1";
            if(choices.Count() == 0) return "*** 2";
            if (choices[0] == null) return "*** 3";
            var message = choices![0]!["message"];
            if (message == null) return "*** 4";
            if (message["content"] == null) return "*** 5";
            var answer = message["content"]!.ToString();

            SaveQAToDb(question, answer);
            return answer;
        }

        return "***";
    }
    static void SaveQAToDb(string q, string a)
    {
        Db.SaveQAToDb(q, a);
    }
}
