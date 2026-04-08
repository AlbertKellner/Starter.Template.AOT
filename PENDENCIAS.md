# Pendências e Bloqueadores

Este arquivo documenta bloqueadores técnicos e pendências de configuração identificados durante o desenvolvimento.

---

## Bloqueadores Ativos

### 1. Docker não instalado no ambiente local

**Status**: Bloqueador ativo

**Impacto**: Impede execução dos passos 4–8 do pipeline de validação pré-commit (escopo Código)

**Descrição**: O ambiente local não possui Docker instalado. Os passos que dependem de `docker compose` não podem ser executados:
- Passo 4: `docker compose up -d` (publicar Release/Native AOT)
- Passo 5: Aguardar `/health` responder HTTP 200
- Passo 6: Validar endpoints via chamada HTTP real
- Passo 7: Exibir logs do container
- Passo 8: `docker compose down`

**Workaround atual**: Validação em modo Debug (passos 1–3) é executada localmente. Validação completa (Release/AOT + Docker) é executada apenas no CI.

**Resolução**: Instalar Docker Desktop ou Docker Engine no ambiente local.

**Referências**:
- `.claude/rules/environment-readiness.md` — protocolo de ambiente não pronto
- `CLAUDE.md` — pipeline de validação pré-commit

---

### 2. Controllers MVC incompatíveis com Debug + Native AOT

**Status**: ✅ Documentado como trade-off aceito (DA-008, DA-009)

**Descrição**: Controllers MVC usam reflection, gerando avisos de incompatibilidade AOT durante `dotnet publish`. Em modo Debug (`dotnet build`, `dotnet run`, `dotnet test`), Controllers funcionam normalmente. Este trade-off é aceito permanentemente conforme DA-008 e DA-009. Avisos AOT em publish são comportamento esperado e não requerem ação.

**Referências**:
- `Instructions/architecture/architecture-decisions.md` — DA-008 (Controllers MVC) e DA-009 (Native AOT)

---

### 3. Datadog Application Key ausente

**Status**: Bloqueador parcial

**Impacto**: Impede uso de ferramentas MCP do Datadog que requerem `DD_APP_KEY`

**Descrição**: O arquivo `.env` contém `DD_API_KEY` mas não contém `DD_APP_KEY`. O servidor MCP Datadog configurado em `.mcp.json` requer ambas as chaves.

**Comportamento atual**:
- Logs fluem para o Datadog via `DatadogHttpSink` (usa apenas `DD_API_KEY`)
- Ferramentas MCP do Datadog não podem ser usadas (requerem `DD_APP_KEY`)

**Workaround atual**: Logs podem ser visualizados no Datadog UI manualmente. Ferramentas MCP não estão disponíveis.

**Resolução**: Obter `DD_APP_KEY` no Datadog UI e adicionar ao `.env`.

**Referências**:
- `.mcp.json` — configuração do servidor MCP Datadog
- `.env.example` — template de variáveis de ambiente

---

### 4. Git Bash configurado (resolvido)

**Status**: ✅ Resolvido

**Descrição**: O ambiente estava configurado com PowerShell como shell padrão. Scripts de governança (`.sh`) requerem Bash.

**Resolução aplicada**: Usuário configurou Git Bash como shell padrão.

**Data de resolução**: 2026-04-06

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-04-06 | Criado: documentação de bloqueadores identificados durante tentativa de implementação de TestGet | Análise de causa-raiz |
| 2026-04-06 | Git Bash marcado como resolvido | Confirmação do usuário |
| 2026-04-08 | Item 2 corrigido: informação factualmente incorreta sobre Minimal APIs removida; trade-off Controllers MVC vs AOT documentado como aceito (DA-008, DA-009) | Auditoria de governança — rodada 4 |

