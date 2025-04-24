FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7093

ENV ASPNETCORE_URLS=http://+:7093

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG CONFIG=Release
WORKDIR /src

COPY ["MusiciansAPP.API/MusiciansAPP.API.csproj", "MusiciansAPP.API/"]
COPY ["MusiciansAPP.BL/MusiciansAPP.BL.csproj", "MusiciansAPP.BL/"]
COPY ["MusiciansAPP.Domain/MusiciansAPP.Domain.csproj", "MusiciansAPP.Domain/"]
COPY ["MusiciansAPP.DAL/MusiciansAPP.DAL.csproj", "MusiciansAPP.DAL/"]

RUN dotnet restore "MusiciansAPP.API/MusiciansAPP.API.csproj"
COPY . .
WORKDIR "/src/MusiciansAPP.API"
RUN dotnet build "MusiciansAPP.API.csproj" -c $CONFIG -o /app/build

FROM build AS publish
ARG CONFIG=Release
RUN dotnet publish "MusiciansAPP.API.csproj" -c $CONFIG -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "MusiciansAPP.API.dll"]
