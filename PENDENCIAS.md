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

**Status**: Limitação técnica conhecida

**Impacto**: Impede uso de Controllers MVC em modo Debug quando Native AOT está habilitado

**Descrição**: O .NET 9 Native AOT não suporta Controllers MVC em modo Debug. A aplicação usa Minimal APIs (FastEndpoints) como padrão arquitetural. Controllers MVC não fazem parte da arquitetura definida.

**Comportamento esperado**:
- Modo Debug: Minimal APIs funcionam normalmente
- Modo Release/AOT: Minimal APIs funcionam normalmente
- Controllers MVC: Não suportados em nenhum modo quando AOT está habilitado

**Validação**:
- Passos 1–3 do pipeline (Debug): validam Minimal APIs
- Passos 4–8 do pipeline (Release/AOT + Docker): validam Minimal APIs em modo AOT

**Resolução**: Não há resolução necessária. Controllers MVC não são usados neste projeto.

**Referências**:
- `Instructions/architecture/patterns.md` — padrão Minimal APIs (FastEndpoints)
- `Instructions/architecture/technical-overview.md` — decisão de usar Minimal APIs

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

