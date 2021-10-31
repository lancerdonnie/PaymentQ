From mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY Payment.Api.csproj .
RUN dotnet restore "Payment.Api.csproj"
COPY . .
RUN dotnet publish "Payment.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Payment.Api.dll"]