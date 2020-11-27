FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore -r linux-musl-x64

# Copy everything else and build
COPY . ./
RUN dotnet publish -r linux-musl-x64 -c Release -o /opt/analyzer --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1-alpine
WORKDIR /opt/analyzer

COPY --from=build /opt/analyzer/ .
COPY --from=build /usr/local/bin/ /usr/local/bin/

COPY run.sh /opt/analyzer/bin/

ENTRYPOINT ["sh", "/opt/analyzer/bin/run.sh"]
