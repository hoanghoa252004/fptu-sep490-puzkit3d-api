# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files for restore
COPY ["src/WebApi/PuzKit3D.WebApi/PuzKit3D.WebApi.csproj", "src/WebApi/PuzKit3D.WebApi/"]
COPY ["src/Contract/PuzKit3D.Contract/PuzKit3D.Contract.csproj", "src/Contract/PuzKit3D.Contract/"]

# SharedKernel
COPY ["src/SharedKernel/PuzKit3D.SharedKernel.Api/PuzKit3D.SharedKernel.Api.csproj", "src/SharedKernel/PuzKit3D.SharedKernel.Api/"]
COPY ["src/SharedKernel/PuzKit3D.SharedKernel.Application/PuzKit3D.SharedKernel.Application.csproj", "src/SharedKernel/PuzKit3D.SharedKernel.Application/"]
COPY ["src/SharedKernel/PuzKit3D.SharedKernel.Domain/PuzKit3D.SharedKernel.Domain.csproj", "src/SharedKernel/PuzKit3D.SharedKernel.Domain/"]
COPY ["src/SharedKernel/PuzKit3D.SharedKernel.Infrastructure/PuzKit3D.SharedKernel.Infrastructure.csproj", "src/SharedKernel/PuzKit3D.SharedKernel.Infrastructure/"]

# Catalog Module
COPY ["src/Modules/Catalog/PuzKit3D.Modules.Catalog.Api/PuzKit3D.Modules.Catalog.Api.csproj", "src/Modules/Catalog/PuzKit3D.Modules.Catalog.Api/"]
COPY ["src/Modules/Catalog/PuzKit3D.Modules.Catalog.Application/PuzKit3D.Modules.Catalog.Application.csproj", "src/Modules/Catalog/PuzKit3D.Modules.Catalog.Application/"]
COPY ["src/Modules/Catalog/PuzKit3D.Modules.Catalog.Domain/PuzKit3D.Modules.Catalog.Domain.csproj", "src/Modules/Catalog/PuzKit3D.Modules.Catalog.Domain/"]
COPY ["src/Modules/Catalog/PuzKit3D.Modules.Catalog.Infrastructure/PuzKit3D.Modules.Catalog.Infrastructure.csproj", "src/Modules/Catalog/PuzKit3D.Modules.Catalog.Infrastructure/"]
COPY ["src/Modules/Catalog/PuzKit3D.Modules.Catalog.Persistence/PuzKit3D.Modules.Catalog.Persistence.csproj", "src/Modules/Catalog/PuzKit3D.Modules.Catalog.Persistence/"]

# Cart Module
COPY ["src/Modules/Cart/PuzKit3D.Modules.Cart.Api/PuzKit3D.Modules.Cart.Api.csproj", "src/Modules/Cart/PuzKit3D.Modules.Cart.Api/"]
COPY ["src/Modules/Cart/PuzKit3D.Modules.Cart.Application/PuzKit3D.Modules.Cart.Application.csproj", "src/Modules/Cart/PuzKit3D.Modules.Cart.Application/"]
COPY ["src/Modules/Cart/PuzKit3D.Modules.Cart.Domain/PuzKit3D.Modules.Cart.Domain.csproj", "src/Modules/Cart/PuzKit3D.Modules.Cart.Domain/"]
COPY ["src/Modules/Cart/PuzKit3D.Modules.Cart.Infrastructure/PuzKit3D.Modules.Cart.Infrastructure.csproj", "src/Modules/Cart/PuzKit3D.Modules.Cart.Infrastructure/"]
COPY ["src/Modules/Cart/PuzKit3D.Modules.Cart.Persistence/PuzKit3D.Modules.Cart.Persistence.csproj", "src/Modules/Cart/PuzKit3D.Modules.Cart.Persistence/"]

# InStock Module
COPY ["src/Modules/InStock/PuzKit3D.Modules.InStock.Api/PuzKit3D.Modules.InStock.Api.csproj", "src/Modules/InStock/PuzKit3D.Modules.InStock.Api/"]
COPY ["src/Modules/InStock/PuzKit3D.Modules.InStock.Application/PuzKit3D.Modules.InStock.Application.csproj", "src/Modules/InStock/PuzKit3D.Modules.InStock.Application/"]
COPY ["src/Modules/InStock/PuzKit3D.Modules.InStock.Domain/PuzKit3D.Modules.InStock.Domain.csproj", "src/Modules/InStock/PuzKit3D.Modules.InStock.Domain/"]
COPY ["src/Modules/InStock/PuzKit3D.Modules.InStock.Infrastructure/PuzKit3D.Modules.InStock.Infrastructure.csproj", "src/Modules/InStock/PuzKit3D.Modules.InStock.Infrastructure/"]
COPY ["src/Modules/InStock/PuzKit3D.Modules.InStock.Persistence/PuzKit3D.Modules.InStock.Persistence.csproj", "src/Modules/InStock/PuzKit3D.Modules.InStock.Persistence/"]

