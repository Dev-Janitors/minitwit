FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY ./Backend ./Backend

RUN ls -a

WORKDIR ./Backend

RUN ls -a

# Restore as distinct layers
RUN dotnet restore Backend.sln



# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .
#ENV ASPNETCORE_URLS="https://+;http://+"
ENV ASPNETCORE_URLS="http://+"
#ENV ASPNETCORE_HTTPS_PORT=2223
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "Backend.dll"]