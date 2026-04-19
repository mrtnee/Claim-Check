FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so the restore layer is cached separately from source changes
COPY ClaimCheck.API/ClaimCheck.API.csproj ClaimCheck.API/
COPY ClaimCheck.Domain/ClaimCheck.Domain.csproj ClaimCheck.Domain/
COPY ClaimCheck.Application/ClaimCheck.Application.csproj ClaimCheck.Application/
COPY ClaimCheck.Infrastructure/ClaimCheck.Infrastructure.csproj ClaimCheck.Infrastructure/
RUN dotnet restore ClaimCheck.API/ClaimCheck.API.csproj

COPY . .
RUN dotnet publish ClaimCheck.API/ClaimCheck.API.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ClaimCheck.API.dll"]
