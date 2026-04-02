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

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `value` | `int` | Valor numérico recebido |
| `text` | `string` | Representação textual do número |

### HTTP 500 — Valor não mapeado

Retorna Problem Details (RFC 7807) quando o valor não possui mapeamento.

## Comportamento

- Valor `1` retorna texto `"Um"`
- Valor `2` retorna texto `"Dois"`
- Qualquer outro valor resulta em erro (valor não mapeado)

## Testes Automatizados

- `NumberStringGetUseCaseTests` — 6 testes (retorno correto, exceção para valores não mapeados, verificação de logs SNP-001)
- `NumberStringGetEndpointTests` — 3 testes (retorno OK, verificação de logs de entrada e saída)

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture) — estrutura Vertical Slice
- [Padrões de Desenvolvimento](Governance-Development-Patterns) — padrões de UseCase e Endpoint
- [Convenções de Código](Governance-Code-Conventions) — logging SNP-001
