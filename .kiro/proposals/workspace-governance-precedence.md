# Proposta: Precedência de Governança de Workspace

## Resumo Executivo

Proposta de modificação nas instruções de sistema do Kiro para estabelecer precedência explícita de processos definidos em workspaces governados (CLAUDE.md, skills, rules) sobre instruções genéricas de eficiência.

## Problema Identificado

### Situação Atual

O Kiro possui duas instruções que podem entrar em conflito:

1. **Instrução genérica (seção `<goal>`):**
   > "Execute the user goal using the provided tools, in as few steps as possible"

2. **Instrução específica do workspace (CLAUDE.md):**
   > "Pipeline de Validação Pré-Commit (Obrigatório)" com 12 passos detalhados

### Comportamento Observado

Quando uma tarefa parece "simples", o agente prioriza a instrução genérica de eficiência e pula etapas do processo definido no workspace, resultando em:

- Pipeline de validação não executado
- Snippets canônicos não aplicados
- Governança não atualizada
- Testes não executados
- CI/CD não acompanhado

### Causa Raiz

**Não há precedência explícita** entre instruções genéricas do sistema e processos específicos do workspace. O agente usa heurísticas (complexidade aparente da tarefa) para decidir qual instrução seguir, levando a comportamento inconsistente.

## Impacto do Problema

### Para Usuários com Governança

- ❌ Processo definido não é respeitado
- ❌ Qualidade comprometida (testes pulados, validações ignoradas)
- ❌ Governança desatualizada (código implementado sem registro)
- ❌ Comportamento imprevisível (às vezes segue, às vezes não)

### Para Usuários sem Governança

- ✅ Não afetados (comportamento atual mantido)

## Solução Proposta

### Modificações nas Instruções de Sistema

#### 1. Nova Seção: `<workspace_governance_precedence>`

Adicionar antes da seção `<goal>`:

```xml
<workspace_governance_precedence>
CRITICAL PRECEDENCE RULES:

1. When CLAUDE.md exists in the workspace root, it is the HIGHEST PRIORITY instruction set
2. Skills in .claude/skills/ define MANDATORY workflows - follow every step
3. Rules in .claude/rules/ define MANDATORY policies - respect all constraints
4. Workspace-specific instructions ALWAYS override generic system instructions
5. "Few steps as possible" means "execute the defined process efficiently", NOT "skip process steps"
6. Pipeline steps marked as "Obrigatório" (mandatory) cannot be skipped regardless of task complexity
7. Task complexity does NOT determine whether to follow the process - the process is always mandatory

WHEN THERE IS CONFLICT:
- CLAUDE.md > Skills > Rules > Generic system instructions
- Specific workspace process > Generic efficiency guidelines
- Mandatory pipeline steps > Perceived task simplicity

BEHAVIOR:
- Before starting ANY task, check if CLAUDE.md exists
- If CLAUDE.md exists, read it completely and treat it as the primary instruction set
- If CLAUDE.md defines a pipeline, execute ALL steps in order
- If a skill is activated, follow its workflow completely
- Do NOT optimize by skipping steps, regardless of task complexity
</workspace_governance_precedence>
```

#### 2. Modificar Seção `<goal>`

Adicionar no início:

```xml
<goal>
- CRITICAL: When CLAUDE.md exists in the workspace, it defines the MANDATORY process that must be followed completely. The phrase "in as few steps as possible" means "execute the defined process efficiently", NOT "skip steps from the process".
- When CLAUDE.md defines a pipeline (e.g., "Pipeline de Validação Pré-Commit"), ALL steps are mandatory unless explicitly marked as conditional.
- Skills defined in .claude/skills/ define MANDATORY workflows that must be followed step-by-step.
- "Efficiency" in a governed workspace means "follow the process correctly", not "take shortcuts".

[... resto do conteúdo atual de <goal> ...]
</goal>
```

#### 3. Adicionar em `<rules>`

No início da seção:

```xml
<rules>
- CRITICAL: Before starting ANY task, check if CLAUDE.md exists. If it exists, read it completely and follow its instructions as the highest priority.
- CRITICAL: If CLAUDE.md defines a pipeline with numbered steps, execute ALL steps in order unless explicitly marked as conditional.
- CRITICAL: If a skill is activated (from .claude/skills/), follow its workflow section step-by-step without skipping.

[... resto do conteúdo atual de <rules> ...]
</rules>
```

## Análise de Impacto

### Workspaces Afetados

| Tipo de Workspace | Impacto | Risco |
|-------------------|---------|-------|
| **Com CLAUDE.md** | Processo passa a ser respeitado integralmente | Baixo - comportamento esperado |
| **Sem CLAUDE.md** | Nenhum - comportamento atual mantido | Nenhum |
| **Com CLAUDE.md mal definido** | Processo mal definido será seguido à risca | Médio - expõe problemas de governança |

### Performance

- **Overhead:** ~1-2 segundos por tarefa (verificação e leitura de CLAUDE.md)
- **Benefício:** Elimina ciclos de correção por processo não seguido
- **Resultado líquido:** Positivo (menos retrabalho)

### Experiência do Usuário

