# NumberGetText

## Resumo
Retorna o texto correspondente a um número inteiro. Endpoint de teste para validação do pipeline.

## Autenticação
Não requer autenticação.

## Contrato de Entrada
- Método: `GET`
- Rota: `/number-texts/{number}`
- Parâmetros: `number` (int, route parameter)

## Contrato de Saída
- `200 OK` — `{ "text": "Um" }` ou `{ "text": "Dois" }`
- `404 Not Found` — número não mapeado

## Comportamento
- Recebe 1 → retorna "Um"
- Recebe 2 → retorna "Dois"
- Qualquer outro valor → 404

## Testes Automatizados
Nenhum teste automatizado presente no repositório

## BDD
Nenhum cenário BDD definido para esta funcionalidade
