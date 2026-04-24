---
title: "Segurança"
linkTitle: "Segurança"
weight: 50
description: "Autenticação JWT, proteção de endpoints"
---

## Descrição

Documenta os mecanismos de autenticação e autorização implementados: JWT Bearer Token (HS256), AuthenticateFilter, padrão de proteção de endpoints. Deve ser consultado ao criar endpoints autenticados ou ao entender o fluxo de autenticação.

## Contexto

A autenticação é implementada via JWT HS256 usando `System.IdentityModel.Tokens.Jwt`. A validação é feita por um `IAsyncActionFilter` (`AuthenticateFilter`) ativado pelo atributo `[Authenticate]`. O enriquecimento de logs com `UserId` e `UserName` é realizado dentro do filtro, de forma transparente para Features e endpoints (DA-013).

---

## JWT Bearer Token (HS256)

| Característica | Valor |
|---|---|
| Algoritmo | HS256 (HMAC-SHA256) |
| Validade | 1 hora |
| Claims | `id` (int), `userName` (string) |
| Secret | Configurado em `appsettings.json` → `Jwt:Secret` |
| Geração | Via endpoint `POST /login` |

---

## Geração de Token

O token é gerado pelo endpoint `POST /login` quando credenciais válidas são fornecidas:

```http
POST /login
Content-Type: application/json

{
  "userName": "<usuario>",
  "password": "<senha>"
}
```

Resposta de sucesso (HTTP 200):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs..."
}
```

---

## Fluxo de Validação de Token

O `AuthenticateFilter` executa a seguinte sequência para cada requisição a endpoints protegidos:

1. Extrai o valor do header `Authorization: Bearer <token>`
2. Valida assinatura, expiração e integridade do token via `ITokenService`
3. Se **válido**: extrai `id` e `userName` das claims, armazena `AuthenticatedUser` em `HttpContext.Items`, enriquece o Serilog LogContext com `UserId` e `UserName`
4. Se **inválido ou ausente**: retorna HTTP 401 com corpo em formato Problem Details (RFC 7807)

O enrichment de logs é **transparente para Features** — endpoints apenas aplicam `[Authenticate]` na classe do Controller, sem lógica de autenticação no corpo do endpoint.

---

## Proteção de Endpoints

Para proteger um endpoint, basta decorar o Controller com o atributo `[Authenticate]`:

```csharp
[Authenticate]
[ApiController]
[Route("exemplo")]
public class ExemploEndpoint(...) : ControllerBase
{
    // Nenhuma lógica de auth aqui — AuthenticateFilter cuida de tudo
}
```

---

## Endpoints Públicos

| Endpoint | Rota | Motivo |
|---|---|---|
| Login | `POST /login` | Precisa estar acessível para gerar o token |
| Health Check | `GET /health` | Verificação de disponibilidade não deve exigir autenticação |

---

## Como Utilizar

1. Obter o token via `POST /login` com credenciais válidas
2. Incluir o token no header `Authorization` das requisições:

```http
GET /exemplo
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

---

## Respostas de Erro

### Token ausente ou inválido (HTTP 401)

```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Token inválido ou ausente"
}
```

### Token expirado (HTTP 401)

```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "detail": "Token expirado"
}
```

---

## Componentes de Segurança

Todos os componentes residem em `Infra/Security/`:

| Componente | Arquivo | Responsabilidade |
|---|---|---|
| `ITokenService` | `ITokenService.cs` | Contrato de geração e validação de JWT |
| `TokenService` | `TokenService.cs` | Implementação JWT HS256: geração e validação de Bearer Token |
| `AuthenticatedUser` | `AuthenticatedUser.cs` | Modelo do usuário autenticado extraído do token (`Id`, `UserName`) |
| `AuthenticateFilter` | `AuthenticateFilter.cs` | `IAsyncActionFilter`: valida Bearer Token, retorna 401 se inválido, enriquece logs com `UserId` e `UserName`, armazena `AuthenticatedUser` em `HttpContext.Items` |
| `AuthenticateAttribute` | `AuthenticateAttribute.cs` | `TypeFilterAttribute`: decorador `[Authenticate]` aplicado nos Controllers para ativar `AuthenticateFilter` via DI |

---

## Observações Técnicas

- `JwtSecurityTokenHandler` usa reflection, gerando potenciais avisos AOT durante `dotnet publish` (trade-off conhecido, registrado em DA-009)
- `dotnet build` e `dotnet run` funcionam normalmente — avisos só aparecem em `dotnet publish --aot`
- O `AuthenticateFilter` armazena `AuthenticatedUser` em `HttpContext.Items` para que a camada de cache possa identificar o usuário autenticado via `IHttpContextAccessor`

---

## Referências

- [Arquitetura](/Starter.Template.AOT/governanca/arquitetura/) — fluxo de requisição com AuthenticateFilter
- [Testes](/Starter.Template.AOT/governanca/testes/) — testes de segurança e validação de token
