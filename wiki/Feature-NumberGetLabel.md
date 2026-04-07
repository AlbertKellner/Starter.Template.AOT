# NumberGetLabel

## Descrição

Documenta o endpoint de mapeamento de número para label textual (`GET /numbers/{number}`). Esta página cobre o contrato de entrada e saída e o comportamento esperado. Consulte quando precisar entender o mapeamento de números para labels ou ao adicionar novos valores mapeados. Relaciona-se com [Arquitetura](Governance-Architecture) (Vertical Slice Query).

## Autenticação

**Não requer autenticação.** Este endpoint é acessível publicamente.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| **Método** | `GET` |
| **Rota** | `/numbers/{number}` |
| **Headers** | Nenhum obrigatório |
| **Body** | Nenhum |

### Parâmetros de Rota

| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| `number` | `int` | Sim | Número a ser mapeado para label. Valores aceitos: `1`, `2` |

## Contrato de Saída

| Status | Corpo | Descrição |
|--------|-------|-----------|
| `200 OK` | `{ "label": "Um" }` | Número `1` mapeado com sucesso |
| `200 OK` | `{ "label": "Dois" }` | Número `2` mapeado com sucesso |
| `500 Internal Server Error` | Problem Details (RFC 7807) | Número não mapeado (fora do conjunto `{1, 2}`) |

## Comportamento

- Recebe um número inteiro via parâmetro de rota.
- Mapeia `1` → `"Um"` e `2` → `"Dois"`.
- Lança `ArgumentOutOfRangeException` para qualquer número fora do conjunto mapeado, resultando em `HTTP 500` via `GlobalExceptionHandler`.

## Testes Automatizados

Nenhum teste automatizado presente no repositório.

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture) — estrutura de Slice Query
- [Qualidade](Governance-Quality) — tratamento de exceções via GlobalExceptionHandler
