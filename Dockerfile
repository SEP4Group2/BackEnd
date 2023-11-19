#This image is used for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

#This image is used to run the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build-env /app/out .
#Adding postgressql-client and a custom entrypoint, where the docker waits until the database is finished loading
#Afterwards proceeds with running the API
RUN apk add --no-cache postgresql-client
COPY entrypoint.sh ./
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]