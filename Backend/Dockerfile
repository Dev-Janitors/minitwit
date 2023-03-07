FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
# COPY ${backendCopySrc} ./Backend
COPY ./Backend ./Backend


WORKDIR ./Backend

RUN --mount=type=secret,id=AUTH0_AUDIENCE \
    export AUTH0_AUDIENCE=$(cat /run/secrets/AUTH0_AUDIENCE) && \
    echo "AUTH0_AUDIENCE=$AUTH0_AUDIENCE" >> .env

# Restore as distinct layers
RUN dotnet restore Backend.sln



# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/Backend/out .
#ENV ASPNETCORE_URLS="https://+;http://+"
ENV ASPNETCORE_URLS="http://+"
#ENV ASPNETCORE_HTTPS_PORT=2223
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Backend.dll"]