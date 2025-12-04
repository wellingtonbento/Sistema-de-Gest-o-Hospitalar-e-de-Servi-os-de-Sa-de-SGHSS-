# 🏥 API-SGHSS Sistema-de-Gestão-Hospitalar-e-de-Serviços-de-Saúde-SGHSS-
Este projeto faz parte de um trabalho acadêmico,o qual devo desenvolver uma API REST contendo um CRUD básico e autenticação. Aproveito esta oportunidade para reforçar meus conhecimentos em desenvolvimento de APIs com C# e ASP.NET Core. Embora seja um projeto relativamente pequeno, meu objetivo é demonstrar de forma sólida minha evolução, organização e capacidade técnica utilizando boas práticas como:
- Services para regras de negócio
- Repository para logica de acesso ao banco de dados
- DTOs para padronização de entrada e saída
- Autenticação JWT
- Documentação com Swagger
- AutoMapper
- Middleware de exceções

## 📌 Tecnologias Utilizadas
- ASP.NET Core 8
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger / OpenAPI
- AutoMapper
- Repository + Service
- Middleware personalizado
- C# 12

## ⚙️ Configuração do Banco de Dados
No arquivo appsettings.json, configure sua conexão:
```C#
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=sghss_db;user=root;password=1234"
}
```

## 🚀 Como Rodar o Projeto
1️⃣Criar o banco e aplicar migrações
```
dotnet ef database update
```

2️⃣Executar a API
```
dotnet run
```

## 🔐 Autenticação JWT
Antes de acessar endpoints protegidos, é necessário criar o administrador, Paciente e Doutor  
1️⃣Registrar os Usuarios
```
POST /auth/register
{
  "name": "marcos",
  "email": "marcos@gmail.com",
  "password": "Teste#2025"
}
```
registre mais dois usuario para depois atribuir eles as Roles. 
Aconcelho a deixar a Password como => Teste#2025 para todos.

2️⃣Criar as Roles
```
POST api/Auth/CreateRole
{
  "RoleName": Admin
}
```
Crie apenas o Admin, Patient, Doctor.

3️⃣Atribuir os usuarios a suas Roles
```
POST api/Auth/AddUserToRole
{
  "Email": marcos@gmail.com,
  "RoleName": Admin
}
```
Adicione as Roles de Admin, Patient, Doctor para seus Usuarios.

4️⃣Fazer o login
```
POST api/Auth/login
{
  "UserName": marcos,
  "Password": Teste#2025
}
```
Com o login feito vai gerar um token para poder testar os endpoints.

5️⃣Se o Token expirar faça esse passo para gerar um novo Token
```
POST api/Auth/refresh-token
{
  "acessToken": Token ,
  "refreshToken": Refresh Token,
  "expiration": 2025-12-03T20:45:17Z
}
```
para obter o Refresh-token use o Banco de dados na tabela ASPNETUSER e copie o RefreshToken do usuario que estiver usando.

6️⃣se quiser Revogar um token faça esse passo
```
POST api/Auth/revoke/{userName}
{
  "UserName": marcos
}
```
Revoga o Token do Banco de dados.

## ⚠️ Tratamento Global de Erros
A API conta com um Middleware próprio que padroniza respostas de erro:  
Mensagem amigável  
StatusCode adequado  
Retorna stacktrace apenas em ambiente de desenvolvimento

## 📂 Controllers da API
🔸 AuthController
- Registro
- Login
- criar função(role)
- Adicionar usuario a função(AddUserToRole)
- Refresh Token
- Revogar(Revoke)

🔸 PatientController
- CRUD de pacientes

🔸 DoctorController
- CRUD de médicos

🔸 AppointmentController
- CRUD de Consultas

## ⚠️Problema de Duplicidade
Durante o desenvolvimento da API, acabei estruturando o cadastro de Paciente e Médico com atributos como Nome, Email e CPF para Pacientes e CRM para Médicos nas próprias entidades do domínio. Paralelamente, utilizei o Identity para autenticação, que também possui Nome e Email por padrão. No início do projeto, eu não percebi que isso acabaria gerando duplicidade das mesmas informações em dois lugares diferentes.

Essa duplicidade não compromete o funcionamento do sistema, mas aconteceu por uma decisão inicial de modelagem antes de eu ter uma visão completa da integração entre o Identity e as entidades do domínio. Optei por manter assim para não prejudicar o andamento e a conclusão do projeto. Dessa forma, o usuário continua sendo registrado no Identity para autenticação, e também nas entidades Paciente ou Médico para funcionamento interno do sistema.
