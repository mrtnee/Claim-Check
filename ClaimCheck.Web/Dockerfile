FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

ARG API_URL
RUN echo "{\"ApiBaseUrl\": \"${API_URL}\"}" > wwwroot/appsettings.Production.json

RUN dotnet publish ClaimCheck.Web.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
