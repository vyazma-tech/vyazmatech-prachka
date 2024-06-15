#!/bin/bash

echo "Starting migration..."
dotnet ef migrations add $1 \
    --project ../src/Infrastructure/VyazmaTech.Prachka.Infrastructure.DataAccess/VyazmaTech.Prachka.Infrastructure.DataAccess.csproj \
    --startup-project ../src/Presentation/VyazmaTech.Prachka.Presentation.WebAPI/VyazmaTech.Prachka.Presentation.WebAPI.csproj \
    --context VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts.DatabaseContext \
    --configuration Release \
    --framework net8.0 \
    --verbose \
    --output-dir Migrations

if [ $? -eq 0 ]; then
    echo "Migration added successfully."
else
    echo "Migration failed."
fi