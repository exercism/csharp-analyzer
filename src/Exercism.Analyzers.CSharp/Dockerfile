FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-sdk
ARG EXERCISM_TOKEN
WORKDIR /app
COPY --from=build-env /app/out .
RUN ./binaries/linux-x64/exercism configure --token $EXERCISM_TOKEN
ENTRYPOINT ["dotnet", "Exercism.Analyzers.CSharp.dll"]