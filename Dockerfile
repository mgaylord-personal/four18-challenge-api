#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY . ./
#RUN dotnet restore --configfile nuget.config 
RUN dotnet publish -c Release -o output

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/output .
ENTRYPOINT [ "dotnet", "Four18.Challenge.WebApi.dll" ]