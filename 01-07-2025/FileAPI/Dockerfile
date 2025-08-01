FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build

WORKDIR /App

# Copy everything
COPY . ./
# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
# Restore as distinct layers
RUN dotnet restore
# Run migrations (this will fail if the db is not up, so do this manually if needed)
# RUN dotnet ef database update

# Build and publish a release
RUN dotnet publish -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
WORKDIR /App
COPY --from=build /App/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "FileAPI.dll"]