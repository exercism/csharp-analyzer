FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env
WORKDIR /app

COPY analyze.sh /opt/analyzer/bin/

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -r linux-musl-x64 -o /opt/analyzer

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine
WORKDIR /opt/analyzer
COPY --from=build-env /opt/analyzer/ .
ENTRYPOINT ["sh", "/opt/analyzer/bin/analyze.sh"]
