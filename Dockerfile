#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7011
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN dotnet dev-certs https
WORKDIR /src
COPY KanbanBoard.sln ./
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Core.Application/Core.Application.csproj", "Core.Application/"]
COPY ["Core.CrossCuttingConcerns/Core.CrossCuttingConcerns.csproj", "Core.CrossCuttingConcerns/"]
COPY ["Core.Security/Core.Security.csproj", "Core.Security/"]
COPY ["Core.Persistence/Core.Persistence.csproj", "Core.Persistence/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Core.Utility/Core.Utility.csproj", "Core.Utility/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]