#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY "Estoque.Api/Estoque.Api.csproj" "Estoque.Api/"
COPY "Estoque.Negocio/Estoque.Negocio.csproj" "Estoque.Negocio/"
COPY "Estoque.Dados/Estoque.Dados.csproj" "Estoque.Dados/"
RUN dotnet restore "Estoque.Api/Estoque.Api.csproj"

COPY . ./
RUN dotnet publish "Estoque.Api/Estoque.Api.csproj" -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
COPY --from=build "/out/" .
ENTRYPOINT ["dotnet", "Estoque.Api.dll"]