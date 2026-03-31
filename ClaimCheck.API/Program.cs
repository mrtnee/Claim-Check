using ClaimCheck.Application.Claims;
using ClaimCheck.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddPolicy("BlazorClient", policy => policy
        .WithOrigins("http://localhost:5138", "https://localhost:7180")
        .AllowAnyMethod()
        .AllowAnyHeader()));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<AnalyzeClaimHandler>();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.UseCors("BlazorClient");
app.MapControllers();

app.Run();
