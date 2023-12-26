#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Identity/Identity.API/Identity.API/Identity.API.csproj", "Services/Identity/Identity.API/Identity.API/"]
COPY ["Services/Tools/Tools.Controller/Tools.Controller.csproj", "Services/Tools/Tools.Controller/"]
COPY ["Services/Tools/Tools.GenericModels/Tools.GenericModels.csproj", "Services/Tools/Tools.GenericModels/"]
COPY ["Services/Identity/Identity.BLL/Identity.BLL.csproj", "Services/Identity/Identity.BLL/"]
COPY ["Services/Identity/Identity.Data/Identity.Data.csproj", "Services/Identity/Identity.Data/"]
COPY ["Services/Identity/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "Services/Identity/JwtAuthenticationManager/"]
RUN dotnet restore "./Services/Identity/Identity.API/Identity.API/./Identity.API.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.API/Identity.API"
RUN dotnet build "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.API.dll"]