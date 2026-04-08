# NumberToStringGet

## Descrição

Converte um número inteiro para sua representação textual em português. Endpoint de teste que suporta os valores 1 ("Um") e 2 ("Dois"). Consultar esta página ao entender o contrato ou comportamento desta feature.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|---|---|
| Método HTTP | `GET` |
| Rota | `/number-to-string/{number}` |
| Parâmetro de rota | `number` (int, obrigatório) |

## Contrato de Saída

| Status | Body | Descrição |
|---|---|---|
| `200 OK` | `"Um"` ou `"Dois"` (string JSON) | Número convertido com sucesso |
| `500 Internal Server Error` | Problem Details (RFC 7807) | Número não suportado (diferente de 1 ou 2) |

## Comportamento

- Se `number` for `1`, retorna `"Um"`
- Se `number` for `2`, retorna `"Dois"`
- Qualquer outro valor lança `ArgumentOutOfRangeException`, capturada pelo `GlobalExceptionHandler` e retornada como Problem Details

## Testes Automatizados

- `Execute_Number1_ReturnsUm` — verifica retorno "Um" para entrada 1
- `Execute_Number2_ReturnsDois` — verifica retorno "Dois" para entrada 2
- `Execute_UnsupportedNumber_ThrowsArgumentOutOfRangeException` — verifica exceção para valor não suportado

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture)
- [Padrões de Desenvolvimento](Governance-Development-Patterns)
