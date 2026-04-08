# NumberToStringGet

## Resumo

Endpoint de teste que converte um número inteiro para sua representação textual em português. Recebe 1 e retorna "Um", recebe 2 e retorna "Dois". Qualquer outro número retorna 404.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|---|---|
| Método | `GET` |
| Rota | `/number-to-string/{number}` |
| Parâmetro | `number` (int, via rota) |

## Contrato de Saída

| Status | Body |
|---|---|
| `200 OK` | `{ "value": "Um" }` ou `{ "value": "Dois" }` |
| `404 Not Found` | Sem body |

## Comportamento

- Se `number` = 1, retorna `{ "value": "Um" }`
- Se `number` = 2, retorna `{ "value": "Dois" }`
- Qualquer outro valor retorna HTTP 404

## Testes Automatizados

- `NumberToStringGetUseCaseTests.Execute_Number1_ReturnsUm`
- `NumberToStringGetUseCaseTests.Execute_Number2_ReturnsDois`
- `NumberToStringGetUseCaseTests.Execute_UnmappedNumber_ReturnsNull`

## BDD

Nenhum cenário BDD definido para esta funcionalidade (endpoint de teste).
