# Estructura optimizada para .NET 10.0
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copiamos todo el contenido primero
COPY . ./

# Restauramos y compilamos apuntando directo al proyecto
RUN dotnet restore "LOGIN/LOGIN.csproj"
RUN dotnet publish "LOGIN/LOGIN.csproj" -c Release -o out

# Crear la imagen final de ejecución usando el entorno de .NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

# Puerto automático para Render
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "LOGIN.dll"]