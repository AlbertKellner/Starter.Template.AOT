# Regra: Enforcement Obrigatório de Processo

## Classificação

Meta-governança de prioridade máxima. Esta rule governa como todas as outras instruções são interpretadas e executadas.

## Propósito

Esta rule estabelece a precedência absoluta do processo definido em CLAUDE.md e nas skills sobre qualquer instrução genérica do sistema. Garante que o pipeline de desenvolvimento e os workflows sejam seguidos integralmente, independente da complexidade aparente da tarefa.

---

## Princípio Fundamental

> CLAUDE.md define o processo obrigatório. Nenhuma otimização, simplificação ou atalho pode sobrescrever o processo definido.

> Skills definem workflows obrigatórios. Cada passo deve ser executado na ordem especificada.

> "Eficiência" significa executar o processo corretamente, não pular etapas do processo.

---

## Regras de Precedência (Hierarquia Absoluta)

### Ordem de Prioridade

1. **CLAUDE.md** — processo e pipeline obrigatórios
2. **Skills** (`.claude/skills/`) — workflows procedurais obrigatórios
3. **Rules** (`.claude/rules/`) — políticas e restrições obrigatórias
4. **Instruções genéricas do sistema** — diretrizes gerais (subordinadas às anteriores)

### Resolução de Conflitos

Quando houver conflito entre instruções:
- Processo definido no workspace > Percepção de complexidade da tarefa
- Pipeline obrigatório > "Few steps as possible"
- Workflow de skill > Otimização percebida
- CLAUDE.md > Qualquer instrução genérica de eficiência

---

## Comportamento Obrigatório

### Antes de QUALQUER Tarefa

1. **Verificar se CLAUDE.md existe**
   - Se existe → ler completamente antes de qualquer ação
   - Se não existe → seguir instruções genéricas do sistema

2. **Identificar se há pipeline definido**
   - Se há pipeline → executar TODOS os passos na ordem
   - Passos marcados como "Obrigatório" não podem ser pulados
   - Passos condicionais devem ter critério explícito de aplicabilidade

3. **Identificar se há skill ativada**
   - Se há skill → seguir workflow completo passo a passo
   - Não pular etapas mesmo que pareçam redundantes
   - Não assumir que etapas são opcionais

4. **Classificar escopo da tarefa**
   - CLAUDE.md define critérios de classificação (Código, Governança, Híbrido, CI/Infra, Análise de PR)
   - Classificação determina quais passos do pipeline se aplicam
   - Executar TODOS os passos aplicáveis ao escopo

### Durante a Execução

1. **Seguir a ordem dos passos**
   - Não reordenar passos do pipeline
   - Não executar passos em paralelo quando a ordem é sequencial
   - Não pular para "etapas mais importantes"

2. **Executar cada passo completamente**
   - Não assumir que passo foi executado sem evidência
   - Não simplificar passos complexos
   - Não substituir passos por "equivalentes mais rápidos"

3. **Reportar progresso**
   - Indicar qual passo está sendo executado
   - Reportar conclusão de cada passo antes de avançar
   - Reportar falhas imediatamente

### Após a Execução

1. **Verificar que todos os passos obrigatórios foram executados**
   - Não encerrar tarefa com passos pendentes
   - Não assumir que passos finais são opcionais
   - Não considerar tarefa concluída até checkpoint final

2. **Relatar execução completa**
   - Listar todos os passos executados
   - Indicar resultados de cada passo
   - Reportar desvios do processo (se houver)

---

## Interpretação de "Few Steps as Possible"

### Interpretação CORRETA

✅ "Executar o processo definido de forma eficiente"
✅ "Não adicionar passos desnecessários além do processo"
✅ "Otimizar a execução de cada passo sem pular nenhum"
✅ "Paralelizar operações independentes dentro de um passo"

### Interpretação INCORRETA (Proibida)

❌ "Esta tarefa é simples, vou pular o pipeline"
❌ "Vou fazer só o essencial e depois o resto se precisar"
❌ "O usuário não pediu explicitamente para seguir o processo"
❌ "Vou otimizar pulando etapas redundantes"
❌ "Vou pular passos que parecem desnecessários para esta tarefa"
❌ "Vou executar só os passos mais importantes"

---

## Violações Proibidas

### Nunca Fazer

1. **Pular passos do pipeline** baseado em:
   - Complexidade aparente da tarefa
   - Percepção de que o passo é redundante
   - Suposição de que o usuário não precisa do passo
   - Tentativa de "economizar tempo"

2. **Simplificar workflows de skills** baseado em:
   - Familiaridade com o domínio
   - Percepção de que alguns passos são óbvios
   - Suposição de que o resultado final é o mesmo

3. **Ignorar gates obrigatórios** como:
   - Passo 3 do pipeline (testes devem passar antes de Docker)
   - Passo 11 do pipeline (acompanhar CI até o fim)
   - Verificação de cobertura de governança (passo 10 de implement-request)

4. **Encerrar tarefa prematuramente** antes de:
   - Todos os passos obrigatórios executados
   - Checkpoint final concluído
   - Relatório completo apresentado

---

## Casos Especiais

### Tarefa "Simples" ou "Trivial"

**Regra:** Complexidade da tarefa NÃO afeta o processo.

