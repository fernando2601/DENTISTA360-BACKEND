# DENTISTA360 Backend API

Sistema de gestão para clínicas odontológicas desenvolvido em ASP.NET Core 8.0 com Dapper, JWT Authentication e Swagger.

## 🚀 Tecnologias Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Dapper** - ORM leve para acesso a dados
- **SQL Server** - Banco de dados
- **JWT (JSON Web Tokens)** - Autenticação
- **Swagger/OpenAPI** - Documentação da API
- **BCrypt.Net** - Hash de senhas

## 📋 Pré-requisitos

- .NET 8.0 SDK
- SQL Server (LocalDB, Express ou Full)
- Visual Studio 2022 ou VS Code

## ⚙️ Configuração do Ambiente

### 1. Clone o repositório
```bash
git clone [url-do-repositorio]
cd DENTISTA360-BACKEND
```

### 2. Configure o banco de dados
1. Abra o SQL Server Management Studio
2. Execute o script `Database/CreateDatabase.sql` para criar o banco e as tabelas
3. Ajuste a connection string no `appsettings.json` se necessário

### 3. Instale as dependências
```bash
dotnet restore
```

### 4. Execute o projeto
```bash
dotnet run
```

A API estará disponível em:
- **Swagger UI**: `https://localhost:7xxx` (porta pode variar)
- **API Base URL**: `https://localhost:7xxx`

## 📊 Estrutura do Banco de Dados

### Tabelas Principais

- **User**: Usuários do sistema
- **Clinica**: Clínicas cadastradas
- **Endereco**: Endereços (compartilhado entre User e Clinica)
- **Grupo**: Grupos/Perfis de usuário
- **user_clinic**: Relacionamento usuário-clínica com permissões

### Dados de Teste

O script SQL inclui dados de exemplo:
- **Usuário**: henrique@clinicalima.com / senha: 123456
- **Clínica**: Clinica Lima

## 🔐 Endpoints da API

### Autenticação

#### POST `/auth/login`
Realiza login e retorna token JWT.

**Request:**
```json
{
  "email": "henrique@clinicalima.com",
  "senha": "123456"
}
```

**Response:**
```json
{
  "accessToken": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Usuário

#### GET `/usuarios/info`
Obtém informações do usuário logado e suas clínicas.

**Headers:**
```
Authorization: Bearer <seu_token_jwt>
```

**Response:**
```json
{
  "clinicas": [
    {
      "id": 1,
      "nomeFantasia": "Clinica Lima"
    }
  ],
  "user": {
    "nome": "Henrique Lima"
  }
}
```

### Clínica

#### GET `/clinica/{clinica_id}/permissions`
Obtém as permissões do usuário em uma clínica específica.

**Headers:**
```
Authorization: Bearer <seu_token_jwt>
```

**Response:**
```json
{
  "permission": "Diretor"
}
```

## 🛠️ Configurações

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DENTISTA360;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "JWT": {
    "SecretKey": "YourSuperSecretKeyForJWTTokenGeneration2024!",
    "Issuer": "DENTISTA360-API",
    "Audience": "DENTISTA360-USERS",
    "ExpirationInHours": 24
  }
}
```

## 📖 Documentação da API

Após executar o projeto, acesse `https://localhost:7xxx` para ver a documentação completa da API no Swagger UI, onde você pode:

- Ver todos os endpoints disponíveis
- Testar as chamadas da API
- Autenticar-se usando JWT tokens
- Ver exemplos de request/response

## 🔒 Autenticação JWT

1. Faça login no endpoint `/auth/login`
2. Copie o token retornado
3. No Swagger UI, clique em "Authorize" 
4. Cole o token completo (incluindo "Bearer ")
5. Agora você pode testar os endpoints protegidos

## 🏗️ Arquitetura do Projeto

```
DENTISTA360-BACKEND/
├── Controllers/           # Controladores da API
├── Models/               # Entidades do banco de dados
├── DTOs/                 # Objetos de transferência de dados
├── Services/             # Serviços de negócio
├── Repositories/         # Acesso a dados com Dapper
├── Data/                 # Configuração do banco de dados
└── Database/             # Scripts SQL
```

## 🚨 Tipos de Usuário

- **employee**: Funcionário
- **director**: Diretor
- **doctor**: Médico

## 🔧 Desenvolvimento

Para adicionar novos endpoints:

1. Crie o DTO necessário em `/DTOs`
2. Adicione métodos no repository apropriado
3. Implemente a lógica no controller
4. Documente com comentários XML para o Swagger

## 📝 Notas Importantes

- Todas as senhas são hasheadas com BCrypt
- Tokens JWT expiram em 24 horas (configurável)
- A API usa CORS liberado para desenvolvimento
- Logs são configurados para Development e Production

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request
