---
name: planning
description: Planejamento estruturado de implementação com exploração de codebase e design de solução
---

# Skill: planning

## Nome

Planejamento Estruturado de Implementação

## Descrição

Esta skill estrutura a análise e o planejamento antes de qualquer implementação, replicando o comportamento do "plan mode" do Claude Code como workflow formal da governança. O assistente explora o codebase, projeta a solução, verifica conformidade com a governança e produz um plano estruturado para aprovação do usuário.

A skill é **portável** — executável tanto no Claude Code quanto no Kiro, usando qualquer LLM do Claude. Recursos específicos do Claude Code (Agent, TodoWrite, AskUserQuestion, WebFetch, WebSearch) são usados quando disponíveis, com fallback para execução direta quando não estão. O workflow nunca bloqueia por ausência de uma ferramenta.

---

## Quando Usar

- O usuário invoca explicitamente via `/planning`
- O usuário solicita planejamento antes de uma implementação
- A tarefa é complexa o suficiente para beneficiar de exploração e design antes da codificação

---

## Entradas Esperadas

- Solicitação do usuário em linguagem natural descrevendo o que deve ser implementado
- Pode incluir requisitos, restrições, referências a features existentes ou links de documentação
- Pode ser implícita (a necessidade de planejamento é inferida da complexidade da tarefa)

---

## Workflow Interno

```
FASE 0 — PERGUNTA INICIAL: PLANEJAR OU EXECUTAR?

  1. Antes de qualquer análise, perguntar ao usuário:

     "Você quer Planejar ou Executar diretamente?
      1. Planejar
      2. Executar diretamente"

  2. Se o usuário selecionar "Planejar" → prosseguir para FASE 1
  3. Se o usuário selecionar "Executar diretamente" → encerrar esta skill
     e executar a tarefa sem planejamento formal (ativar implement-request
     ou skill pertinente conforme classificação da mensagem)
  4. Esta é a PRIMEIRA ação obrigatória da skill, sem exceções


FASE 1 — GATE PRÉ-PLANEJAMENTO

  1. Ler open-questions.md — há dúvidas abertas relevantes à tarefa?
  2. Ler PENDENCIAS.md — há bloqueadores ativos que afetam a tarefa?
  3. Consultar arquivos de governança pertinentes:
     - Instructions/business/business-rules.md (regras que a tarefa afeta)
     - Instructions/business/domain-model.md (entidades envolvidas)
     - Instructions/business/workflows.md (fluxos pressupostos)
     - Instructions/glossary/ubiquitous-language.md (terminologia)
  4. Identificar definições pendentes ou cenários não mapeados
  5. Se a tarefa envolver integração externa:
     - Consultar documentação online (WebFetch/WebSearch quando disponíveis)
     - Resolver autonomamente as dúvidas que a documentação responder
  6. Se restarem dúvidas verdadeiramente abertas:
     - Apresentar ao usuário apenas os pontos não resolvíveis por consulta
     - Iterar até que não restem pendências
  7. Marcar gate satisfeito:
     - Claude Code: criar state file via touch do path exibido pelo hook
     - Kiro: registrar mentalmente que o gate foi satisfeito

  → Referência: .claude/rules/pre-planning-consultation.md


FASE 2 — ENTENDIMENTO INICIAL

  Objetivo: explorar o codebase para encontrar funções, padrões e utilitários
  existentes que possam ser reutilizados na implementação.

  Caminho direto (sempre disponível):
  1. Usar Glob para mapear a estrutura de pastas relevante
     - Features/Query/ e Features/Command/ — slices existentes
     - Infra/ — componentes de infraestrutura
     - Shared/ — utilitários e integrações compartilhadas
  2. Usar Grep para localizar implementações relacionadas
     - Padrões de código semelhantes ao que será implementado
     - Interfaces, classes e métodos reutilizáveis
  3. Usar Read para entender código-chave
     - Slices existentes como referência de estrutura
     - Program.cs — registro de DI e pipeline
     - AppJsonContext.cs — tipos registrados para AOT

  Caminho com Agent (quando executor suportar):
  1. Lançar até 3 agentes general-purpose em paralelo
  2. Critério de quantidade:
     - 1 agente: tarefa isolada em área conhecida do codebase
     - 2 agentes: escopo que abrange múltiplas áreas
     - 3 agentes: escopo incerto que requer exploração ampla
  3. Cada agente recebe prompt autossuficiente com foco específico:
     - Agente A: buscar implementações existentes e padrões de código
     - Agente B: explorar componentes relacionados e dependências
     - Agente C: investigar padrões de teste e infraestrutura
  4. Os prompts devem ser autocontidos (o agente não tem contexto da conversa)

  Resultado esperado: lista de arquivos críticos, funções reutilizáveis,
  padrões de referência e restrições identificadas.


FASE 3 — DESIGN DA SOLUÇÃO

  Objetivo: projetar a abordagem de implementação com base nos achados da Fase 2.

  Caminho direto (sempre disponível):
  1. Ler a governança arquitetural:
     - Instructions/architecture/patterns.md (PAD-001 a PAD-008)
     - Instructions/architecture/naming-conventions.md
     - Instructions/architecture/folder-structure.md
     - Instructions/architecture/engineering-principles.md
     - Instructions/architecture/technical-overview.md
  2. Projetar a abordagem considerando:
     - Reutilização de código encontrado na Fase 2
     - Conformidade com Vertical Slice Architecture (PAD-001)
     - Segregação Command/Query (PAD-002)
     - Padrão de logging storytelling (SNP-001)
     - Checklist de criação de feature (PAD-001)
  3. Identificar alternativas e selecionar a recomendada
  4. Documentar trade-offs da abordagem escolhida

  Caminho com Agent (quando executor suportar):
  1. Lançar 1 agente general-purpose com contexto completo:
     - Achados da Fase 2 (paths, line numbers, padrões)
     - Restrições de governança identificadas
     - Requisitos do usuário
  2. Solicitar plano detalhado de implementação

  Resultado esperado: abordagem de implementação definida com justificativa.


FASE 4 — REVISÃO E ALINHAMENTO

  Objetivo: verificar que o design está correto e alinhado antes de formalizar.

  1. Ler diretamente os arquivos críticos identificados nas fases anteriores
     - NUNCA delegar entendimento a agentes nesta fase
     - O assistente deve compreender o código ele mesmo
  2. Verificar conformidade com a governança:
     - Nomenclatura segue naming-conventions.md?
     - Estrutura segue folder-structure.md?
     - Padrões seguem patterns.md?
     - Princípios seguem engineering-principles.md?
  3. Verificar alinhamento com a solicitação original do usuário:
     - O plano atende ao que foi pedido?
     - Há escopo extra não solicitado?
     - Há requisitos não cobertos?
  4. Se houver dúvidas remanescentes:
     - Claude Code: usar AskUserQuestion
     - Kiro: apresentar como texto e aguardar resposta
  5. Iterar até que não restem dúvidas


FASE 5 — PLANO FINAL

  Objetivo: produzir o plano estruturado e apresentar ao usuário para aprovação.

  1. Escrever arquivo markdown com o plano estruturado:

     Template obrigatório:
     ┌──────────────────────────────────────────────────┐
     │ # Plano: [Título]                                │
     │                                                  │
     │ ## Contexto                                      │
     │ [Por que esta mudança é necessária]              │
     │                                                  │
     │ ## Abordagem                                     │
     │ [Estratégia recomendada — apenas uma]            │
     │                                                  │
     │ ## Arquivos a Criar/Modificar                    │
     │ [Lista com caminho completo e descrição]         │
     │                                                  │
     │ ## Código Existente a Reutilizar                 │
     │ [Funções e padrões com file_path:line_number]    │
     │                                                  │
     │ ## Verificação                                   │
     │ [Como testar end-to-end — comandos, testes]      │
     └──────────────────────────────────────────────────┘

  2. Localização do arquivo de plano:
     - Se o executor definir path de plano → usar esse path
     - Caso contrário → criar em .claude/plans/<nome-descritivo>.md

  3. Apresentar o plano ao usuário e solicitar aprovação:
     - Exibir resumo do plano na resposta
     - Perguntar se o usuário aprova, quer ajustes ou quer descartar

  4. Se aprovado → o plano pode ser entregue à skill implement-request
     como input para execução
  5. Se ajustes solicitados → retornar à fase pertinente e iterar
  6. Se descartado → encerrar a skill sem implementação
```

