FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY [".", "PropertyAPI/"]
RUN dotnet restore "PropertyAPI/PropertyAPI/PropertyAPI.csproj"
COPY . .
WORKDIR "/src/PropertyAPI"
RUN dotnet dev-certs https --clean && dotnet dev-certs https --verbose && dotnet dev-certs https --trust
RUN dotnet build "./PropertyAPI/PropertyAPI.csproj" -c Release -o /app/build
RUN dotnet publish "./PropertyAPI/PropertyAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false
WORKDIR /app/PropertyAPI
ENTRYPOINT ["dotnet", "run"]