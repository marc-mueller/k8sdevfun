ARG  IMAGE_NET_ASPNET_VERSION=latest
ARG  IMAGE_NET_SDK_VERSION=latest

FROM mcr.microsoft.com/dotnet/runtime:${IMAGE_NET_ASPNET_VERSION} AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:${IMAGE_NET_SDK_VERSION} AS build
WORKDIR /src
COPY ["*.props", "./"]
COPY ["DevFun.DbMigration.Cli/DevFun.DbMigration.Cli.csproj", "DevFun.DbMigration.Cli/"]
COPY ["DevFun.DB.Build/DevFun.DB.Build.csproj", "DevFun.DB.Build/"]
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.DbMigration.Cli/DevFun.DbMigration.Cli.csproj"
COPY . .
WORKDIR "/src/DevFun.DbMigration.Cli"
RUN dotnet build "DevFun.DbMigration.Cli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevFun.DbMigration.Cli.csproj" -c Release -o /app/publish
RUN cp /app/build/DevFun.DB.Build.dacpac /app/publish

FROM base AS final
WORKDIR /app

# Set environment variables for GC (enable server mode and dynamic adaptation [DATAS])
ENV DOTNET_gcServer=1
ENV DOTNET_GCDynamicAdaptationMode=1

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevFun.DbMigration.Cli.dll"]