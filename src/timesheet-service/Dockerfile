FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["src/TimesheetService.Core/TimesheetService.Core.csproj", "TimesheetService.Core/"]
COPY ["src/TimesheetService.Domain/TimesheetService.Domain.csproj", "TimesheetService.Domain/"]
COPY ["src/TimesheetService.Infrastructure/TimesheetService.Infrastructure.csproj", "TimesheetService.Infrastructure/"]
COPY ["src/TimesheetService.Persistence.Postgres/TimesheetService.Persistence.Postgres.csproj", "TimesheetService.Persistence.Postgres/"]
COPY ["src/TimesheetService.Persistence.SqlServer/TimesheetService.Persistence.SqlServer.csproj", "TimesheetService.Persistence.SqlServer/"]
COPY ["src/TimesheetService.WebApi/TimesheetService.WebApi.csproj", "TimesheetService.WebApi/"]
COPY ["src/Shared/TimesheetService.Shared.Abstractions/TimesheetService.Shared.Abstractions.csproj", "Shared/TimesheetService.Shared.Abstractions/"]
COPY ["src/Shared/TimesheetService.Shared.Infrastructure/TimesheetService.Shared.Infrastructure.csproj", "Shared/TimesheetService.Shared.Infrastructure/"]

RUN dotnet restore "TimesheetService.WebApi/TimesheetService.WebApi.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "src/TimesheetService.WebApi/TimesheetService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "src/TimesheetService.WebApi/TimesheetService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TimesheetService.WebApi.dll"]