FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia los archivos de proyecto y restaura dependencias
COPY ["car_repair.csproj", "./"]
RUN dotnet restore "./car_repair.csproj"

# Copia el resto del código y compila
COPY . .
RUN dotnet publish "car_repair.csproj" -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expone los puertos definidos en launchSettings.json
EXPOSE 5000
EXPOSE 5001

# Comando de inicio
ENTRYPOINT ["dotnet", "car_repair.dll"]