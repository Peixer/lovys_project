FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Lovys.WebApp/Lovys.WebApp.csproj", "Lovys.WebApp/"]
COPY ["Lovys.Core/Lovys.Core.csproj", "Lovys.Core/"]
RUN dotnet restore "Lovys.WebApp/Lovys.WebApp.csproj"
COPY . .
WORKDIR "/src/Lovys.WebApp"
RUN dotnet build "Lovys.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lovys.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lovys.WebApp.dll"]
