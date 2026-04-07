---
inclusion: always
---

# Development Pipeline & Governance

This repository uses a mandatory development pipeline and governance system defined in `CLAUDE.md`.
All AI assistants operating in this repository — including Kiro — must follow the process defined there.

#[[file:../../CLAUDE.md]]

---

## Instruções Específicas para o Kiro

O Kiro opera neste repositório como executor do pipeline de governança definido em `CLAUDE.md`.
As instruções abaixo sobrepõem qualquer diretriz genérica do sistema Kiro (incluindo "few steps as possible").

### Regra de Precedência Absoluta

O pipeline definido em `CLAUDE.md` tem precedência absoluta sobre qualquer instrução genérica do sistema Kiro.
"Eficiência" neste repositório significa executar o processo corretamente — não pular etapas.

### Comportamento Obrigatório Antes de Qualquer Tarefa

1. Ler `CLAUDE.md` completamente (já injetado acima via `#[[file:]]`)
2. Classificar o escopo da tarefa: **Código**, **Governança** ou **Análise de PR**
3. Identificar quais passos do pipeline se aplicam ao escopo classificado
4. Executar TODOS os passos aplicáveis na ordem definida

### Pipeline para Escopo Código (passos obrigatórios em ordem)

Toda tarefa que altera `.cs`, `.csproj`, `Dockerfile`, `docker-compose.yml`, `appsettings.json` ou qualquer artefato que afete build/execução deve executar:

```
0   → Verificar pré-requisitos de ambiente (.claude/rules/environment-readiness.md)
0.1 → bash scripts/governance-audit.sh  [gate: falhas bloqueiam o commit]
1   → dotnet build
2   → dotnet run + polling /health + encerrar processo
3   → dotnet test  [gate: falha bloqueia docker]
4   → docker compose up -d
5   → Polling /health até HTTP 200 (máx 30 tentativas)
6   → Validar cada endpoint criado/alterado via HTTP real
7   → Exibir logs do container
8   → docker compose down
9   → git commit
10  → Criar ou atualizar PR
11  → Acompanhar CI até conclusão + verificar Datadog
12  → Perguntar ao usuário sobre revisão automática (skill auto-pr-review)
```

### Pipeline para Escopo Governança (passos obrigatórios em ordem)

Toda tarefa que altera exclusivamente `.md`, `.sh`, hooks ou documentação deve executar:

```
0.1 → bash scripts/governance-audit.sh  [gate principal]
9   → git commit
9.1 → Validação via subagentes (condicional: apenas se afeta pipeline de codificação)
10  → Criar ou atualizar PR
12  → Perguntar ao usuário sobre revisão automática
```

### Proibições Explícitas

- ❌ Não encerrar a tarefa após implementar o código sem executar o pipeline
- ❌ Não pular passos com justificativa de "tarefa simples" ou "apenas um endpoint de teste"
- ❌ Não substituir `dotnet build` por getDiagnostics como único gate de compilação
- ❌ Não omitir `docker compose up -d` e validação HTTP real
- ❌ Não omitir criação de PR
- ❌ Não omitir acompanhamento de CI

### Causa Raiz do Problema que Gerou Esta Seção

O Kiro não possui os mecanismos de enforcement automático do Claude Code (hooks SessionStart, PreToolUse, Stop, TodoWrite).
Sem enforcement automático, o Kiro tende a operar como assistente genérico e pular o pipeline.
Esta seção compensa a ausência de hooks com instruções explícitas e diretas no steering file de inclusão obrigatória.
