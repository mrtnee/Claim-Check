using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClaimCheck.Application.Claims;
using ClaimCheck.Domain.Claims;
using Microsoft.Extensions.Options;

namespace ClaimCheck.Infrastructure.Claude;

public sealed class ClaudeClient : IClaudeClient
{
  private readonly HttpClient _http;
  private readonly ClaudeOptions _options;

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNameCaseInsensitive = true
  };

  private const string SystemPrompt =
      "You are a propaganda analysis expert. When given a claim, analyze it for propaganda " +
      "techniques, logical fallacies, and factual accuracy. Respond ONLY with a single JSON " +
      "object — no markdown, no code fences, no extra text.\n\n" +
      "Required fields:\n" +
      "- \"techniques\": string[] — propaganda techniques / logical fallacies found (empty array if none)\n" +
      "- \"counterArguments\": string[] — concise counter-arguments or corrective evidence (at least one)\n" +
      "- \"truthfulnessScore\": int 1–10 (1 = completely false, 10 = completely true)\n" +
      "- \"explanation\": string — neutral 2-4 sentence analysis\n\n" +
      "Example: {\"techniques\":[\"appeal to fear\"],\"counterArguments\":[\"Studies show...\"],\"truthfulnessScore\":3,\"explanation\":\"...\"}";

  public ClaudeClient(HttpClient http, IOptions<ClaudeOptions> options)
  {
    _http = http;
    _options = options.Value;
  }

  public async Task<ClaimAnalysisResult> AnalyzeAsync(ClaimText claim, CancellationToken ct = default)
  {
    var requestBody = new
    {
      model = _options.Model,
      max_tokens = _options.MaxTokens,
      system = SystemPrompt,
      messages = new[]
        {
                new { role = "user", content = claim.Value }
            }
    };

    using var request = new HttpRequestMessage(HttpMethod.Post, "v1/messages");
    request.Headers.Add("x-api-key", _options.ApiKey);
    request.Headers.Add("anthropic-version", "2023-06-01");
    request.Content = JsonContent.Create(requestBody);

    using var response = await _http.SendAsync(request, ct);
    response.EnsureSuccessStatusCode();

    var envelope = await response.Content
        .ReadFromJsonAsync<AnthropicResponse>(JsonOptions, ct)
        ?? throw new InvalidOperationException("Empty response from Anthropic API.");

    var text = envelope.Content.First(c => c.Type == "text").Text;

    return JsonSerializer.Deserialize<ClaimAnalysisResult>(text, JsonOptions)
        ?? throw new InvalidOperationException("Failed to parse analysis result from Claude.");
  }

  private sealed record AnthropicResponse(
      [property: JsonPropertyName("content")] AnthropicContentBlock[] Content
  );

  private sealed record AnthropicContentBlock(
      [property: JsonPropertyName("type")] string Type,
      [property: JsonPropertyName("text")] string Text
  );
}
