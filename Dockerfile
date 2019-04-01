FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /opt

# Copy csproj and restore as distinct layers
COPY src/Exercism.Analyzers.CSharp/Exercism.Analyzers.CSharp.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o analyzer

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine
WORKDIR /opt
COPY --from=build-env /opt/analyzer .

ENTRYPOINT ["dotnet", "Exercism.Analyzers.CSharp.dll"]