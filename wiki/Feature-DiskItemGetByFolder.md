# Feature: DiskItemGetByFolder

## Descrição

Retorna a subárvore de uma pasta específica dentro de uma unidade de disco. Útil para explorar o conteúdo de um caminho sem recarregar a árvore completa.

## Autenticação

Requer autenticação: Não.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| Método | `GET` |
| Rota | `/disk-items/{driveIndex}/folder/{*folderPath}` |
| Headers | Nenhum obrigatório |
| Body | Nenhum |

| Parâmetro | Tipo | Local | Descrição |
|-----------|------|-------|-----------|
| `driveIndex` | `int` | Rota | Índice da unidade obtido em `GET /disk-drives` |
| `folderPath` | `string` | Rota (catch-all) | Caminho relativo da pasta (ex: `home/user/Documents`) |

## Contrato de Saída

### HTTP 200 — Pasta encontrada

```json
{
  "name": "Documents",
  "value": 10485760,
  "formattedSize": "10 MB",
  "children": [
    {
      "name": "report.pdf",
      "value": 524288,
      "formattedSize": "512 KB",
      "children": null
    }
  ]
}
```

### HTTP 404 — Pasta não encontrada

Retorna HTTP 404 sem body quando o caminho não existe na estrutura.

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `name` | `string` | Nome da pasta |
| `value` | `long` | Tamanho em bytes |
| `formattedSize` | `string` | Tamanho formatado |
| `children` | `array\|null` | Filhos (recursivo); `null` se nenhum filho |

## Comportamento

- Escaneia a unidade completa antes de localizar a pasta
- Navega o caminho segmento por segmento (separador `/`)
- Retorna HTTP 404 se qualquer segmento do caminho não for encontrado
- Não aplica cores (diferente de [Feature-DiskItemsGetAll](Feature-DiskItemsGetAll))

## Testes Automatizados

- `ExecuteAsync_ComPastaExistente_DeveRetornarOutput` — verifica retorno com pasta existente
- `ExecuteAsync_ComPastaInexistente_DeveRetornarNull` — verifica retorno null para pasta ausente
- `ExecuteAsync_ComPastaExistente_DeveMapearFilhos` — verifica mapeamento correto dos filhos
- `ExecuteAsync_DeveRegistrarLogInformationNaBusca` — verifica log de busca

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governance-Architecture](Governance-Architecture)
- [Feature-DiskDrivesGetAll](Feature-DiskDrivesGetAll)
- [Feature-DiskItemsGetAll](Feature-DiskItemsGetAll)
