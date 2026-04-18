# Regra: Governança de Revisão Automática de Pull Request

## Propósito

Esta rule define a política de revisão automática de código em Pull Requests, utilizando dois papéis distintos — Codificador e Revisor — com ciclo iterativo até aprovação. O workflow procedural está em `.claude/skills/auto-pr-review/SKILL.md`.

---

## Princípio Fundamental

> A revisão de código é uma responsabilidade de governança.
> O Revisor valida conformidade com as diretrizes do repositório.
> O Codificador implementa correções. Os papéis nunca se misturam.

---

## Papéis

### Codificador

| Campo | Valor |
|-------|-------|
| **Username GitHub** | `ClaudeCode-Bot` |
| **Nome (git user.name)** | `Codificador - Claude Agent` |
| **Token (secret)** | `GH_CLAUDE_CODE_MCP_CODIFICADOR` (token do usuário codificador) |
| **MCP Server** | `github` (prefixo `mcp__github__*`) |
| **Responsabilidades** | Implementar código, fazer commits em nome do usuário codificador, responder comentários de review |
| **Restrições** | NUNCA submeter review (APPROVE ou REQUEST_CHANGES); NUNCA usar tools `mcp__github-revisor__*` |

### Revisor

| Campo | Valor |
|-------|-------|
| **Username GitHub** | `Claude-Revisor` |
| **Token (secret)** | `GH_CLAUDE_CODE_MCP_REVISOR` |
| **MCP Server** | `github-revisor` (prefixo `mcp__github-revisor__*`) |
| **Email git** | `Claude-Revisor@users.noreply.github.com` |
| **Responsabilidades** | Revisar código contra governança, comentar em trechos específicos, aprovar ou solicitar mudanças, resolver threads |
| **Restrições** | NUNCA alterar código, fazer commits ou push; NUNCA usar tools `mcp__github__*` |

---

## Políticas

### Identidade Git

- **git commit**: configurar `user.name="Codificador - Claude Agent"` (apenas Codificador faz commits)
- **git push**: SEMPRE usa credenciais do token do usuário codificador (via `GH_CLAUDE_CODE_MCP_CODIFICADOR`), independente do papel
- **Interações MCP**: usar EXCLUSIVAMENTE o MCP server do papel ativo

### Trigger

Após a conclusão do acompanhamento de GitHub Actions (passo 11 do pipeline pré-commit), como passo 12, o assistente deve perguntar ao usuário se deseja revisão automática de código. A skill só prossegue com confirmação positiva.

### Isolamento de MCP Tools

Cada subagent (Revisor ou Codificador) deve usar EXCLUSIVAMENTE as ferramentas MCP do seu papel. A violação de isolamento (ex: Revisor usando `mcp__github__*`) é um erro de governança.

### Filtragem de Comentários

O Codificador deve IGNORAR comentários que não sejam do Codificador (`ClaudeCode-Bot`) nem do Revisor (`Claude-Revisor`). Para threads com comentários de terceiros, o Codificador só pode implementar quando existir reply do Revisor **dentro da mesma thread** com texto exato **"concordo"** (sem adições). O "concordo" em outra thread, em review separada ou como parte de frase ("Concordo com o apontamento...") NÃO é válido — deve ser reply na mesma thread com texto exato "concordo".

### Tratamento de Comentários de Terceiros pelo Revisor

Qualquer comentário no PR que não tenha sido escrito pelo Revisor deve ser tratado como comentário de humano:
- **Aderente à governança** → Revisor responde **na mesma thread** do comentário do terceiro via `mcp__github-revisor__add_reply_to_pull_request_comment` (usando o commentId do comentário original) com texto exato **"concordo"** — sem adições, sem qualificações, sem explicações adicionais no mesmo comentário. Contexto técnico adicional, se necessário, deve ser postado em comentário separado após o "concordo".
- **Não aderente** → Revisor responde **na mesma thread** via `mcp__github-revisor__add_reply_to_pull_request_comment` explicando por que não está em conformidade e sugere alternativa alinhada à governança.
- **NUNCA** criar thread/comentário separado para responder a terceiros — a resposta DEVE ser reply na thread original.

### Política de Comentários do Revisor

O Revisor só posta comentários inline quando há ação para o Codificador: corrigir, justificar ou investigar. Comentários que apenas confirmam conformidade sem apontar problema, sugestão ou dúvida são proibidos — geram ruído sem valor.

**Exemplos proibidos** (comentários de confirmação):
- "✅ está correto"
- "✅ segue o padrão"
- "Nomenclatura OK"
- "Logging conforme SNP-001"
- "Vertical Slice corretamente estruturada"

**Exemplos permitidos** (apontamentos com ação):
- "Nomenclatura diverge de naming-conventions §X — renomear para Y"
- "Falta log de saída conforme SNP-001"
- "Considerar extrair para método — complexidade alta"
- "Tipo não registrado no AppJsonContext — adicionar [JsonSerializable]"

**Body da review de APPROVE sem apontamentos**: quando o Revisor aprova sem encontrar problemas, o body deve ser um resumo breve (2-3 frases) confirmando conformidade geral. Checklists item-a-item são proibidos no body — o APPROVE já comunica a aprovação. Exemplo: "Implementação conforme governança. Vertical Slice, logging SNP-001, testes e artefatos de governança presentes."

### Aprovação e Request Changes

