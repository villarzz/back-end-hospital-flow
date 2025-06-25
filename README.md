# 🏥 Hospital Flow - Back-end

Este é o back-end do sistema **Hospital Flow**, uma aplicação desenvolvida para gerenciamento de internações hospitalares. Ele fornece endpoints RESTful para criação e edição de pacientes, controle de internações e geração de relatórios.

## ⚙️ Tecnologias utilizadas

- C#  
- .NET 8  
- SQLite  
- Swagger para documentação da API  
- JWT para autenticação  

## 🚀 Como executar o projeto

1. Certifique-se de ter o [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) instalado.
2. Abra o projeto na sua IDE (como Visual Studio ou VS Code).
3. Execute o projeto com o seguinte comando no terminal ou através da própria IDE:
4. A aplicação estará disponível em:
- 🔗 HTTP: http://localhost:5240
- 🔒 HTTPS: https://localhost:7174

## 📚 Documentação da API
A documentação interativa (Swagger) estará disponível ao rodar o projeto:

📄 https://localhost:7174/swagger/index.html

## 📌 Endpoints principais
Internações
- POST /api/Internacoes/criar-internacao
- PUT /api/Internacoes/atualizar-internacao
- GET /api/Internacoes/obter-internacoes
- DELETE /api/Internacoes/deletar-internacao/{id}

Paciente
- POST /api/Paciente/adicionar-paciente
- PUT /api/Paciente/editar-paciente
- GET /api/Paciente/obter-pacientes
 
Relatórios
- GET /api/Relatorios/relatorio-internacoes

## 🔐 Autenticação
A API utiliza JWT (JSON Web Token) para autenticação. Certifique-se de incluir o token no header das requisições aos endpoints protegidos.
