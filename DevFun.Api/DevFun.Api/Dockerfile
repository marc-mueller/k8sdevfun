ARG  IMAGE_NET_ASPNET_VERSION=latest
ARG  IMAGE_NET_SDK_VERSION=latest

FROM mcr.microsoft.com/dotnet/aspnet:${IMAGE_NET_ASPNET_VERSION} AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:${IMAGE_NET_SDK_VERSION} AS build
WORKDIR /src

COPY ["nuget.config", "./"]
COPY ["*.props", "./"]
COPY ["DevFun.Api/DevFun.Api.csproj", "DevFun.Api/"]
COPY ["DevFun.Logic/DevFun.Logic.csproj", "DevFun.Logic/"]
COPY ["DevFun.Common/DevFun.Common.csproj", "DevFun.Common/"]
COPY ["DevFun.Storage/DevFun.Storage.csproj", "DevFun.Storage/"]
COPY ["DevFun.DB.Build/DevFun.DB.Build.csproj", "DevFun.DB.Build/"]
COPY ["DevFun.Common.Model/DevFun.Common.Model.csproj", "DevFun.Common.Model/"]
COPY ["DevFun.Clients/DevFun.Clients.csproj", "DevFun.Clients/"]
COPY ["DevFun.Logic.Unit.Tests/DevFun.Logic.Unit.Tests.csproj", "DevFun.Logic.Unit.Tests/"]

RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Api/DevFun.Api.csproj"
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Logic.Unit.Tests/DevFun.Logic.Unit.Tests.csproj"
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.DB.Build/DevFun.DB.Build.csproj"
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Common.Model/DevFun.Common.Model.csproj"
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Clients/DevFun.Clients.csproj"
COPY . .

ENV IsCIBuild=true

RUN dotnet build "DevFun.Api/DevFun.Api.csproj" -c Release -o /app/build --no-restore
RUN dotnet build "DevFun.Logic.Unit.Tests/DevFun.Logic.Unit.Tests.csproj" -c Release --no-restore

FROM build AS client
ARG BUILDID=localbuild
LABEL client=${BUILDID}
WORKDIR /src
RUN dotnet build "DevFun.Common.Model/DevFun.Common.Model.csproj" -c Release --no-restore
RUN dotnet pack "DevFun.Common.Model/DevFun.Common.Model.csproj" -c Release -o /nupkgs --no-restore
RUN dotnet build "DevFun.Clients/DevFun.Clients.csproj" -c Release --no-restore
RUN dotnet pack "DevFun.Clients/DevFun.Clients.csproj" -c Release -o /nupkgs --no-restore

FROM build AS dacpac
ARG BUILDID=localbuild
LABEL dacpac=${BUILDID}
WORKDIR /src
RUN dotnet build "DevFun.DB.Build/DevFun.DB.Build.csproj" -c Release -o /dacpacs --no-restore

FROM build AS test
ARG BUILDID=localbuild
LABEL testresults=${BUILDID}
RUN dotnet test --no-build -c Release --results-directory /testresults --logger "trx;LogFileName=test_results.trx" /p:CollectCoverage=true /p:CoverletOutputFormat=json%2cCobertura /p:CoverletOutput=/testresults/coverage/ -p:MergeWith=/testresults/coverage/coverage.json DevFun.Logic.Unit.Tests/DevFun.Logic.Unit.Tests.csproj

FROM test AS publish
RUN dotnet publish "DevFun.Api/DevFun.Api.csproj" -c Release -o /app/publish  --no-restore

FROM base AS final
WORKDIR /app

# Set environment variables for GC (enable server mode and dynamic adaptation [DATAS])
ENV DOTNET_gcServer=1
ENV DOTNET_GCDynamicAdaptationMode=1

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevFun.Api.dll"]