- **REQUEST_CHANGES**: submetido pelo Revisor quando encontra problemas de conformidade com governança
- **APPROVE**: submetido pelo Revisor quando não há mais problemas e todos os apontamentos foram resolvidos
- O Codificador NUNCA submete reviews (nem APPROVE nem REQUEST_CHANGES)

### Resolução de Threads

- O Revisor resolve threads respondendo **na mesma thread** via `mcp__github-revisor__add_reply_to_pull_request_comment` com "Thread resolvida — correção verificada" quando a correção do Codificador é verificada e adequada
- O Revisor resolve threads respondendo **na mesma thread** quando a justificativa de inviabilidade do Codificador é válida
- Threads resolvidas por humanos são respeitadas — não são re-abertas nem reimplementadas
- **NUNCA** indicar resolução em review separada ou comentário avulso — a indicação DEVE ser reply na thread original do apontamento

### Inviabilidade de Implementação

Quando o Codificador não consegue implementar uma correção (tecnicamente inviável, conflito com outra regra):
- Responde na thread explicando a inviabilidade
- O Revisor reavalia na próxima iteração:
  - Se justificativa válida → resolve thread
  - Se justificativa insuficiente → insiste com alternativa

### Insistência Humana contra Governança

Se um humano insistir em comentário que o Revisor considerou não aderente à governança:
- **Governança prevalece** — o Revisor não cede
- O Revisor re-explica a diretriz e indica que a governança deve ser alterada primeiro
- O Codificador não implementa (thread sem "concordo" do Revisor)

### Reviews Formais de Humanos

Se um humano submeter review formal (REQUEST_CHANGES ou APPROVE) durante o ciclo:
- **Respeitar e incorporar** — apontamentos de REQUEST_CHANGES são tratados como adicionais
- A condição de encerramento do ciclo depende APENAS da aprovação do Revisor — reviews humanas são independentes

### Threads Outdated

Quando o GitHub marca threads como "outdated" (código referenciado mudou):
- **Re-avaliar sempre** — verificar se o problema persiste no código atual
- Não ignorar automaticamente por ser outdated

### Limites de Segurança

| Limite | Valor | Ação ao atingir |
|--------|-------|-----------------|
| Max iterações Revisor↔Codificador | 10 | Pausar ciclo, reportar estado detalhado de cada thread |
| Conflito de merge | Detectado no push | Pausar ciclo, reportar ao usuário |
| PR fechado/merged externamente | Detectado na pré-condição | Encerrar ciclo, reportar ao usuário |

### Condição de Encerramento

O ciclo encerra com sucesso quando o Revisor submete APPROVE. Não é necessário que reviewers humanos também aprovem — reviews humanas são independentes do ciclo automatizado.

### Relatório Final

O relatório ao encerrar deve incluir:
- Threads resolvidas (com resumo de cada correção)
- Threads ignoradas de humanos (sem "concordo" do Revisor) — listar com justificativa
- Threads resolvidas por humano manualmente (respeitadas, não reimplementadas)
- Número de iterações executadas

### Escopo da Revisão

- **Tipo**: apenas code review contra governança (sem execução de pipeline build/test/docker)
- **Abrangência**: arquivos inteiros alterados no PR (não apenas linhas do diff)

---

## Relação com Outras Rules

- `pr-metadata-governance.md` — governa criação e atualização de PRs; esta rule governa a revisão automatizada
- `governance-policies.md` — políticas de contexto (§2), propagação (§3) e ambiguidade (§4) aplicáveis à revisão
- `source-of-truth-priority.md` — hierarquia usada pelo Revisor para resolver conflitos de governança
- `architecture-governance.md` — decisões arquiteturais verificadas pelo Revisor
- `naming-governance.md` — convenções de nomenclatura verificadas pelo Revisor
- `endpoint-validation.md` — validação de endpoints é responsabilidade do pipeline, não da revisão automática

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-03-31 | Criado: política de revisão automática de PR com papéis Codificador e Revisor | Instrução do usuário |
| 2026-04-07 | Atualizado: explicitado nome do usuário codificador (Codificador - Claude Agent); corrigido prefixo MCP de `mcp__github__*` para `mcp__github-codificador__*` | Instrução do usuário |
| 2026-04-08 | Corrigido: restrição do Revisor atualizada de `mcp__github__*` para `mcp__github-codificador__*` | Auditoria de governança — rodada 2 |
| 2026-04-09 | Reforçado: "concordo" deve ser reply na mesma thread do terceiro (texto exato, sem adições); resolução de threads deve ser reply na thread original (não review separada); filtragem do Codificador exige "concordo" na mesma thread | Análise de falhas no PR #55 |
| 2026-04-09 | Adicionado: Política de Comentários do Revisor — comentários de confirmação proibidos; body de APPROVE sem apontamentos deve ser resumo breve (2-3 frases), sem checklist | Análise de causa raiz — ruído no PR #56 |
| 2026-04-18 | Corrigido: nome do MCP Server do Codificador revertido de `github-codificador` para `github` e prefixo de tools `mcp__github-codificador__*` para `mcp__github__*`. Razão: divergência entre rule (esperava `github-codificador`) e realidade operacional (harness do Claude Code expõe o server como `github`, conforme `session-start.sh:70` e ferramentas carregadas via ToolSearch). As alterações de 2026-04-07 e 2026-04-08 haviam introduzido o nome inconsistente; este registro reverte ambas. | Análise de causa raiz — bloqueio de `auto-pr-review` no PR #64 (Erro 23 do `bash-errors-log.md`) |
