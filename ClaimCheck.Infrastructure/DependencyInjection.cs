using ClaimCheck.Application.Claims;
using ClaimCheck.Infrastructure.Claude;
using ClaimCheck.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}
