FROM mcr.microsoft.com/dotnet/aspnet:8.0.0 AS base
WORKDIR /app
#EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build
WORKDIR /src

# Copia los archivos del proyecto y restaura las dependencias
COPY . .

# Restaura las dependencias y realiza la construcción del proyecto
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Punto de entrada para la aplicación
ENTRYPOINT ["dotnet", "INVENTARIO.dll"]