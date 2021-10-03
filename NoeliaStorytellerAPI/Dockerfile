#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY ["NoeliaStorytellerAPI.csproj", "."]
RUN dotnet restore "./NoeliaStorytellerAPI.csproj"

COPY . .
RUN dotnet publish "NoeliaStorytellerAPI.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NoeliaStorytellerAPI.dll"]