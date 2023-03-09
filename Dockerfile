FROM mcr.microsoft.com/dotnet/sdk:7.0.101-alpine3.16-amd64 AS build
WORKDIR /app

# Copy csproj and restore as distinct layer
COPY src/Exercism.Analyzers.CSharp/*.csproj .
RUN dotnet restore -r linux-musl-x64

# Copy everything else and build
COPY src/Exercism.Analyzers.CSharp .
RUN dotnet publish -r linux-musl-x64 -c Release -o /opt/analyzer --no-restore --self-contained true

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0.1-alpine3.16-amd64 AS runtime
WORKDIR /opt/analyzer

COPY --from=build /opt/analyzer/ .
COPY --from=build /usr/local/bin/ /usr/local/bin/

COPY bin/run.sh /opt/analyzer/bin/

ENTRYPOINT ["sh", "/opt/analyzer/bin/run.sh"]
