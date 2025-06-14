# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookingService/Hotels.BookingsService.csproj", "BookingService/"]
COPY ["Hotels.Booking.Persistence/Hotels.Bookings.Persistence.csproj", "Hotels.Booking.Persistence/"]
COPY ["Domain/Hotels.Domain.csproj", "Domain/"]
COPY ["Hotels.Application/Hotels.Application.csproj", "Hotels.Application/"]
COPY ["Hotels.Bookings.Infrastructure/Hotels.Bookings.Infrastructure.csproj", "Hotels.Bookings.Infrastructure/"]
COPY ["Hotels.Infrastructure/Hotels.Infrastructure.csproj", "Hotels.Infrastructure/"]
COPY ["Hotels.Persistence/Hotels.Persistence.csproj", "Hotels.Persistence/"]
COPY ["Hotels.Presentation/Hotels.Presentation.csproj", "Hotels.Presentation/"]
RUN dotnet restore "./BookingService/Hotels.BookingsService.csproj"
COPY . .
WORKDIR "/src/BookingService"
RUN dotnet build "./Hotels.BookingsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hotels.BookingsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotels.BookingsService.dll"]