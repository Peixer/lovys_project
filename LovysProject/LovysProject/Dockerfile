FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["LovysProject/LovysProject.csproj", "LovysProject/"]
RUN dotnet restore "LovysProject/LovysProject.csproj"
COPY . .
WORKDIR "/src/LovysProject"
RUN dotnet build "LovysProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LovysProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LovysProject.dll"]
