using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClaimCheck.Web.Models;

namespace ClaimCheck.Web.Services;

public sealed class ClaimService : IClaimService
{
  private readonly HttpClient _http;
  private readonly IAuthService _authService;

  public ClaimService(HttpClient http, IAuthService authService)
  {
    _http = http;
    _authService = authService;
  }

  public async Task<ClaimResultModel> AnalyzeAsync(string claimText, CancellationToken ct = default)
  {
    var token = await _authService.GetTokenAsync();

    using var request = new HttpRequestMessage(HttpMethod.Post, "api/claims");
    request.Content = JsonContent.Create(new { ClaimText = claimText });
    if (token is not null)
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var response = await _http.SendAsync(request, ct);
    response.EnsureSuccessStatusCode();

    return await response.Content
      .ReadFromJsonAsync<ClaimResultModel>(cancellationToken: ct)
      ?? throw new InvalidOperationException("Empty response from API.");
  }
}
