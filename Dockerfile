FROM mcr.microsoft.com/dotnet/sdk:7.0.306-alpine3.18-amd64 AS build
WORKDIR /app

# Copy csproj and restore as distinct layer
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj .
RUN dotnet restore -r linux-musl-x64

# Copy everything else and build
COPY src/Exercism.Analyzers.CSharp .
RUN dotnet publish -r linux-musl-x64 -c Release --self-contained true -o /opt/analyzer

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0.9-alpine3.18-amd64 AS runtime
WORKDIR /opt/analyzer

COPY --from=build /opt/analyzer/ .
COPY --from=build /usr/local/bin/ /usr/local/bin/

COPY bin/run.sh /opt/analyzer/bin/

ENTRYPOINT ["sh", "/opt/analyzer/bin/run.sh"]
