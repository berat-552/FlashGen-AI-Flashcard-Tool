using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FlashGenDesktop;

public sealed class BackendClient
{
    private readonly HttpClient _client;

    public BackendClient()
    {
        _client = new HttpClient();
    }

    public async Task<string> GenerateFlashcardsAsync(string inputText, string userPrompt)
    {
        var payload = new
        {
            text = inputText,
            prompt = userPrompt
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("http://localhost:8000/generate", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);
        var resultText = doc.RootElement.GetProperty("result").GetString();

        return resultText?.Trim() ?? "";
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
