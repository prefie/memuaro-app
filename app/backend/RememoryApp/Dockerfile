FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Memuaro.WebApi/Memuaro.WebApi.csproj", "Memuaro.WebApi/"]
RUN dotnet restore "Memuaro.WebApi/Memuaro.WebApi.csproj"
COPY . .
WORKDIR "/src/Memuaro.WebApi"
RUN dotnet build "Memuaro.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Memuaro.WebApi.csproj" -c Release -o /app/publish

RUN apt-get update \
    && apt-get install -y --allow-unauthenticated wkhtmltopdf \
    && rm -rf /var/lib/apt/lists/*

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Memuaro.WebApi.dll"]
