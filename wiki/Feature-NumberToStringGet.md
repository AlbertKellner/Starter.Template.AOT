# Conversão de Número para String

## Descrição
Endpoint de teste que converte um número inteiro para sua representação textual em português. Aceita os valores 1 e 2, retornando "Um" e "Dois" respectivamente. Consultar esta página para entender o contrato e o comportamento da funcionalidade.

## Autenticação
Não requer autenticação.

## Contrato de Entrada

| Item | Valor |
|------|-------|
| Método HTTP | `GET` |
| Rota | `/number-to-string/{number}` |
| Parâmetro `number` | `int` — obrigatório, via rota. Valores aceitos: `1`, `2` |

## Contrato de Saída

### HTTP 200 — Sucesso
```json
{
  "result": "Um"
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `result` | `string` | Representação textual do número em português |

### HTTP 500 — Número não suportado
Retorna Problem Details (RFC 7807) quando o número não é 1 nem 2.

## Comportamento
- Se `number` for `1`, retorna `"Um"`
- Se `number` for `2`, retorna `"Dois"`
- Para qualquer outro valor, lança `ArgumentOutOfRangeException` capturada pelo `GlobalExceptionHandler`, que retorna Problem Details

## Testes Automatizados
- `NumberToStringGetUseCaseTests.Execute_Number1_ReturnsUm` — valida retorno "Um" para entrada 1
- `NumberToStringGetUseCaseTests.Execute_Number2_ReturnsDois` — valida retorno "Dois" para entrada 2
- `NumberToStringGetUseCaseTests.Execute_UnsupportedNumber_ThrowsArgumentOutOfRangeException` — valida exceção para valores não suportados (0, 3, -1)

## BDD
Nenhum cenário BDD definido para esta funcionalidade.

## Referências
- [Arquitetura](Governance-Architecture) — estrutura de Slices e componentes
- [Regras de Negócio](Domain-Business-Rules) — índice de regras
- [Health Check](Feature-Health) — outro endpoint de referência
