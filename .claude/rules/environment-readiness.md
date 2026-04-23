---
paths:
  - "**/Dockerfile"
  - "**/docker-compose*.yml"
  - "**/*.csproj"
  - "**/appsettings*.json"
---

# Regra: Prontidão de Ambiente e Eficiência de Execução

## Propósito

Esta rule define a política de verificação de prontidão de ambiente e eficiência de execução. O workflow procedural está em `.claude/skills/verify-environment/SKILL.md`.

---

## Princípio Fundamental

> Falhas de ambiente previsíveis são responsabilidade do agente, não do usuário.
> Verificar antes de executar custa segundos. Recuperar de falhas em cascata custa minutos.
> O ambiente deve estar pronto antes que qualquer operação substantiva comece.
> Eficiência não é um objetivo secundário — é parte obrigatória de toda tarefa.

---

## Políticas

### Script de Bootstrap

`scripts/setup-env.sh` é um **modelo declarativo de configuração de ambiente**. O agente nunca deve executá-lo diretamente. O ambiente deve chegar já pronto. Se um pré-requisito estiver ausente, o agente atualiza o script e sinaliza ao usuário para sincronizar a ferramenta externa.

### Arquivos de Registro

| Arquivo | Propósito |
|---|---|
| `scripts/required-vars.md` | Variáveis de ambiente e secrets que a ferramenta externa deve prover |
| `scripts/container-setup.md` | Dependências de sistema, PATH, SDKs, permissões |
| `scripts/operational-runbook.md` | Portas, URLs, comandos, credenciais de teste, troubleshooting |

### Eficiência de Execução

Em toda tarefa, antes de iniciar qualquer sequência de operações, avaliar ativamente:
- Existe artefato já gerado que pode ser reutilizado?
- Existe etapa que pode ser antecipada para evitar falha custosa posterior?
- Existe etapa redundante que pode ser eliminada?
- Existe abordagem mais rápida e reversível que produz o mesmo resultado?

### Timeouts de MCP Servers

O Claude Code inicializa MCP servers de forma assíncrona no startup da sessão. Se o handshake exceder o timeout padrão, as ferramentas MCP não ficam disponíveis. As variáveis `MCP_TIMEOUT` (handshake) e `MCP_TOOL_TIMEOUT` (chamadas individuais) devem ser configuradas em `.claude/settings.json` seção `env` para garantir inicialização confiável dos MCP servers do GitHub (payload pesado com 20+ tools).

Valores recomendados: `MCP_TIMEOUT=60000` (60s), `MCP_TOOL_TIMEOUT=300000` (5min).

### Uso Correto da Ferramenta Monitor

A ferramenta `Monitor` do Claude Code emite eventos a partir de linhas de stdout do script monitorado. A política de cobertura exige que cada linha represente um **estado real do recurso observado** (log de aplicação, status de CI, evento de arquivo). Scripts que emitem apenas **timers** (`sleep 60; echo tick`) violam a cobertura — um timer não descreve o estado do alvo e, na prática, transforma o `Monitor` em um cronômetro cego.

**Proibido**:
- Usar `Monitor` para "aguardar" um recurso externo (CI, build remoto, fila) sem uma consulta real dentro do script monitorado (chamada a endpoint, `curl`, ferramenta CLI que retorne estado).
- Confundir `Monitor` com `Bash` + `run_in_background`: para um único "espere até X segundos e continue", usar `Bash` com `run_in_background: true` + `sleep N`. Monitor é para streaming contínuo de eventos reais.
- Encerrar o turno apoiado apenas em ticks de timer, sem ter chamado a ferramenta que observa o estado real do alvo.

**Recomendado**:
- Para esperas pontuais (um único intervalo): `Bash` com `run_in_background: true` + `sleep N` + chamada MCP após a notificação de conclusão.
- Para polling de CI especificamente: ver "Padrão Canônico de Polling" em `.claude/skills/manage-pr-lifecycle/SKILL.md` — é um loop de `get_check_runs` + `run_in_background`, não um `Monitor`.

### Conversão de Problemas Recorrentes

Todo problema recorrente de ambiente deve ser convertido em pré-requisito verificável:
- Prevenível por checklist → adicionar ao checklist de pré-requisitos na skill
- Prevenível por configuração → atualizar `scripts/setup-env.sh`
- Prevenível por documentação → adicionar a `scripts/operational-runbook.md`

---

## Workflow

O workflow completo (checklist de pré-requisitos, protocolo de ambiente não pronto, avaliação de eficiência, evolução do script e conversão de problemas recorrentes) está definido em `.claude/skills/verify-environment/SKILL.md`.

---

## Relação com Outras Rules

- `bash-error-logging.md` — registra erros APÓS ocorrerem; esta rule previne que ocorram
- `governance-policies.md` — políticas de ambiguidade (§4) e contexto do repositório (§2) aplicáveis a ambiente
- `governance-audit.md` — a auditoria automatizada complementa a verificação de prontidão

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-03-18 | Criado: regra de prontidão de ambiente | Instrução do usuário |
| 2026-03-19 | Expandido: eficiência de execução, evolução do script, conversão de problemas recorrentes | Instrução do usuário |
| 2026-03-21 | Refatorado: workflows procedurais extraídos para skill verify-environment; rule simplificada para conter apenas política | Auditoria de governança |
| 2026-04-02 | Adicionado: política de timeouts MCP (MCP_TIMEOUT, MCP_TOOL_TIMEOUT) para garantir inicialização confiável | Diagnóstico de MCP — Erro 12 |
| 2026-04-23 | Adicionado: seção "Uso Correto da Ferramenta Monitor" — proibição de timers cegos; `Bash` + `run_in_background` é a ferramenta correta para esperas pontuais | Análise de causa raiz — polling passivo com Monitor |
