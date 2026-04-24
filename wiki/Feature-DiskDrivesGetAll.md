# Listar Drives do Disco

## Descrição
Funcionalidade que retorna a lista de drives (unidades de disco) disponíveis no sistema operacional onde a aplicação está em execução. Deve ser consultada para entender o contrato de entrada/saída e o comportamento do endpoint de listagem de drives.

## Autenticação
Não requer autenticação.

## Contrato de Entrada
- **Método HTTP**: `GET`
- **Rota**: `/disk-drives`
- **Headers obrigatórios**: nenhum
- **Body**: nenhum

## Contrato de Saída

### HTTP 200 — Drives encontrados
```json
{
  "drives": ["C:\\", "D:\\"]
}
```

### HTTP 204 — Nenhum drive encontrado
Sem body.

## Comportamento
- Lista todos os drives disponíveis no sistema via `DriveInfo.GetDrives()`
- Retorna os nomes dos drives como lista de strings
- Se nenhum drive for encontrado, retorna 204 No Content

## Testes Automatizados
- `DiskDrivesGetAllUseCaseTests.Execute_WithDrivesAvailable_ReturnsDriveList`
- `DiskDrivesGetAllUseCaseTests.Execute_WithNoDrives_ReturnsEmptyList`

## BDD
Nenhum cenário BDD definido para esta funcionalidade.

## Referências
- [Arquitetura](Governance-Architecture) — estrutura Vertical Slice
- [Escanear Drive](Feature-DiskStructureGetByDrive) — funcionalidade relacionada
- [Buscar Pasta](Feature-DiskStructureGetByFolder) — funcionalidade relacionada
