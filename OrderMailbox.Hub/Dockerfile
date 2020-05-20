FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["./OrderMailboxHub.Host/OrderMailboxHub.Host.csproj", "OrderMailboxHub.Host/"]
COPY ["./OrderMailboxHub.Application/OrderMailboxHub.Application.csproj", "OrderMailboxHub.Application/"]
COPY ["./OrderMailboxHub.Api/OrderMailboxHub.Api.csproj", "OrderMailboxHub.Api/"]
COPY ./privatefeeds.config ./
RUN dotnet restore "OrderMailboxHub.Host/OrderMailboxHub.Host.csproj" --configfile ./privatefeeds.config
COPY . .
WORKDIR "/src/OrderMailboxHub.Host"
RUN dotnet build "OrderMailboxHub.Host.csproj" -c Release -o /app/

FROM build AS publish
ARG buildnumber=1.0.0
ARG generate_swagger=false
RUN dotnet publish "OrderMailboxHub.Host.csproj" -c Release /p:Version=$buildnumber /p:generate_swagger=$generate_swagger -o /app/

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OrderMailboxHub.Host.dll"]