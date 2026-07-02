FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0.301-alpine3.23 AS build
ARG TARGETARCH

WORKDIR /app

# Copy csproj and restore as distinct layer
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj .
RUN dotnet restore -a $TARGETARCH

# Copy everything else and build
COPY src/Exercism.Analyzers.CSharp .
RUN dotnet publish -a $TARGETARCH --no-restore --self-contained true --output /opt/analyzer

# Build runtime image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/runtime-deps:10.0.9-alpine3.23 AS runtime
WORKDIR /opt/analyzer

COPY --from=build /opt/analyzer/ .
COPY --from=build /usr/local/bin/ /usr/local/bin/

COPY bin/run.sh /opt/analyzer/bin/

ENTRYPOINT ["sh", "/opt/analyzer/bin/run.sh"]
