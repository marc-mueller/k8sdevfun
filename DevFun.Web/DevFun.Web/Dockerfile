ARG  IMAGE_NET_ASPNET_VERSION=latest
ARG  IMAGE_NET_SDK_VERSION=latest

FROM mcr.microsoft.com/dotnet/aspnet:${IMAGE_NET_ASPNET_VERSION} AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:${IMAGE_NET_SDK_VERSION} AS build
WORKDIR /src
COPY ["nuget.config", "./"]
COPY ["*.props", "./"]
COPY ["DevFun.Web/DevFun.Web.csproj", "DevFun.Web/"]
COPY ["DevFun.Web.Unit.Tests/DevFun.Web.Unit.Tests.csproj", "DevFun.Web.Unit.Tests/"]

RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Web/DevFun.Web.csproj"
RUN --mount=type=secret,id=nugetconfig \
    dotnet restore --configfile "/run/secrets/nugetconfig" "DevFun.Web.Unit.Tests/DevFun.Web.Unit.Tests.csproj"
COPY . .

RUN dotnet build "DevFun.Web/DevFun.Web.csproj" -c Release -o /app/build --no-restore
RUN dotnet build "DevFun.Web.Unit.Tests/DevFun.Web.Unit.Tests.csproj" -c Release --no-restore

FROM build AS test
ARG BUILDID=localbuild
LABEL testresults=${BUILDID}
RUN dotnet test --no-build -c Release --results-directory /testresults --logger "trx;LogFileName=test_results.trx" /p:CollectCoverage=true /p:CoverletOutputFormat=json%2cCobertura /p:CoverletOutput=/testresults/coverage/ -p:MergeWith=/testresults/coverage/coverage.json DevFun.Web.Unit.Tests/DevFun.Web.Unit.Tests.csproj

FROM test AS publish
RUN dotnet publish "DevFun.Web/DevFun.Web.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app

# Set environment variables for GC (enable server mode and dynamic adaptation [DATAS])
ENV DOTNET_gcServer=1
ENV DOTNET_GCDynamicAdaptationMode=1

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevFun.Web.dll"]