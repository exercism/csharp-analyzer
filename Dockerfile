FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine
WORKDIR /opt/analyzer
COPY --from=build-env /app/out/ .
COPY --from=build-env /app/bin/analyze.sh bin/analyze.sh

ENTRYPOINT ["dotnet", "Exercism.Analyzers.CSharp.dll"]