#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/UserService/UserService.Api/UserService.Api.csproj", "Services/UserService/UserService.Api/"]
COPY ["Services/Tools/Tools.Controller/Tools.Controller.csproj", "Services/Tools/Tools.Controller/"]
COPY ["Services/Tools/Tools.GenericModels/Tools.GenericModels.csproj", "Services/Tools/Tools.GenericModels/"]
COPY ["Services/UserService/UserService.BLL/UserService.BLL.csproj", "Services/UserService/UserService.BLL/"]
COPY ["Services/UserService/UserService.Data/UserService.Data.csproj", "Services/UserService/UserService.Data/"]
RUN dotnet restore "./Services/UserService/UserService.Api/./UserService.Api.csproj"
COPY . .
WORKDIR "/src/Services/UserService/UserService.Api"
RUN dotnet build "./UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Api.dll"]