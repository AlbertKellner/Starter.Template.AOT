# Conversão de Número para Texto (NumberStringGetByNumber)

## Descrição

Endpoint de consulta que recebe um número inteiro (1 ou 2) e retorna sua representação textual em português ("Um" ou "Dois"). Feature de teste para validar o padrão Vertical Slice do template.

## Autenticação

Não requer autenticação.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| **Método** | `GET` |
| **Rota** | `/number-string/{number}` |
| **Parâmetro** | `number` (int, obrigatório, via rota) |
| **Headers** | Nenhum obrigatório |
| **Body** | Nenhum |

## Contrato de Saída

### 200 OK

```json
{
  "numberAsString": "Um"
}
```

### 400 Bad Request

Retornado quando o número não é 1 nem 2.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Bad Request",
  "status": 400,
  "detail": "The number 3 is not supported. Only 1 and 2 are valid."
}
```

## Comportamento

- Recebe `1` → retorna `"Um"`
- Recebe `2` → retorna `"Dois"`
- Qualquer outro valor → retorna 400 Bad Request com Problem Details

## Testes Automatizados

- `NumberStringGetByNumberUseCaseTests` — testa conversão de 1, 2 e números inválidos; verifica logs
- `NumberStringGetByNumberEndpointTests` — testa retorno 200 com output correto e 400 para inválidos; verifica logs

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governança de Arquitetura](Governance-Architecture)
- [Padrões de Desenvolvimento](Governance-Development-Patterns)
