#!/bin/bash

dotnet ef database update \
 --project src/Infrastructure/VyazmaTech.Prachka.Infrastructure.DataAccess/VyazmaTech.Prachka.Infrastructure.DataAccess.csproj \
 --startup-project src/Presentation/VyazmaTech.Prachka/VyazmaTech.Prachka.csproj \
 --context VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts.DatabaseContext \
 --configuration Release \
 --framework net8.0 \
 --verbose