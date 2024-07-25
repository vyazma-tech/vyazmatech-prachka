#!/bin/bash

echo "Removing migration..."
dotnet ef migrations remove \
 --project src/Infrastructure/VyazmaTech.Prachka.Infrastructure.DataAccess/VyazmaTech.Prachka.Infrastructure.DataAccess.csproj \
 --startup-project src/Presentation/VyazmaTech.Prachka/VyazmaTech.Prachka.csproj \
 --context VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts.DatabaseContext \
 --configuration Release \
 --verbose \
 --force
    
if [ $? -eq 0 ]; then
    echo "Migration removed successfully."
else
    echo "Migration removal failed."
fi