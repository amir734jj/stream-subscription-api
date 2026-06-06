FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env

WORKDIR /app/stage

# Copy csproj and restore as distinct layers
COPY . .

RUN dotnet restore
RUN dotnet publish Api/Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

WORKDIR /app/build
COPY --from=build-env "/app/stage/out" .
ENTRYPOINT ["dotnet", "Api.dll"]
