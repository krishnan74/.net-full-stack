FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5182

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["DotnetAPI.csproj", "./"]
RUN dotnet restore "./DotnetAPI.csproj"

COPY . .

RUN dotnet tool install --global dotnet-ef 
RUN /root/.dotnet/tools/dotnet-ef database update 

WORKDIR "/src"
RUN dotnet build "DotnetAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotnetAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5182 
ENTRYPOINT ["dotnet", "DotnetAPI.dll"]