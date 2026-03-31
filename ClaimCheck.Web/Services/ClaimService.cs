using System.Net.Http.Json;
using ClaimCheck.Web.Models;

namespace ClaimCheck.Web.Services;

public sealed class ClaimService : IClaimService
{
  private readonly HttpClient _http;

  public ClaimService(HttpClient http) => _http = http;

  public async Task<ClaimResultModel> AnalyzeAsync(string claimText, CancellationToken ct = default)
  {
    var response = await _http.PostAsJsonAsync(
      "api/claims",
      new { ClaimText = claimText },
      ct);

    response.EnsureSuccessStatusCode();

    return await response.Content
      .ReadFromJsonAsync<ClaimResultModel>(cancellationToken: ct)
      ?? throw new InvalidOperationException("Empty response from API.");
  }
}
