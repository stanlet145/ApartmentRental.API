FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApartmentRental.API/ApartmentRental.API.csproj", "ApartmentRental.API/"]
RUN dotnet restore "ApartmentRental.API/ApartmentRental.API.csproj"
COPY . .
WORKDIR "/src/ApartmentRental.API"
RUN dotnet build "ApartmentRental.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApartmentRental.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApartmentRental.API.dll"]
