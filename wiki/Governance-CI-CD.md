# CI/CD e Deploy

## Descrição

Documenta os pipelines de CI/CD configurados no GitHub Actions. Deve ser consultado ao entender o fluxo de build/validação ou ao diagnosticar falhas no CI.

## Contexto

O projeto utiliza GitHub Actions como plataforma de CI/CD, com três pipelines: um para validação de execução (build, testes e health checks), um para publicação automática da Wiki e um para revisão automática de Pull Requests via Claude-Revisor. Todos os jobs de validação utilizam o GitHub Environment `ClaudeCode`, que fornece secrets como `DD_API_KEY` para integração com o Datadog.

---

## Pipeline "Validar Execução" (`ci.yml`)

### Gatilhos

- Push para `main` / `master`
- Pull Requests
- Execução manual via `workflow_dispatch`

### Ambiente

Todos os jobs declaram `environment: ClaudeCode`, que disponibiliza o secret `DD_API_KEY` para integração com o Datadog Agent durante a execução.

### Diagrama de Jobs

```
Compilação → Execução → unit-tests → ┬→ Validar Health Check (Debug)
                                      └→ Validar Health Check (Publish)
```

Os dois últimos jobs são executados em paralelo após a aprovação no gate de testes unitários.

### Job 1 — Compilação (`build`)

- Compila o projeto com Native AOT para `linux-x64`
- Executa `dotnet publish` gerando o binário nativo
- Faz upload do artefato `published-app` para uso nos jobs subsequentes

### Job 2 — Execução (`run`)

- Faz download do artefato `published-app`
- Inicia o Datadog Agent como container adjacente
- Executa o binário nativo da aplicação
- Realiza polling no endpoint `/health` até obter resposta

### Job 3 — Testes Unitários (`unit-tests`)

- Executa `dotnet test` com todos os testes unitários do projeto
- **Gate obrigatório**: falha em qualquer teste bloqueia os jobs de health check

### Job 4 — Validar Health Check Debug (`healthcheck-debug`)

- Executa a aplicação em modo debug via `dotnet run`
- Valida o endpoint `/health`

### Job 5 — Validar Health Check Publish (`healthcheck-publish`)

- Executa o binário AOT publicado
- Valida o endpoint `/health`
- Confirma que a aplicação funciona corretamente em modo Native AOT

---

## Pipeline "Publicar Wiki" (`wiki-publish.yml`)

### Gatilhos

- Push para `main` / `master` que altere arquivos em `wiki/**`
- Execução manual via `workflow_dispatch`

### Comportamento

- Clona o repositório wiki do GitHub usando `GITHUB_TOKEN`
- Copia todos os arquivos da pasta `wiki/` para o repositório wiki
- Faz push apenas se houver mudanças efetivas (evita commits vazios)

---

## Datadog e GitHub Environment

- O GitHub Environment `ClaudeCode` armazena o secret `DD_API_KEY`
- A variável `DD_ENV` varia por contexto de execução:
  - `build` — durante o job de compilação
  - `ci` — durante os jobs de execução e health check
  - `local` — durante execução local via Docker Compose
- O Datadog Agent é iniciado como container adjacente nos jobs que validam a aplicação em execução
- A configuração do Datadog Agent no CI difere do Docker Compose local:
  - No CI, o Agent usa `DD_ENV=ci` e coleta logs via file tailing (`app-logs/run.log`) em vez de Docker log collection
  - No Docker Compose, o Agent coleta logs de todos os containers via Docker socket (`DD_LOGS_CONFIG_CONTAINER_COLLECT_ALL=true`)

---

## Pipeline "Revisão Automática de Pull Request" (`claude-reviewer.yml`)

### Gatilho

- Pull Request com evento `review_requested`, filtrando apenas quando o revisor adicionado é `Claude-Revisor`

### Comportamento

- Executa `anthropics/claude-code-action@v1` em modo automação (com `prompt` definido)
- Carrega a skill `.claude/skills/auto-pr-review/SKILL.md` e inicia o ciclo iterativo Revisor↔Codificador
- A confirmação do usuário é implícita: o ato de adicionar `Claude-Revisor` como revisor equivale à aprovação
- O ciclo pode realizar até 10 iterações de revisão e correção antes de aprovar o PR

### Ambiente e Secrets Necessários

| Secret | Propósito |
|--------|-----------|
| `ANTHROPIC_API_KEY` | Autenticação com a API Anthropic para o `claude-code-action` |
| `GH_CLAUDE_CODE_MCP_CODIFICADOR` | Token MCP do papel Codificador (`ClaudeCode-Bot`) |
| `GH_CLAUDE_CODE_MCP_REVISOR` | Token MCP do papel Revisor (`Claude-Revisor`) |
| `GITHUB_TOKEN` | Token padrão do GitHub Actions (gerado automaticamente) |

Os três primeiros secrets devem estar configurados no GitHub Environment `ClaudeCode`.

### Configuração MCP

A action reutiliza o arquivo `.mcp.json` do repositório, que define os dois servidores MCP do GitHub (`github` e `github-revisor`). Os tokens são injetados via variáveis de ambiente no step da action.

---

## Referências

- [Operação](Governance-Operation) — pré-requisitos e configuração do ambiente
- [Health Check](Feature-Health) — endpoint validado pelos jobs de health check
- [Observabilidade](Governance-Observability) — integração com Datadog para métricas e logs
