# Hooks do Claude Code

## Propósito

Esta pasta contém os hooks de enforcement do Claude Code para este repositório. Hooks são scripts executados automaticamente antes ou depois de operações de ferramentas.

---

## Hooks Ativos

| Hook | Tipo | Matcher | Propósito |
|---|---|---|---|
| `instruction-change-detector.sh` | PostToolUse | Write\|Edit | Detecta mudanças em arquivos de governança e emite lembrete de revisão via REVIEW.md. A auditoria (`scripts/governance-audit.sh`) é executada no passo 0.1 do pipeline pré-commit, não por este hook. |
| `pre-commit-gate.sh` | Manual | — | Gate de validação: dotnet build + dotnet test antes de commit; paths resolvidos dinamicamente |
| `branch-guard.sh` | PostToolUse | Bash | Detecta operações de branch incorretas durante pr-analysis; emite alerta se o branch não for o head.ref esperado |
| `session-timer.sh` | PostToolUse | Bash | Exibe tempo efetivo acumulado da sessão após cada chamada Bash; informativo, nunca bloqueante |
| `post-commit-pr-reminder.sh` | PostToolUse | Bash | Detecta `git commit`/`git push` e emite lembrete para executar passo 10 (criar/atualizar PR); informativo, nunca bloqueante |

---

## Configuração

Os hooks são configurados em `.claude/settings.json` na seção `hooks`. Os hooks `instruction-change-detector.sh`, `branch-guard.sh`, `session-timer.sh` e `post-commit-pr-reminder.sh` são acionados automaticamente (PostToolUse). O `pre-commit-gate.sh` é referência para execução manual no pipeline pré-commit.

---

## Relação com Governança

- `instruction-change-detector.sh` → ativa `.claude/rules/instruction-review.md` → emite lembrete para executar `REVIEW.md`; a auditoria é executada no passo 0.1 do pipeline pré-commit
- `pre-commit-gate.sh` → implementa parte do pipeline de validação pré-commit definido em `CLAUDE.md`
- `branch-guard.sh` → protege o branch correto durante pr-analysis; usa `.claude/.pr-analysis-context` como contexto; arquivo criado pela skill pr-analysis
- `session-timer.sh` → implementa `.claude/rules/execution-time-tracking.md` → exibe tempo efetivo acumulado; usa `.claude/.session-timer` como estado
- `post-commit-pr-reminder.sh` → implementa enforcement do passo 10 de `.claude/rules/pr-metadata-governance.md` → lembra criação de PR após commit/push

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-03-18 | Criado: hooks reais substituindo placeholders | Reestruturação de governança |
| 2026-03-20 | Adicionado: branch-guard.sh para proteção de branch durante pr-analysis | Correção de workflow de PR |
| 2026-03-21 | Atualizado: documentação do instruction-change-detector.sh — emite lembrete mas não executa auditoria diretamente | Auditoria de governança |
| 2026-03-21 | Corrigido: branch-guard.sh criado (estava configurado mas inexistente); pre-commit-gate.sh refatorado com paths dinâmicos (paths hardcoded estavam obsoletos) | Análise de causas-raiz |
| 2026-03-30 | Adicionado: post-commit-pr-reminder.sh — enforcement informativo do passo 10 (criação de PR) após git commit/push | Verificação de conformidade de governança |
