FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY ./src ./src
COPY ./*.sln .
COPY ./*.props ./
COPY ./.editorconfig .

RUN dotnet restore "src/Presentation/VyazmaTech.Prachka/VyazmaTech.Prachka.csproj"

FROM build AS publish
WORKDIR "/source/src/Presentation/VyazmaTech.Prachka"
RUN dotnet publish "VyazmaTech.Prachka.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
EXPOSE 8000
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8000
ENTRYPOINT ["dotnet", "VyazmaTech.Prachka.dll"]