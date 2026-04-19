FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ClaimCheck.Web/ClaimCheck.Web.csproj ClaimCheck.Web/
RUN dotnet restore ClaimCheck.Web/ClaimCheck.Web.csproj

COPY . .
RUN dotnet publish ClaimCheck.Web/ClaimCheck.Web.csproj -c Release --no-restore -o /app/publish

FROM nginx:alpine AS final
COPY ClaimCheck.Web/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
