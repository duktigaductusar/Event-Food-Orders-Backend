# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files
COPY ["EventFoodOrders/EventFoodOrders.csproj", "EventFoodOrders/"]

# Restore dependencies
RUN dotnet restore "./EventFoodOrders/EventFoodOrders.csproj"

# Copy everything and build
COPY . .
WORKDIR /src/EventFoodOrders
RUN dotnet build "./EventFoodOrders.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventFoodOrders.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Add debugging tools
RUN apt update && apt install -y iproute2 net-tools curl

# Ensure the environment is set to Production
ENV ASPNETCORE_ENVIRONMENT=Production

# Image entry point for debugging
ENTRYPOINT ["tail", "-f", "/dev/null"]
