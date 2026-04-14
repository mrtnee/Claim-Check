using ClaimCheck.Application.Claims;
using ClaimCheck.Infrastructure;
using ClaimCheck.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
  .GetSection("Cors:AllowedOrigins")
  .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
  options.AddPolicy("BlazorClient", policy => policy
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader()));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<AnalyzeClaimHandler>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Runs migrations on startup. If scaling to multiple API instances, replace this
// with a CI/CD migration step to avoid race conditions on simultaneous deploys.
using (var scope = app.Services.CreateScope())
{
  await scope.ServiceProvider
    .GetRequiredService<AppDbContext>()
    .Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("BlazorClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
