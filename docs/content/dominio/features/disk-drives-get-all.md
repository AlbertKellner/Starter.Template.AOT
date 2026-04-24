---
title: "Listar Drives do Disco"
linkTitle: "Listar Drives"
weight: 20
description: "Lista drives disponíveis no sistema"
---

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
- [Arquitetura](/Starter.Template.AOT/governanca/arquitetura/) — estrutura Vertical Slice
- [Escanear Drive](/Starter.Template.AOT/dominio/features/disk-structure-get-by-drive/) — funcionalidade relacionada
- [Buscar Pasta](/Starter.Template.AOT/dominio/features/disk-structure-get-by-folder/) — funcionalidade relacionada
