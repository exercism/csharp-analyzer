FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine3.13-amd64 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore -r linux-musl-x64

# Copy everything else and build
COPY . ./
RUN dotnet publish -r linux-musl-x64 -c Release -o /opt/analyzer --no-restore -p:PublishReadyToRun=true

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine3.13-amd64 AS runtime
WORKDIR /opt/analyzer

COPY --from=build /opt/analyzer/ .
COPY --from=build /usr/local/bin/ /usr/local/bin/

COPY run.sh /opt/analyzer/bin/

ENTRYPOINT ["sh", "/opt/analyzer/bin/run.sh"]
