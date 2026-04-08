# Comportamentos Obrigatórios — Tabela Canônica

## Propósito

Este arquivo é a fonte de verdade canônica da lista numerada de todos os comportamentos obrigatórios definidos na seção "Comportamento Obrigatório" do `CLAUDE.md`. É referenciado pelo CLAUDE.md, pela rule `governance-behavior-tracking.md` e pela skill `governance-behavior-tracking/SKILL.md` para garantir que nenhum comportamento seja omitido no rastreamento.

---

## Tabela de Comportamentos

| # | Comportamento | Aplicável a | Referência |
|---|---|---|---|
| 1 | Interpretar antes de agir | Todos os escopos | CLAUDE.md §1 |
| 2 | Ler a governança relevante antes de implementar | Todos os escopos | CLAUDE.md §2 |
| 3 | Verificar ambiguidades antes de implementar | Todos os escopos | CLAUDE.md §3 |
| 4 | Classificar trechos técnicos enviados pelo usuário | Quando há trecho técnico | CLAUDE.md §4 |
| 5 | Atualizar a governança primeiro | Quando há definição durável nova | CLAUDE.md §5 |
| 6 | Seguir a prioridade entre fontes de verdade | Todos os escopos | CLAUDE.md §6 |
| 7 | Usar contexto acumulado do repositório | Todos os escopos | CLAUDE.md §7 |
| 8 | Não depender de repetição de instruções de processo | Todos os escopos | CLAUDE.md §8 |
| 9 | Avaliar eficiência em toda tarefa | Todos os escopos | CLAUDE.md §9 |
| 10 | Proteção de branch em análise de PR | Escopo Análise de PR | CLAUDE.md §10 |
| 11 | Rastrear comportamentos esperados durante toda a sessão | Todos os escopos | CLAUDE.md §11 |
| 12 | Consulta pré-planejamento obrigatória | Todos os escopos | CLAUDE.md §12, `.claude/rules/pre-planning-consultation.md` |
| 13 | Validar mudanças de governança via subagentes | Escopo Governança (quando afeta pipeline) | CLAUDE.md §13, `.claude/rules/governance-validation-pipeline.md` |

---

## Mapeamento Comportamentos → Passos do Pipeline

| Comportamento | Passo(s) do Pipeline |
|---|---|
| #9 (eficiência) | Passo 0 (verificação de ambiente) |
| #11 (rastreamento) | Início e fim da tarefa |
| #12 (pré-planejamento) | Antes de qualquer passo |
| #13 (validação via subagentes) | Passo 9.1 |

---

## Regras de Manutenção

- Quando um novo comportamento for adicionado ao CLAUDE.md, ele deve ser adicionado a esta tabela simultaneamente.
- Quando um comportamento for removido do CLAUDE.md, ele deve ser removido desta tabela simultaneamente.
- A numeração é estável — números removidos deixam gap (nunca reutilizados).
- Esta tabela é a referência para a skill `governance-behavior-tracking/SKILL.md` derivar a lista de comportamentos esperados.

**Nota sobre políticas de numeração distintas**: A política de numeração estável com gaps aplica-se a comportamentos obrigatórios. Para checks de auditoria (`governance-audit.md`), a política é renumeração obrigatória sem gaps. As políticas diferem porque comportamentos são referenciados por número em múltiplos arquivos (custo alto de renumeração), enquanto checks são referenciados apenas entre rule e script (custo baixo de renumeração).

---

## Referências Cruzadas

- `CLAUDE.md` — seção "Comportamento Obrigatório" (fonte primária)
- `.claude/rules/governance-behavior-tracking.md` — política de rastreamento
- `.claude/skills/governance-behavior-tracking/SKILL.md` — workflow de rastreamento

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-04-08 | Criado: tabela canônica de comportamentos obrigatórios extraída do CLAUDE.md | Auditoria de governança |
| 2026-04-08 | Adicionado: nota sobre políticas de numeração distintas (comportamentos com gaps vs. checks sem gaps) | Auditoria de governança — rodada 2 |
