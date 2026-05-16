# SalaCerta - Back-end C# (Autenticação)

API desenvolvida em C# com ASP.NET Core responsável pelo controle de identidade, cadastro de usuários corporativos, validação de credenciais de acesso e emissão de tokens JWT. 

Este microsserviço funciona como o Provedor de Identidade (IdP) centralizado da aplicação, isolando a base de dados de login do restante do ecossistema.


## Decisões Técnicas e Arquitetura

* **ASP.NET Core Web API:** Escolhido pela robustez arquitetural, performance em segurança e forte tipagem em tempo de compilação, garantindo contratos estáveis para o fluxo de autenticação.
* **Entity Framework Core & PostgreSQL:** Utilizado como ORM para mapeamento relacional. A estrutura do banco de dados de usuários é controlada através do mecanismo de Migrations do EF Core, facilitando o rastreamento e atualização do esquema de dados.
* **Segurança de Credenciais:** As senhas dos usuários nunca são persistidas em texto limpo. O sistema aplica funções de hashing criptográfico antes de gravar os dados no banco, neutralizando vulnerabilidades em caso de vazamento da base.


## Estrutura de Configuração (appsettings.json)

As strings de conexão com o PostgreSQL e as configurações de tempo de expiração do token ficam centralizadas no arquivo `appsettings.json` na raiz da API:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=salacerta_auth;Username=usuario;Password=senha"
  },
  "JwtSettings": {
    "Secret": "ChaveMestreCompartilhadaDeSeguranca123!",
    "ExpiryInMinutes": 60
  }
}
Como Configurar e Rodar Localmente
Pré-requisitos
.NET SDK 6.0 ou superior instalado

Banco de dados PostgreSQL ativo

Passo a Passo
Abra o terminal na pasta raiz deste projeto e restaure as dependências do NuGet:

Bash
dotnet restore
Execute as migrações pendentes para criar a estrutura de tabelas de autenticação no PostgreSQL:

Bash
dotnet ef database update
Inicialize a aplicação:

Bash
dotnet run
O serviço de identidade passará a escutar requisições na porta http://localhost:5000. Os endpoints principais para integração com a camada cliente são POST /api/auth/register para novos cadastros e POST /api/auth/login para autenticação e retorno do token de acesso.