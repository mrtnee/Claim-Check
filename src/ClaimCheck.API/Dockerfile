FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first so the restore layer is cached separately from source changes
COPY src/ClaimCheck.API/ClaimCheck.API.csproj src/ClaimCheck.API/
COPY src/ClaimCheck.Domain/ClaimCheck.Domain.csproj src/ClaimCheck.Domain/
COPY src/ClaimCheck.Application/ClaimCheck.Application.csproj src/ClaimCheck.Application/
COPY src/ClaimCheck.Infrastructure/ClaimCheck.Infrastructure.csproj src/ClaimCheck.Infrastructure/
RUN dotnet restore src/ClaimCheck.API/ClaimCheck.API.csproj

COPY . .
RUN dotnet publish src/ClaimCheck.API/ClaimCheck.API.csproj -c Release --no-restore -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ClaimCheck.API.dll"]
