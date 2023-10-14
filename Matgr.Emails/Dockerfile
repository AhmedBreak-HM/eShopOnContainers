#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Matgr.Emails/Matgr.Emails.csproj", "Matgr.Emails/"]
RUN dotnet restore "Matgr.Emails/Matgr.Emails.csproj"
COPY . .
WORKDIR "/src/Matgr.Emails"
RUN dotnet build "Matgr.Emails.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Matgr.Emails.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Matgr.Emails.dll"]