FROM mcr.microsoft.com/dotnet/core/sdk:3.0.101-alpine3.10 AS build-env
WORKDIR /app

COPY analyze.sh /opt/analyzer/bin/

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -r linux-musl-x64 -o /opt/analyzer

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.0.1-alpine3.10
WORKDIR /opt/analyzer
COPY --from=build-env /opt/analyzer/ .
ENTRYPOINT ["sh", "/opt/analyzer/bin/analyze.sh"]
