using CaSecrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatGPT1;

public class AmChat
{
    static string questionTemplate = @"{
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

    public static async Task<string> GetAnswer(string question, int uid = 0)
    {
        //question = CleanseString(question);
        string jsonContent = questionTemplate.Replace("{0}", question);

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
        string answer = await GetClearAnswerFromResponse(response);

        Db.SaveQA(question, answer, uid);
        return answer;
    }

    static async Task<string> GetClearAnswerFromResponse(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var parsedResponse = JObject.Parse(responseContent);

            var choices = parsedResponse["choices"];
            if (choices == null) return "*** 1";
            if (choices.Count() == 0) return "*** 2";
            if (choices[0] == null) return "*** 3";
            var message = choices![0]!["message"];
            if (message == null) return "*** 4";
            if (message["content"] == null) return "*** 5";
            var answer = message["content"]!.ToString();

            return answer;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return $"Error: {response.StatusCode}, Content: {errorContent}";
    }
    static string CleanseString(string input)
    {
        return JsonConvert.ToString(input);
    }
}