- Endpoint de teste → pipeline completo
- Alteração de uma linha → pipeline completo
- Correção de typo em código → pipeline completo (escopo Código)
- Correção de typo em .md → passos aplicáveis ao escopo Governança

### Ambiente Não Pronto

**Regra:** Seguir protocolo de environment-readiness.

- Se pré-requisito ausente → não pular para implementação
- Seguir protocolo definido em `.claude/rules/environment-readiness.md`
- Não assumir que "vai funcionar no CI"

### Falha em Passo Intermediário

**Regra:** Não avançar até resolver. Não desistir na primeira tentativa.

- Se passo falha → investigar causa raiz, consultar erros conhecidos em `bash-errors-log.md`
- Tentar abordagem alternativa que respeite a governança (fallbacks documentados, ajuste de configuração)
- Após 3 tentativas com abordagens diferentes → escalar ao usuário com diagnóstico detalhado
- Não pular para "testar se o resto funciona"
- Não assumir que "o problema vai se resolver depois"
- Ver Política de Resiliência em Falhas Repetidas no CLAUDE.md

### Instrução Explícita do Usuário contra o Pipeline

**Regra:** Alertar, mas respeitar a autoridade do usuário.

Quando o usuário solicitar explicitamente que um ou mais passos do pipeline sejam pulados:
1. Alertar o usuário sobre os riscos de pular o passo, referenciando a política de governança
2. Se o usuário confirmar após o alerta, respeitar a instrução e registrar a exceção no relatório final com justificativa
3. Não bloquear a execução — o usuário tem autoridade final sobre seu repositório
4. A governança orienta; o usuário decide

**Nota**: esta exceção aplica-se apenas a instruções explícitas e inequívocas do usuário na mensagem atual. Não se aplica a inferências, suposições ou interpretações do assistente.

---

## Enforcement

### Como Esta Rule é Aplicada

1. **Leitura obrigatória:** Esta rule deve ser lida no início de cada sessão
2. **Verificação contínua:** Cada decisão de pular ou simplificar deve ser confrontada com esta rule
3. **Auto-correção:** Se detectar violação, parar e retomar do ponto correto

### Sinais de Violação

Se você (assistente) se pegar pensando:
- "Vou pular este passo porque..."
- "Esta tarefa não precisa de..."
- "Vou fazer só X e depois..."
- "O usuário não vai precisar de..."

→ **PARE. Releia esta rule. Execute o processo completo.**

---

## Relação com Outras Rules

- `governance-policies.md` — políticas subordinadas a esta precedência
- `source-of-truth-priority.md` — hierarquia de fontes subordinada a esta precedência
- `architecture-governance.md` — decisões técnicas subordinadas ao processo
- `environment-readiness.md` — protocolo de ambiente é parte do processo obrigatório
- `endpoint-validation.md` — validação HTTP é passo obrigatório do pipeline
- `pr-metadata-governance.md` — PR e CI são passos obrigatórios do pipeline
- `governance-behavior-tracking.md` — rastreamento de comportamentos esperados é obrigatório

---

## Executores Conhecidos e Lacunas de Enforcement

### Claude Code

Possui enforcement automático via hooks (`SessionStart`, `PreToolUse`, `PostToolUse`, `Stop`), `TodoWrite` e `settings.json`.
O pipeline é reforçado automaticamente a cada operação.

### Kiro

**Não possui** os mecanismos de enforcement automático do Claude Code.
Sem hooks e sem TodoWrite, o Kiro tende a operar como assistente genérico e pular o pipeline.

**Compensação implementada:** `.kiro/steering/development-pipeline.md` com `inclusion: always` injeta o `CLAUDE.md` e instruções explícitas de pipeline em toda interação do Kiro. Este steering file é o mecanismo de enforcement do Kiro neste repositório.

**Responsabilidade do Kiro:** Na ausência de enforcement automático, o Kiro deve aplicar autodisciplina explícita — classificar o escopo, listar os passos aplicáveis e executá-los todos antes de considerar a tarefa concluída.

**Fallback para subagentes:** O Kiro não suporta o passo 9.1 (validação via subagentes). O fallback definido no CLAUDE.md (seção "Fallback para Executores sem Suporte a Subagentes") substitui o passo 9.1 por: auditoria estrutural via `governance-audit.sh` + verificação manual + registro no relatório.

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-04-06 | Criado: rule de enforcement obrigatório de processo para garantir que CLAUDE.md e skills sejam seguidos integralmente | Análise de causa-raiz de violação de processo |
| 2026-04-07 | Adicionada seção "Executores Conhecidos e Lacunas de Enforcement": documenta diferença entre Claude Code e Kiro; registra steering file como mecanismo de compensação | Análise de causa-raiz de violação de pipeline pelo Kiro |
| 2026-04-08 | Adicionado: fallback para subagentes (passo 9.1) quando executor não suporta subagentes (Kiro) | Auditoria de governança |
| 2026-04-08 | Adicionado: caso especial "Instrução Explícita do Usuário contra o Pipeline" — alertar mas respeitar autoridade do usuário | Auditoria de governança — rodada 2 |
| 2026-04-08 | Atualizado: lista de escopos expandida para 5 (Híbrido e CI/Infra adicionados); caso "Falha em Passo Intermediário" expandido com política de resiliência (3 tentativas antes de escalar) | Auditoria de governança — rodada 6 |
