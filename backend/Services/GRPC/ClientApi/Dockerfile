#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/GRPC/ClientApi/ClientApi.csproj", "Services/GRPC/ClientApi/"]
COPY ["Services/GRPC/Protos/Protos.csproj", "Services/GRPC/Protos/"]
RUN dotnet restore "Services/GRPC/ClientApi/ClientApi.csproj"
COPY . .
WORKDIR "/src/Services/GRPC/ClientApi"
RUN dotnet build "ClientApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClientApi.dll"]