# User Module
COPY ["src/Modules/User/PuzKit3D.Modules.User.Api/PuzKit3D.Modules.User.Api.csproj", "src/Modules/User/PuzKit3D.Modules.User.Api/"]
COPY ["src/Modules/User/PuzKit3D.Modules.User.Application/PuzKit3D.Modules.User.Application.csproj", "src/Modules/User/PuzKit3D.Modules.User.Application/"]
COPY ["src/Modules/User/PuzKit3D.Modules.User.Domain/PuzKit3D.Modules.User.Domain.csproj", "src/Modules/User/PuzKit3D.Modules.User.Domain/"]
COPY ["src/Modules/User/PuzKit3D.Modules.User.Infrastructure/PuzKit3D.Modules.User.Infrastructure.csproj", "src/Modules/User/PuzKit3D.Modules.User.Infrastructure/"]
COPY ["src/Modules/User/PuzKit3D.Modules.User.Persistence/PuzKit3D.Modules.User.Persistence.csproj", "src/Modules/User/PuzKit3D.Modules.User.Persistence/"]

# Partner Module
COPY ["src/Modules/Partner/PuzKit3D.Modules.Partner.Api/PuzKit3D.Modules.Partner.Api.csproj", "src/Modules/Partner/PuzKit3D.Modules.Partner.Api/"]
COPY ["src/Modules/Partner/PuzKit3D.Modules.Partner.Application/PuzKit3D.Modules.Partner.Application.csproj", "src/Modules/Partner/PuzKit3D.Modules.Partner.Application/"]
COPY ["src/Modules/Partner/PuzKit3D.Modules.Partner.Domain/PuzKit3D.Modules.Partner.Domain.csproj", "src/Modules/Partner/PuzKit3D.Modules.Partner.Domain/"]
COPY ["src/Modules/Partner/PuzKit3D.Modules.Partner.Infrastructure/PuzKit3D.Modules.Partner.Infrastructure.csproj", "src/Modules/Partner/PuzKit3D.Modules.Partner.Infrastructure/"]
COPY ["src/Modules/Partner/PuzKit3D.Modules.Partner.Persistence/PuzKit3D.Modules.Partner.Persistence.csproj", "src/Modules/Partner/PuzKit3D.Modules.Partner.Persistence/"]

# Payment Module
COPY ["src/Modules/Payment/PuzKit3D.Modules.Payment.Api/PuzKit3D.Modules.Payment.Api.csproj", "src/Modules/Payment/PuzKit3D.Modules.Payment.Api/"]
COPY ["src/Modules/Payment/PuzKit3D.Modules.Payment.Application/PuzKit3D.Modules.Payment.Application.csproj", "src/Modules/Payment/PuzKit3D.Modules.Payment.Application/"]
COPY ["src/Modules/Payment/PuzKit3D.Modules.Payment.Domain/PuzKit3D.Modules.Payment.Domain.csproj", "src/Modules/Payment/PuzKit3D.Modules.Payment.Domain/"]
COPY ["src/Modules/Payment/PuzKit3D.Modules.Payment.Infrastructure/PuzKit3D.Modules.Payment.Infrastructure.csproj", "src/Modules/Payment/PuzKit3D.Modules.Payment.Infrastructure/"]
COPY ["src/Modules/Payment/PuzKit3D.Modules.Payment.Persistence/PuzKit3D.Modules.Payment.Persistence.csproj", "src/Modules/Payment/PuzKit3D.Modules.Payment.Persistence/"]

# Restore dependencies
RUN dotnet restore "src/WebApi/PuzKit3D.WebApi/PuzKit3D.WebApi.csproj"

# Copy all source code
COPY . .

# Build the WebApi project
WORKDIR "/src/src/WebApi/PuzKit3D.WebApi"
RUN dotnet build "PuzKit3D.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PuzKit3D.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configure Kestrel to listen on HTTP only
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_HTTP_PORTS=80

ENTRYPOINT ["dotnet", "PuzKit3D.WebApi.dll"]