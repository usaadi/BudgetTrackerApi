#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BudgetTrackerApi/BudgetTrackerApi.csproj", "BudgetTrackerApi/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "BudgetTrackerApi/BudgetTrackerApi.csproj"
COPY . .
WORKDIR "/src/BudgetTrackerApi"
RUN dotnet build "BudgetTrackerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BudgetTrackerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetTrackerApi.dll"]
