# Conversão de Número para String

## Descrição

Endpoint de teste que converte um número inteiro para sua representação textual em português. Aceita os valores 1 e 2, retornando "Um" e "Dois" respectivamente. Deve ser consultado como exemplo de implementação de Query com parâmetro de rota.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| `number` | `int` (rota) | Sim | Número a ser convertido (valores aceitos: 1, 2) |

**Método**: `GET`
**Rota**: `/number-string/{number}`

## Contrato de Saída

### HTTP 200 — Sucesso

```json
{
  "value": "Um"
}
```

| Campo | Tipo | Descrição |
|---|---|---|
| `value` | `string` | Representação textual do número |

### HTTP 500 — Número não suportado

Retorna Problem Details (RFC 7807) quando o número não é 1 nem 2.

## Comportamento

- Se `number` = 1, retorna `"Um"`
- Se `number` = 2, retorna `"Dois"`
- Qualquer outro valor gera `ArgumentOutOfRangeException`, capturada pelo `GlobalExceptionHandler`

## Testes Automatizados

- `Execute_Number1_ReturnsUm` — valida retorno "Um" para entrada 1
- `Execute_Number2_ReturnsDois` — valida retorno "Dois" para entrada 2
- `Execute_InvalidNumber_ThrowsArgumentOutOfRangeException` — valida exceção para valores inválidos

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture) — estrutura de Slices e componentes
- [Padrões de Desenvolvimento](Governance-Development-Patterns) — Vertical Slice Architecture
- [Qualidade](Governance-Quality) — tratamento de exceções via Problem Details
