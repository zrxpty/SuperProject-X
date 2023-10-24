#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/GRPC/OtherServer/OtherServer.csproj", "Services/GRPC/OtherServer/"]
COPY ["Services/Identity/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "Services/Identity/JwtAuthenticationManager/"]
COPY ["Services/GRPC/Protos/Protos.csproj", "Services/GRPC/Protos/"]
RUN dotnet restore "Services/GRPC/OtherServer/OtherServer.csproj"
COPY . .
WORKDIR "/src/Services/GRPC/OtherServer"
RUN dotnet build "OtherServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OtherServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OtherServer.dll"]