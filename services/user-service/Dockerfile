FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY UserService.API/UserService.API.csproj .
RUN dotnet restore
COPY UserService.API/ .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "UserService.API.dll"]
