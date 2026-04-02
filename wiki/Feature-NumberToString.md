# NumberToString

## Descrição

Endpoint de teste que converte um número inteiro para sua representação textual em português. Consultar esta página para entender o contrato de entrada/saída e o comportamento esperado. Relacionado à página [Arquitetura](Governance-Architecture) para entender a estrutura de Vertical Slice adotada.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|---|---|
| **Método** | `GET` |
| **Rota** | `/number-to-string/{number}` |
| **Parâmetro** | `number` (inteiro, obrigatório, via rota) |

## Contrato de Saída

| Status | Body | Descrição |
|---|---|---|
| `200 OK` | `{ "value": "Um" }` | Número mapeado com sucesso |
| `422 Unprocessable Entity` | — | Número sem mapeamento definido |

## Comportamento

| Entrada | Saída |
|---|---|
| `1` | `{ "value": "Um" }` |
| `2` | `{ "value": "Dois" }` |
| `3` | `{ "value": "Três" }` |
| `4` | `{ "value": "Quatro" }` |
| `5` | `{ "value": "Cinco" }` |

Para qualquer outro valor inteiro, retorna HTTP 422.

## Testes Automatizados

- `ExecuteAsync_DeveRetornarTextoCorreto_QuandoNumeroCadastrado` — valida 1→"Um", 2→"Dois", 3→"Três", 4→"Quatro", 5→"Cinco"
- `ExecuteAsync_DeveRetornarNull_QuandoNumeroNaoMapeado` — valida retorno null para número não mapeado
- `ExecuteAsync_DeveRegistrarLogDeConversao_QuandoNumeroCadastrado` — valida log de entrada
- `ExecuteAsync_DeveRegistrarLogDeRetorno_QuandoNumeroCadastrado` — valida log de saída
- `ExecuteAsync_DeveRegistrarLogsComPrefixoCorreto` — valida padrão SNP-001

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture) — estrutura Vertical Slice
- [Padrões de Desenvolvimento](Governance-Development-Patterns) — padrões UseCase e Endpoint
- [Regras de Negócio](Domain-Business-Rules) — índice de regras
