# NumberStringGet — Conversão de Número para String

## Descrição

Endpoint de consulta que recebe um número inteiro via rota e retorna sua representação textual em português. Atualmente mapeia os valores 1 ("Um") e 2 ("Dois"). Endpoint de teste sem autenticação.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| **Método** | `GET` |
| **Rota** | `/number-string/{number}` |
| **Parâmetros de rota** | `number` (int, obrigatório) |
| **Headers obrigatórios** | Nenhum |
| **Body** | Nenhum |

## Contrato de Saída

### HTTP 200 — Sucesso

```json
{
  "value": "Um"
}
```

### HTTP 404 — Número não mapeado

Retornado quando o número não possui representação textual.

## Comportamento

- Se `number` = 1, retorna `{ "value": "Um" }`
- Se `number` = 2, retorna `{ "value": "Dois" }`
- Para qualquer outro valor, retorna HTTP 404

## Testes Automatizados

- `Execute_DeveRetornarUm_QuandoNumeroFor1`
- `Execute_DeveRetornarDois_QuandoNumeroFor2`
- `Execute_DeveRetornarNull_QuandoNumeroNaoMapeado`
- `Execute_DeveRegistrarLogInformation_QuandoNumeroMapeado`
- `Execute_DeveRegistrarLogWarning_QuandoNumeroNaoMapeado`

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governança Arquitetural](Governance-Architecture)
- [Regras de Negócio](Domain-Business-Rules)
