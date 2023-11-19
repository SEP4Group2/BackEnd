#!/bin/sh

while ! pg_isready -h db -p 5432 -q -U admin; do
  echo "Waiting for {postgreSQL to start..."
  sleep 2
done

echo "postgreSQL finished loading. Starting API..."
dotnet BackEndAPI.dll