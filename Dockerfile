# Stage 1: Runtime Base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# We don't expose 80 or 443 because Render uses dynamic PORT
# The app is already configured to listen on PORT environment variable

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore
COPY ["src/Gym.ManagementAPI/Gym.ManagementAPI.csproj", "src/Gym.ManagementAPI/"]
COPY ["src/Gym.PublicAPI/Gym.PublicAPI.csproj", "src/Gym.PublicAPI/"]
COPY ["src/Gym.Application/Gym.Application.csproj", "src/Gym.Application/"]
COPY ["src/Gym.Domain/Gym.Domain.csproj", "src/Gym.Domain/"]
COPY ["src/Gym.Infrastructure/Gym.Infrastructure.csproj", "src/Gym.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/Gym.ManagementAPI/Gym.ManagementAPI.csproj"

# Copy full source
COPY . .
WORKDIR "/src/src/Gym.ManagementAPI"
RUN dotnet build "Gym.ManagementAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Gym.ManagementAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# COPY FILE DỮ LIỆU SQLITE TỪ LOCAL VÀO DOCKER
# Đường dẫn: /src/src/Gym.ManagementAPI/GymManagement.sqlite
COPY src/Gym.ManagementAPI/GymManagement.sqlite .

# Fix for inotify limit on Linux/Render
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Use PORT from environment variable (as configured in Program.cs)
ENTRYPOINT ["dotnet", "Gym.ManagementAPI.dll"]
