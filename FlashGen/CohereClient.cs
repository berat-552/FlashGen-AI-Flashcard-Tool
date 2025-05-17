using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FlashGen;

public sealed class CohereClient
{
    private readonly HttpClient _client;
    private readonly string _apiKey;

    public CohereClient(string apiKey)
    {
        _apiKey = apiKey;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", _apiKey);
    }

    public async Task<string> GenerateFlashcardsAsync(string inputText, string userPrompt)
    {
        string basePrompt =
            "Convert the following text into flashcards in Q&A format.";

        string fullPrompt = string.IsNullOrWhiteSpace(userPrompt)
            ? $"{basePrompt} Use clear, concise language:\n\n{inputText}"
            : $"{basePrompt} {userPrompt.Trim()}\n\n{inputText}";

        var requestBody = new
        {
            model = "command-r-plus",
            prompt = fullPrompt,
            max_tokens = 500,
            temperature = 0.5
        };

        var content =
            new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

        var response =
            await _client.PostAsync(
                "https://api.cohere.ai/v1/generate", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer
            .Deserialize<CohereResponse>(responseJson);

        return result?.generations?[0]?.text?.Trim() ?? "";
    }

    private class CohereResponse
    {
        public List<Generation> generations { get; set; }
    }

    private class Generation
    {
        public string text { get; set; }
    }
}
