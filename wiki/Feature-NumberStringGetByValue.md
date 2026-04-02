# NumberStringGetByValue

## Descrição
Endpoint de consulta que recebe um valor numérico inteiro e retorna sua representação textual em português. Endpoint de teste para validar a estrutura Vertical Slice do projeto.

## Autenticação
Não requer autenticação.

## Contrato de Entrada

| Campo | Tipo | Obrigatório | Descrição |
|-------|------|-------------|-----------|
| `value` | `int` (route parameter) | Sim | Número inteiro a ser convertido |

**Método**: `GET`
**Rota**: `/number-string/{value}`

## Contrato de Saída

### HTTP 200 — Sucesso
```json
{
  "value": "Um"
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `value` | `string` | Representação textual do número |

### HTTP 404 — Número não encontrado
```json
{
  "status": 404,
  "title": "Number not found",
  "detail": "No string representation found for value 3.",
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5"
}
```

## Comportamento

| Entrada | Saída | Status |
|---------|-------|--------|
| `1` | `"Um"` | 200 |
| `2` | `"Dois"` | 200 |
| Qualquer outro | Problem Details | 404 |

## Testes Automatizados

- `NumberStringGetByValueUseCaseTests` — 5 testes (valor 1, valor 2, valor não mapeado, logs de sucesso, logs de warning)
- `NumberStringGetByValueEndpointTests` — 4 testes (retorno OK, retorno 404, logs de requisição/retorno, log de warning)

## BDD
Nenhum cenário BDD definido para esta funcionalidade.

## Referências
- [Governance-Architecture](Governance-Architecture) — estrutura Vertical Slice
- [Governance-Code-Conventions](Governance-Code-Conventions) — padrão de logging SNP-001
