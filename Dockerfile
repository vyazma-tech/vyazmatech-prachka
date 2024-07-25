FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

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
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VyazmaTech.Prachka.dll"]