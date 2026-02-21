FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY NotificationService.API/NotificationService.API.csproj .
RUN dotnet restore
COPY NotificationService.API/ .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
