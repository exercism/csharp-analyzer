FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet add package ILLink.Tasks -v 0.1.5-preview-1841731 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
RUN dotnet publish -c Release -r linux-musl-x64 -o /opt/analyzer

# Create analyze script
RUN mkdir /opt/analyzer/bin && \
    echo "/opt/analyzer/Exercism.Analyzers.CSharp \$1 \$2" > /opt/analyzer/bin/analyze.sh && \
    chmod +x /opt/analyzer/bin/analyze.sh

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:2.2-alpine
WORKDIR /opt/analyzer
COPY --from=build-env /opt/analyzer/ .
ENTRYPOINT ["sh", "/opt/analyzer/bin/analyze.sh"]