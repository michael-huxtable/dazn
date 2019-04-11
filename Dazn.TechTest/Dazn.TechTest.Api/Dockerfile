FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Dazn.TechTest.Api/Dazn.TechTest.Api.csproj", "Dazn.TechTest.Api/"]
RUN dotnet restore "Dazn.TechTest.Api/Dazn.TechTest.Api.csproj"
COPY . .
WORKDIR "/src/Dazn.TechTest.Api"
RUN dotnet build "Dazn.TechTest.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dazn.TechTest.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dazn.TechTest.Api.dll"]