FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["BasedLaucncher/BasedLaucncher.csproj", "BasedLaucncher/"]
RUN dotnet restore "BasedLaucncher/BasedLaucncher.csproj"
COPY . .
WORKDIR "/src/BasedLaucncher"
RUN dotnet build "BasedLaucncher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BasedLaucncher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BasedLaucncher.dll"]
