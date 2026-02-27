# ═══════════════════════════════════════════════════════════════
#  NexCart API — Multi-stage Docker Build
# ═══════════════════════════════════════════════════════════════

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first (better layer caching)
COPY NexCart.slnx .
COPY src/NexCart.Domain/NexCart.Domain.csproj src/NexCart.Domain/
COPY src/NexCart.Application/NexCart.Application.csproj src/NexCart.Application/
COPY src/NexCart.Infrastructure/NexCart.Infrastructure.csproj src/NexCart.Infrastructure/
COPY src/NexCart.API/NexCart.API.csproj src/NexCart.API/
RUN dotnet restore NexCart.slnx

# Copy everything and build
COPY . .
RUN dotnet publish src/NexCart.API/NexCart.API.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Non-root user for security (built-in to Microsoft .NET images)
USER $APP_UID

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5119
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5119

HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget --no-verbose --tries=1 --spider http://localhost:5119/health || exit 1

ENTRYPOINT ["dotnet", "NexCart.API.dll"]