**Positivo:**
- ✅ Comportamento previsível e consistente
- ✅ Governança respeitada automaticamente
- ✅ Qualidade garantida por processo
- ✅ Menos surpresas e retrabalho

**Negativo:**
- ⚠️ Tarefas "simples" demoram mais (seguem processo completo)
- ⚠️ Pode parecer "menos inteligente" para quem não entende governança
- ⚠️ Requer governança bem definida para funcionar bem

## Implementação

### Fase 1: Feature Flag (Recomendado)

Implementar com feature flag para rollout gradual:

```json
{
  "features": {
    "workspaceGovernancePrecedence": {
      "enabled": false,
      "rolloutPercentage": 0
    }
  }
}
```

**Rollout sugerido:**
1. 0% - Desenvolvimento e testes internos
2. 10% - Early adopters (workspaces com governança madura)
3. 50% - Maioria dos usuários
4. 100% - Todos os usuários

### Fase 2: Monitoramento

Métricas a acompanhar:
- Taxa de workspaces com CLAUDE.md
- Taxa de conclusão de pipelines completos
- Tempo médio de execução de tarefas
- Feedback de usuários (positivo/negativo)
- Taxa de erros/falhas em processos

### Fase 3: Ajustes

Baseado no feedback:
- Ajustar mensagens de progresso
- Melhorar detecção de processos mal definidos
- Adicionar sugestões de otimização de governança

## Alternativas Consideradas

### Alternativa 1: Apenas Documentação

**Descrição:** Documentar que usuários devem criar rule de enforcement em seus workspaces

**Prós:**
- Sem mudança no sistema
- Cada workspace controla seu comportamento

**Contras:**
- Inconsistente (depende de cada usuário)
- Não escala
- Não resolve o problema de precedência

**Decisão:** Rejeitada - não resolve o problema raiz

### Alternativa 2: Configuração por Workspace

**Descrição:** Adicionar `.kiro/settings/governance-enforcement.json` para controlar comportamento

**Prós:**
- Flexível
- Opt-in por workspace

**Contras:**
- Mais complexo
- Requer configuração adicional
- Não é o comportamento padrão esperado

**Decisão:** Pode ser adicionada como complemento, não como solução principal

### Alternativa 3: Solução Proposta (Precedência Sistêmica)

**Descrição:** Modificar instruções de sistema para estabelecer precedência clara

**Prós:**
- Resolve o problema raiz
- Comportamento consistente
- Funciona "out of the box"
- Alinhado com proposta de valor do Kiro

**Contras:**
- Requer mudança no sistema
- Afeta todos os usuários (mas só workspaces com CLAUDE.md)

**Decisão:** Recomendada

## Caso de Teste

### Workspace de Referência

Este workspace (`Starter.Template.AOT`) serve como caso de teste:

1. ✅ CLAUDE.md com pipeline de 12 passos
2. ✅ Skills com workflows detalhados
3. ✅ Rules com políticas específicas
4. ✅ Snippet canônico (SNP-001) para logging
5. ✅ Rule de enforcement criada (`.claude/rules/mandatory-process-enforcement.md`)

### Teste de Validação

**Tarefa:** "Crie um endpoint de get, para teste, se ele receber 1, retorna a string 'Um' se ele receber 2, retorna a string 'Dois'"

**Comportamento Atual (Sem Precedência):**
- ❌ Pulou para implementação direta
- ❌ Não seguiu snippet canônico SNP-001
- ❌ Não executou pipeline de validação
- ❌ Não atualizou governança

**Comportamento Esperado (Com Precedência):**
- ✅ Lê CLAUDE.md e identifica pipeline
- ✅ Ativa skill implement-request
- ✅ Segue workflow completo (11 passos)
- ✅ Aplica snippet canônico SNP-001
- ✅ Executa pipeline de validação (12 passos)
- ✅ Atualiza governança antes de implementar
- ✅ Acompanha CI até o fim

## Recomendação

**Implementar a solução proposta** com rollout gradual via feature flag.

**Justificativa:**
1. Resolve problema real identificado em workspace de produção
2. Alinhado com proposta de valor do Kiro (governança persistente)
3. Impacto controlado (só afeta workspaces com CLAUDE.md)
4. Melhora previsibilidade e qualidade
5. Caso de teste disponível para validação

## Próximos Passos

1. ✅ Criar rule de enforcement no workspace de teste (concluído)
2. ⏳ Validar comportamento com rule no workspace
3. ⏳ Coletar feedback do usuário
4. ⏳ Implementar mudança sistêmica com feature flag
5. ⏳ Rollout gradual
6. ⏳ Monitorar métricas e feedback
7. ⏳ Ajustar baseado em dados reais

## Contato

**Workspace de Referência:** `Starter.Template.AOT`
**Rule Criada:** `.claude/rules/mandatory-process-enforcement.md`
**Data:** 2026-04-06

---

**Nota:** Esta proposta foi gerada a partir de análise de causa-raiz de violação de processo em workspace real. O problema foi identificado, analisado e uma solução de curto prazo (rule no workspace) foi implementada. Esta proposta documenta a solução de longo prazo (mudança sistêmica) para beneficiar todos os usuários do Kiro.
