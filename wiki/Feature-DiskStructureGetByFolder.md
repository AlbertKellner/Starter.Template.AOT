# Buscar Estrutura de Pasta Específica

## Descrição
Funcionalidade que escaneia um drive e retorna a estrutura hierárquica de uma pasta específica dentro dele. Migrada do repositório GeminiClone (ArquivosDoDisco).

## Autenticação
Não requer autenticação.

## Contrato de Entrada
- **Método HTTP**: `GET`
- **Rota**: `/disk-structure/{selectedDrive}/folder/{selectedFolder}`
- **Parâmetros de rota**:
  - `selectedDrive` (string, obrigatório): letra do drive sem ":" (ex: `C`, `D`)
  - `selectedFolder` (string, obrigatório): caminho relativo da pasta dentro do drive (ex: `Users`, `Windows/System32`)
- **Headers obrigatórios**: nenhum
- **Body**: nenhum

## Contrato de Saída

### HTTP 200 — Pasta encontrada
```json
{
  "name": "System32",
  "value": 268435456,
  "color": "",
  "children": [...],
  "formattedSize": "256 MB"
}
```

### HTTP 204 — Pasta não encontrada
Sem body.

## Comportamento
- Recebe a letra do drive e o caminho da pasta
- Escaneia o drive completo e busca a pasta pelo caminho relativo
- A busca é case-insensitive e navega segmento por segmento
- Se a pasta não for encontrada, retorna 204
- Pastas sem permissão de acesso são silenciosamente ignoradas

## Testes Automatizados
- `DiskStructureGetByFolderUseCaseTests.ExecuteAsync_FolderExists_ReturnsStructure`
- `DiskStructureGetByFolderUseCaseTests.ExecuteAsync_FolderNotFound_ReturnsNull`

## BDD
Nenhum cenário BDD definido para esta funcionalidade.

## Referências
- [Arquitetura](Governance-Architecture) — estrutura Vertical Slice
- [Listar Drives](Feature-DiskDrivesGetAll) — funcionalidade relacionada
- [Escanear Drive](Feature-DiskStructureGetByDrive) — funcionalidade relacionada
