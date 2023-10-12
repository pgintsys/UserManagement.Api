#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Emte.UserManagement.MicroService.API/Emte.UserManagement.MicroService.API.csproj", "Emte.UserManagement.MicroService.API/"]
COPY ["Emte.UserManagement.MicroService.API/appsettings.json", "Emte.UserManagement.MicroService.API/"]
COPY ["Core/Emte.Core.API/Emte.Core.API.csproj", "Core/Emte.Core.API/"]
COPY ["Core/Emte.Core.Models/Emte.Core.Models.csproj", "Core/Emte.Core.Models/"]
COPY ["Core/Emte.Core.DataAccess.Impl/Emte.Core.DataAccess.Impl.csproj", "Core/Emte.Core.DataAccess.Impl/"]
COPY ["Core/Emte.Core.DataAccess/Emte.Core.DataAccess.csproj", "Core/Emte.Core.DataAccess/"]
COPY ["Core/Emte.Core.DomainModels/Emte.Core.DomainModels.csproj", "Core/Emte.Core.DomainModels/"]
COPY ["Core/Emte.Core.Repository.Contracts/Emte.Core.Repository.Contracts.csproj", "Core/Emte.Core.Repository.Contracts/"]
COPY ["UserManagement/Emte.UserManagement.BusinessLogic.Contracts/Emte.UserManagement.BusinessLogic.Contracts.csproj", "UserManagement/Emte.UserManagement.BusinessLogic.Contracts/"]
COPY ["UserManagement/Emte.UserManagement.DomainModels/Emte.UserManagement.DomainModels.csproj", "UserManagement/Emte.UserManagement.DomainModels/"]
COPY ["UserManagement/Emte.UserManagement.Models/Emte.UserManagement.Models.csproj", "UserManagement/Emte.UserManagement.Models/"]
COPY ["Core/Emte.Core.Authentication.Contract/Emte.Core.Authentication.Contract.csproj", "Core/Emte.Core.Authentication.Contract/"]
COPY ["UserManagement/Emte.UserManagement.BusinessLogic/Emte.UserManagement.BusinessLogic.csproj", "UserManagement/Emte.UserManagement.BusinessLogic/"]
COPY ["UserManagement/Emte.UserManagement.Core/Emte.UserManagement.Core.csproj", "UserManagement/Emte.UserManagement.Core/"]
COPY ["UserManagement/Emte.UserManagement.DataAccess/Emte.UserManagement.DataAccess.csproj", "UserManagement/Emte.UserManagement.DataAccess/"]
COPY ["UserManagement/Emte.UserManagement.Controllers/Emte.UserManagement.Controllers.csproj", "UserManagement/Emte.UserManagement.Controllers/"]
COPY ["UserManagement/Emte.UserManagement.Repository/Emte.UserManagement.Repository.csproj", "UserManagement/Emte.UserManagement.Repository/"]
COPY ["Core/Emte.Core.JWTAuth/Emte.Core.JWTAuth.csproj", "Core/Emte.Core.JWTAuth/"]
COPY ["Core/Emte.Core.Authentication.Impl/Emte.Core.Authentication.Impl.csproj", "Core/Emte.Core.Authentication.Impl/"]
RUN dotnet restore "Emte.UserManagement.MicroService.API/Emte.UserManagement.MicroService.API.csproj"
COPY . .
WORKDIR "/src/Emte.UserManagement.MicroService.API"
RUN dotnet build "Emte.UserManagement.MicroService.API.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Emte.UserManagement.MicroService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emte.UserManagement.MicroService.API.dll"]
