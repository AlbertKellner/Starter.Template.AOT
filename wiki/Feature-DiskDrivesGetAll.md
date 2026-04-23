# Feature: DiskDrivesGetAll

## Descrição

Lista todas as unidades de disco disponíveis no sistema. Retorna índice, nome e tipo de cada unidade. O índice é utilizado como parâmetro nos endpoints de exploração de disco.

## Autenticação

Requer autenticação: Não.

## Contrato de Entrada

| Campo | Valor |
|-------|-------|
| Método | `GET` |
| Rota | `/disk-drives` |
| Headers | Nenhum obrigatório |
| Body | Nenhum |

## Contrato de Saída

### HTTP 200 — Sucesso

```json
[
  {
    "index": 0,
    "name": "/",
    "driveType": "Fixed"
  }
]
```

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `index` | `int` | Índice da unidade (usar como parâmetro em `/disk-items/{driveIndex}`) |
| `name` | `string` | Nome da unidade de disco |
| `driveType` | `string` | Tipo da unidade (Fixed, Network, Removable, etc.) |

## Comportamento

- Lista todas as unidades retornadas por `DriveInfo.GetDrives()`
- Atribui índice sequencial a partir de 0 para cada unidade
- Não filtra por tipo de unidade

## Testes Automatizados

- `Execute_DeveRetornarListaDeDrives` — verifica que a lista não está vazia
- `Execute_DeveAtribuirIndicesSequenciais` — verifica que os índices são 0, 1, 2, …
- `Execute_DeveRegistrarLogInformationNoInicio` — verifica log de entrada
- `Execute_DeveRegistrarLogInformationNoRetorno` — verifica log de saída

## BDD

Nenhum cenário BDD definido para esta funcionalidade.

## Referências

- [Governance-Architecture](Governance-Architecture)
- [Feature-DiskItemsGetAll](Feature-DiskItemsGetAll)
- [Feature-DiskItemGetByFolder](Feature-DiskItemGetByFolder)
