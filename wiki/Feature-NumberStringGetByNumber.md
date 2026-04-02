# Conversão de Número para String

## Descrição

Endpoint de consulta que converte um número inteiro para sua representação textual em português. Atualmente suporta os números 1 ("Um") e 2 ("Dois"). Números não mapeados retornam 404 Not Found. Não requer autenticação.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| **Método** | `GET` |
| **Rota** | `/number-string/{number}` |
| **Parâmetro** | `number` (int, obrigatório, via rota) |
| **Headers** | Nenhum obrigatório |
| **Body** | Não aplicável |

## Contrato de Saída

### HTTP 200 — Número mapeado com sucesso

```json
{
  "value": "Um"
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `value` | `string` | Representação textual do número em português |

### HTTP 404 — Número não mapeado

Retorna HTTP 404 sem corpo.

## Comportamento

- Se `number` for `1`, retorna `{"value": "Um"}` com status 200
- Se `number` for `2`, retorna `{"value": "Dois"}` com status 200
- Para qualquer outro valor de `number`, retorna 404 Not Found

## Testes Automatizados

| Teste | Classe | Validação |
|-------|--------|-----------|
| `Execute_DeveRetornarUm_QuandoNumeroFor1` | `NumberStringGetByNumberUseCaseTests` | UseCase retorna "Um" para 1 |
| `Execute_DeveRetornarDois_QuandoNumeroFor2` | `NumberStringGetByNumberUseCaseTests` | UseCase retorna "Dois" para 2 |
| `Execute_DeveRetornarNull_QuandoNumeroNaoMapeado` | `NumberStringGetByNumberUseCaseTests` | UseCase retorna null para 3 |
| `Execute_DeveRegistrarLogInformation_QuandoNumeroEncontrado` | `NumberStringGetByNumberUseCaseTests` | Logs de storytelling para sucesso |
| `Execute_DeveRegistrarLogWarning_QuandoNumeroNaoMapeado` | `NumberStringGetByNumberUseCaseTests` | Log warning para número não mapeado |
| `GetByNumber_DeveRetornarOk_QuandoNumeroEncontrado` | `NumberStringGetByNumberEndpointTests` | Endpoint retorna 200 + output |
| `GetByNumber_DeveRetornarNotFound_QuandoNumeroNaoMapeado` | `NumberStringGetByNumberEndpointTests` | Endpoint retorna 404 |
| `GetByNumber_DeveRegistrarLogInformation_QuandoNumeroEncontrado` | `NumberStringGetByNumberEndpointTests` | Logs do endpoint para sucesso |
| `GetByNumber_DeveRegistrarLogWarning_QuandoNumeroNaoEncontrado` | `NumberStringGetByNumberEndpointTests` | Log warning do endpoint |

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governança de Arquitetura](Governance-Architecture)
- [Padrões de Desenvolvimento](Governance-Development-Patterns)
- [Convenções de Código](Governance-Code-Conventions)
