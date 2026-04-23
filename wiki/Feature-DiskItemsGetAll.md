# Feature: DiskItemsGetAll

## Descrição

Retorna a árvore completa de pastas e arquivos de uma unidade de disco identificada por índice. Cada item recebe uma cor gerada algoritmicamente para visualização hierárquica.

## Autenticação

Requer autenticação: Não.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| Método | `GET` |
| Rota | `/disk-items/{driveIndex}` |
| Headers | Nenhum obrigatório |
| Body | Nenhum |

| Parâmetro | Tipo | Local | Descrição |
|-----------|------|-------|-----------|
| `driveIndex` | `int` | Rota | Índice da unidade obtido em `GET /disk-drives` |

## Contrato de Saída

### HTTP 200 — Sucesso

```json
{
  "name": "root",
  "value": 1073741824,
  "color": "#FF6633",
  "formattedSize": "1 GB",
  "children": [
    {
      "name": "home",
      "value": 536870912,
      "color": "#FFB399",
      "formattedSize": "512 MB",
      "children": null
    }
  ]
}
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `name` | `string` | Nome do item (pasta ou arquivo) |
| `value` | `long` | Tamanho em bytes |
| `color` | `string` | Cor hex atribuída para visualização |
| `formattedSize` | `string` | Tamanho formatado (ex: `512 MB`) |
| `children` | `array\|null` | Filhos (recursivo); `null` se nenhum filho |

## Comportamento

- Escaneia a unidade de disco até profundidade máxima de 6 níveis
- Aplica paleta de 44 cores aos itens de primeiro nível
- Para níveis mais profundos, interpola para cinza com fator `level × 0.2`
- Alterna entre saturação (`count par`) e dessaturação (`count ímpar`) em cada nível
- Ordena itens por tamanho decrescente em todos os níveis

## Testes Automatizados

- `ExecuteAsync_DeveRetornarOutputComNomeRoot` — verifica que o output tem nome "root"
- `ExecuteAsync_DeveAtribuirCoresAosFilhosRaiz` — verifica que itens de nível 1 possuem cor
- `ExecuteAsync_DeveRegistrarLogInformationNoInicio` — verifica log de entrada
- `ExecuteAsync_DeveRegistrarLogInformationNoRetorno` — verifica log de saída

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governance-Architecture](Governance-Architecture)
- [Feature-DiskDrivesGetAll](Feature-DiskDrivesGetAll)
- [Feature-DiskItemGetByFolder](Feature-DiskItemGetByFolder)
