using ClaimCheck.Application.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ClaimCheck.API.Claims;

[ApiController]
[Route("api/[controller]")]
public sealed class ClaimsController : ControllerBase
{
    private readonly AnalyzeClaimHandler _handler;

    public ClaimsController(AnalyzeClaimHandler handler) => _handler = handler;

    [HttpPost]
    public async Task<ActionResult<ClaimResponse>> Post(
        [FromBody] ClaimRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.ClaimText))
            return BadRequest("Claim text is required.");

        var result = await _handler.HandleAsync(new AnalyzeClaimCommand(request.ClaimText), ct);

        return Ok(new ClaimResponse(
            result.Techniques,
            result.CounterArguments,
            result.TruthfulnessScore,
            result.Explanation));
    }
}
