# Rota de Viagem
Escolha a rota de viagem mais barata independente da quantidade de conexões. Para isso precisamos inserir as rotas.

## Criar uma solução completa para o problema das rotas de viagem usando .NET Core com Entity Framework e SQL Server, seguindo o padrão CQRS

> Pré-requisitos

1. Microsoft.EntityFrameworkCore - (versão 6.0.0)
2. Microsoft.EntityFrameworkCore.SqlServer - (versão 6.0.0)
3. Microsoft.EntityFrameworkCore.Tools - (versão 6.0.0)
4. Microsoft.Extensions.Configuration.Json - (versão 6.0.0)
5. Microsoft.Extensions.Configuration - (versão 6.0.0)
6. Microsoft.Extensions.DependencyInjection - (versão 6.0.0)
7. MediatR - (versão 12.0.1)
8. AutoMapper - (versão 12.0.1)
9. AutoMapper.Extensions.Microsoft.DependencyInjection - (versão 12.0.1)
10. Banco de dados SQL Server


## Como Executar

1. Configurar a string de conexão no projeto da API no appsettings.json.

## Executar Migration como já existe o migration InitialCreate apenas executar o update

> dotnet ef migrations add InitialCreate --startup-project ../TravelPlanner.Api/TravelPlanner.Api.csproj

> dotnet ef database update --startup-project ../TravelPlanner.Api/TravelPlanner.Api.csproj
