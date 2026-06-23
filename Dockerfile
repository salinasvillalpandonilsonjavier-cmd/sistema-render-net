# Estructura oficial para compilar apps de .NET 8 en Render
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY *.sln ./
COPY LOGIN/*.csproj ./LOGIN/
RUN dotnet restore

# Copiar todo lo demás y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Crear la imagen final de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Configurar el puerto automático de Render
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "LOGIN.dll"]