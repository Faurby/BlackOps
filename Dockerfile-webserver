FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY . /source
WORKDIR /source/Server

RUN dotnet restore

RUN dotnet publish --configuration Release --output /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "MiniTwit.Server.dll"]