#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs


FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["duendeadfs/duendeadfs.Server/duendeadfs.Server.csproj", "duendeadfs/duendeadfs.Server/"]
COPY ["duendeadfs/duendeadfs.client/duendeadfs.client.esproj", "duendeadfs/duendeadfs.client/"]
RUN dotnet restore "./duendeadfs/duendeadfs.Server/duendeadfs.Server.csproj"
COPY . .
WORKDIR "/src/duendeadfs/duendeadfs.Server"
RUN dotnet build "./duendeadfs.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./duendeadfs.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificate
COPY IdentityServer/certificate/aspnetapp.pfx /app/certificate

RUN chmod 777 /app

ENTRYPOINT ["dotnet", "duendeadfs.Server.dll"]
