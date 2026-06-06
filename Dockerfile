FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

WORKDIR /app/stage

# Copy csproj and restore as distinct layers
COPY . .

RUN dotnet restore
RUN dotnet publish Api/Api.csproj -c Release -o out

# Download shoutcast directory data at build time
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS data-fetch
RUN apk add --no-cache curl jq
RUN DOWNLOAD_URL=$(curl -s -H "User-Agent: stream-subscription-api" \
      https://api.github.com/repos/amir734jj/shoutcast-directory-crawler/releases/latest \
      | jq -r '.assets[] | select(.name == "shoutcast-directory.json") | .browser_download_url') \
    && curl -sL -o /shoutcast-directory.json "$DOWNLOAD_URL"

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

WORKDIR /app/build
COPY --from=build-env "/app/stage/out" .
COPY --from=data-fetch /shoutcast-directory.json .
ENTRYPOINT ["dotnet", "Api.dll"]
