# Base image for ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ECom_Inventory/ECom_Inventory.csproj", "ECom_Inventory/"]
RUN dotnet restore "./ECom_Inventory/ECom_Inventory.csproj"
COPY . .
WORKDIR "/src/ECom_Inventory"
RUN dotnet build "./ECom_Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish .NET application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ECom_Inventory.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Install Node.js and Build React App
FROM node:18 AS node-build
WORKDIR /frontend
COPY ECom_Inventory/ClientApp .  # Assuming React is in "ClientApp" folder
RUN npm install
RUN npm run build  # Creates production-ready frontend build

# Final stage: Serve .NET + React
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=node-build /frontend/build ./wwwroot  # Move React build to wwwroot

# Start the app
ENTRYPOINT ["dotnet", "ECom_Inventory.dll"]
