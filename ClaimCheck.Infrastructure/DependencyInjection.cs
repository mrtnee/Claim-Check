using System.Text;
using ClaimCheck.Application.Claims;
using ClaimCheck.Infrastructure.Auth;
using ClaimCheck.Infrastructure.Claude;
using ClaimCheck.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ClaimCheck.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<ClaudeOptions>(
      configuration.GetSection(ClaudeOptions.SectionName));

    services.AddHttpClient<IClaudeClient, ClaudeClient>(client =>
    {
      client.BaseAddress = new Uri("https://api.anthropic.com/");
    });

    services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IClaimRepository, ClaimRepository>();

    services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
      options.Password.RequireNonAlphanumeric = false;
      options.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
    services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateLifetime = true
      };
    });

    services.AddAuthorization();

    return services;
  }
}
