#!/bin/bash

echo "Removing migration..."
dotnet ef migrations remove \
    --project ../src/Infrastructure/VyazmaTech.Prachka.Infrastructure.DataAccess/VyazmaTech.Prachka.Infrastructure.DataAccess.csproj \
    --startup-project ../src/Presentation/VyazmaTech.Prachka.Presentation.WebAPI/VyazmaTech.Prachka.Presentation.WebAPI.csproj \
    --context VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts.DatabaseContext \
    --configuration Release \
    --framework net8.0 \
    --verbose \
    --force 
    
if [ $? -eq 0 ]; then
    echo "Migration removed successfully."
else
    echo "Migration removal failed."
fi