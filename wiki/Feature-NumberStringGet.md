# NumberStringGet — Conversão de Número para Texto

## Descrição

Endpoint de consulta que converte um valor numérico inteiro para sua representação textual em português. Escopo limitado aos valores 1 e 2.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `value` | `int` (route parameter) | Sim | Valor numérico a ser convertido |

**Método**: `GET`
**Rota**: `/number-string/{value}`

## Contrato de Saída

### HTTP 200 — Sucesso

```json
{
  "value": 1,
  "text": "Um"
}
```

### HTTP 500 — Valor não mapeado

Retorna Problem Details (RFC 7807) quando o valor não possui mapeamento.

## Comportamento

- Valor `1` retorna texto `"Um"`
- Valor `2` retorna texto `"Dois"`
- Qualquer outro valor resulta em erro (valor não mapeado)

## Testes Automatizados

- `NumberStringGetUseCaseTests` — 6 testes
- `NumberStringGetEndpointTests` — 3 testes

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture)
- [Padrões de Desenvolvimento](Governance-Development-Patterns)
- [Convenções de Código](Governance-Code-Conventions)
