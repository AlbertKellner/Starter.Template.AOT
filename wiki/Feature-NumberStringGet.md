# NumberStringGet

## Descrição

Endpoint de teste que recebe um número inteiro como parâmetro de rota e retorna sua representação textual em português. Suporta os valores 1 ("Um") e 2 ("Dois"). Valores não mapeados retornam HTTP 400 com ProblemDetails. Consultar esta página para entender o contrato de entrada/saída e o comportamento de validação.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Tipo | Obrigatoriedade | Descrição |
|---|---|---|---|
| `value` | `int` (route param) | Obrigatório | Número a ser convertido. Valores suportados: 1, 2 |

**Método:** `GET`
**Rota:** `/number-strings/{value}`

Exemplo:
```
GET /number-strings/1
GET /number-strings/2
```

## Contrato de Saída

### HTTP 200 — Valor mapeado com sucesso

```json
{
  "value": "Um"
}
```

| Campo | Tipo | Descrição |
|---|---|---|
| `value` | `string` | Representação textual do número em português |

### HTTP 400 — Valor não suportado

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Unsupported value",
  "status": 400,
  "detail": "Value '99' is not supported. Use 1 or 2."
}
```

## Comportamento

| Entrada | Resultado | Status |
|---|---|---|
| `value = 1` | `{"value": "Um"}` | 200 |
| `value = 2` | `{"value": "Dois"}` | 200 |
| Qualquer outro inteiro | ProblemDetails com mensagem de erro | 400 |

## Testes Automatizados

| Teste | Tipo | Cobertura |
|---|---|---|
| `Execute_ComValor1_DeveRetornarUm` | Unitário (UseCase) | Mapeamento 1 → "Um" |
| `Execute_ComValor2_DeveRetornarDois` | Unitário (UseCase) | Mapeamento 2 → "Dois" |
| `Execute_ComValorNaoMapeado_DeveRetornarNulo` | Unitário (UseCase) | Valor inválido retorna null |
| `Execute_ComValorValido_DeveRegistrarLogInformationNoInicio` | Unitário (UseCase) | Padrão SNP-001 — log de entrada |
| `Execute_ComValorValido_DeveRegistrarLogInformationNoRetorno` | Unitário (UseCase) | Padrão SNP-001 — log de saída |
| `Execute_ComValorNaoMapeado_DeveRegistrarLogWarning` | Unitário (UseCase) | Padrão SNP-001 — warning em valor inválido |
| `Execute_DeveRegistrarLogsComPrefixoCorreto` | Unitário (UseCase) | Prefixo `[Classe][Método]` obrigatório |

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Arquitetura](Governance-Architecture) — padrões de Vertical Slice e nomenclatura de features
- [Padrões de Desenvolvimento](Governance-Development-Patterns) — UseCase, Endpoint, Models
- [Observabilidade](Governance-Observability) — padrão de logging SNP-001
