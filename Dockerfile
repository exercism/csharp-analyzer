FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /opt/analyzer

# Create analyze script
RUN mkdir /opt/analyzer/bin && \
    echo "dotnet /opt/analyzer/Exercism.Analyzers.CSharp.dll \$1 \$2" > /opt/analyzer/bin/analyze.sh && \
    chmod +x /opt/analyzer/bin/analyze.sh

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine
WORKDIR /opt/analyzer
COPY --from=build-env /opt/analyzer/ .

ENTRYPOINT ["sh", "/opt/analyzer/bin/analyze.sh"]