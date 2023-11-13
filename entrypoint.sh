#!/bin/sh
#Waiting for the db to get initialized so that migration of data tables is possible in runtime
while ! pg_isready -h db -p 5432 -q -U admin; do
  echo "Waiting for PostgreSQL to start..."
  sleep 2
done

echo "PostgreSQL finished loading. Starting API..."
dotnet BackEndAPI.dll