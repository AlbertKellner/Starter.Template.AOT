# Escanear Estrutura do Drive

## Descrição
Funcionalidade que escaneia um drive selecionado e retorna a estrutura hierárquica completa de pastas e arquivos, com tamanhos calculados e cores atribuídas para visualização. Migrada do repositório GeminiClone (ArquivosDoDisco).

## Autenticação
Não requer autenticação.

## Contrato de Entrada
- **Método HTTP**: `GET`
- **Rota**: `/disk-structure/{selectedDrive}`
- **Parâmetros de rota**:
  - `selectedDrive` (string, obrigatório): letra do drive sem ":" (ex: `C`, `D`)
- **Headers obrigatórios**: nenhum
- **Body**: nenhum

## Contrato de Saída

### HTTP 200 — Estrutura retornada
```json
{
  "name": "root",
  "value": 1073741824,
  "color": "#FF6666",
  "children": [
    {
      "name": "Windows",
      "value": 536870912,
      "color": "#AA8888",
      "children": [],
      "formattedSize": "512 MB"
    }
  ],
  "formattedSize": "1 GB"
}
```

### HTTP 204 — Drive vazio ou inacessível
Sem body.

## Comportamento
- Recebe a letra do drive e monta o caminho completo (ex: `C:/`)
- Escaneia recursivamente todas as pastas e arquivos usando `System.IO`
- Calcula o tamanho de cada pasta somando os tamanhos dos filhos
- Ordena filhos por tamanho (decrescente) em todos os níveis
- Aplica cores hierárquicas: cores base para itens de primeiro nível, interpolação para cinza conforme a profundidade
- Pastas sem permissão de acesso são silenciosamente ignoradas
- Se o drive estiver vazio ou inacessível, retorna 204

## Testes Automatizados
- `DiskStructureGetByDriveUseCaseTests.ExecuteAsync_ValidDrive_ReturnsStructureWithColors`

## BDD
Nenhum cenário BDD definido para esta funcionalidade.

## Referências
- [Arquitetura](Governance-Architecture) — estrutura Vertical Slice
- [Listar Drives](Feature-DiskDrivesGetAll) — funcionalidade relacionada
- [Buscar Pasta](Feature-DiskStructureGetByFolder) — funcionalidade relacionada