---

## Saídas Esperadas

- Arquivo markdown com plano estruturado (template da Fase 5)
- Gate pré-planejamento satisfeito (comportamento #12)
- Dúvidas resolvidas ou registradas em open-questions.md
- Premissas adotadas registradas em assumptions-log.md (quando aplicável)

---

## Portabilidade

| Recurso | Claude Code | Kiro | Fallback |
|---------|-------------|------|----------|
| Agent (exploração paralela) | Disponível — usar para Fases 2 e 3 | Indisponível | Usar Glob/Grep/Read diretamente |
| TodoWrite (rastreamento) | Disponível — rastrear fases | Indisponível | Rastrear progresso via texto na resposta |
| AskUserQuestion (dúvidas) | Disponível — usar na Fase 4 | Indisponível | Apresentar dúvidas como texto e aguardar |
| WebFetch/WebSearch (docs) | Disponível — usar na Fase 1 | Indisponível | Basear-se no conhecimento da LLM |
| Write (plano) | Disponível | Disponível | Sempre disponível |
| Read/Grep/Glob | Disponível | Disponível | Sempre disponível |

**Regra**: o workflow nunca bloqueia por ausência de ferramenta. Se um recurso não está disponível, usar o fallback e prosseguir.

---

## Arquivos de Governança Relacionados

- `.claude/rules/pre-planning-consultation.md` — política que a Fase 1 operacionaliza (comportamento #12)
- `.claude/rules/governance-policies.md` — políticas de ambiguidade (§4), propagação (§3) e contexto (§2)
- `.claude/rules/source-of-truth-priority.md` — hierarquia usada para resolver conflitos na Fase 3
- `.claude/rules/architecture-governance.md` — decisões técnicas verificadas na Fase 4
- `Instructions/architecture/patterns.md` — padrões verificados na Fase 3 (PAD-001 a PAD-008)
- `Instructions/architecture/naming-conventions.md` — nomenclatura verificada na Fase 4
- `Instructions/architecture/folder-structure.md` — estrutura verificada na Fase 4
- `Instructions/operating-model.md` — classificação de mensagens que ativa esta skill
- `open-questions.md` — consultado na Fase 1 e atualizado quando necessário
- `assumptions-log.md` — atualizado quando premissas são adotadas

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-04-10 | Criado: skill de planejamento estruturado com 6 fases, portabilidade Claude Code/Kiro e integração com pre-planning-consultation | Instrução do usuário |
