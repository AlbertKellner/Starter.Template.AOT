# NumberGetByValue

## Resumo

Endpoint de teste que recebe um número inteiro e retorna sua representação textual em português. Aceita os valores 1 e 2; qualquer outro valor resulta em erro 500 (Problem Details RFC 7807).

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|---|---|
| Método | `GET` |
| Rota | `/numbers/{number}` |
| Parâmetro | `number` (int, obrigatório, via rota) |
| Headers | Nenhum obrigatório |
| Body | Não aplicável |

### Valores aceitos

| Entrada | Saída |
|---|---|
| `1` | `"Um"` |
| `2` | `"Dois"` |

## Contrato de Saída

### Sucesso (HTTP 200)

```json
{
  "value": "Um"
}
```

### Erro — valor não suportado (HTTP 500)

Retorna Problem Details (RFC 7807) com `ArgumentOutOfRangeException` capturada pelo `GlobalExceptionHandler`.

## Comportamento

- Classificação: Query (leitura, sem side effects)
- O UseCase converte o número via pattern matching
- Valores fora do conjunto {1, 2} lançam `ArgumentOutOfRangeException`
- Sem acesso a banco de dados ou serviços externos
- Sem cache

## Testes Automatizados

| Teste | Cenário | Resultado esperado |
|---|---|---|
| `Execute_Number1_ReturnsUm` | Entrada: 1 | Output com Value="Um" |
| `Execute_Number2_ReturnsDois` | Entrada: 2 | Output com Value="Dois" |
| `Execute_InvalidNumber_ThrowsArgumentOutOfRangeException` | Entrada: 3 | Lança ArgumentOutOfRangeException |
| `Execute_Number1_LogsProcessingAndResult` | Entrada: 1 | Logs de processamento e resultado emitidos |

## BDD

Nenhum cenário BDD definido — feature de teste sem regras de negócio.
