# Skill: remove-feature

## Nome
Remoção de Feature

## Descrição
Esta skill orienta o assistente quando o usuário solicita a remoção de uma feature (Slice) do repositório, garantindo que todos os artefatos de código, governança, wiki e referências cruzadas sejam limpos de forma consistente.

## Quando Usar
Ativar esta skill quando:
- O usuário solicitar remoção de uma feature existente
- O usuário solicitar remoção de um endpoint ou funcionalidade
- A remoção for consequência de uma refatoração ou decisão arquitetural

## Entradas Esperadas
- Nome da feature a ser removida
- Justificativa (pode ser implícita na mensagem)

## Workflow Interno

```
1. IDENTIFICAR ARTEFATOS DA FEATURE
   - Localizar a pasta da Slice em Features/Query/ ou Features/Command/
   - Listar todos os arquivos: Endpoint, UseCase, Repository, Models, Interfaces, Scripts SQL
   - Identificar dependências em Shared/ (se houver decorator de cache, client externo, etc.)
   - Identificar registros de DI em Program.cs

2. IDENTIFICAR ARTEFATOS DE GOVERNANÇA
   - Regras de negócio em Instructions/business/business-rules.md (RN-NNN)
   - Cenários BDD em Instructions/bdd/ relacionados à feature
   - Contratos em Instructions/contracts/openapi.yaml (rotas da feature)
   - Referências em Instructions/architecture/technical-overview.md
   - Referências em Instructions/architecture/folder-structure.md
   - Referências em Instructions/architecture/architecture-decisions.md
   - Referências em scripts/operational-runbook.md (endpoints, credenciais)

3. IDENTIFICAR ARTEFATOS DE WIKI
   - Página Feature-<NomeDaFeature>.md em wiki/
   - Referências em wiki/Home.md
   - Referências em wiki/_Sidebar.md
   - Referências em wiki/Domain-Business-Rules.md
   - Referências em wiki/Governance-Architecture.md

4. ATUALIZAR GOVERNANÇA PRIMEIRO
   - Marcar regras de negócio como Removidas em business-rules.md (campo Status)
   - Remover cenários BDD da feature
   - Atualizar contratos OpenAPI (remover rotas)
   - Atualizar technical-overview.md (remover componentes)
   - Atualizar folder-structure.md (remover referências)

5. REMOVER CÓDIGO
   - Remover a pasta da Slice inteira
   - Remover registros de DI em Program.cs
   - Remover decorators de cache se exclusivos da feature
   - Remover testes unitários correspondentes

6. ATUALIZAR WIKI
   - Remover página Feature-<NomeDaFeature>.md
   - Atualizar Home.md (remover link)
   - Atualizar _Sidebar.md (remover link)
   - Atualizar Domain-Business-Rules.md (remover referências)
   - Atualizar Governance-Architecture.md (remover da tabela de features)

7. EXECUTAR PIPELINE
   - Seguir o pipeline de escopo Código (passos 0–12)
   - O governance-audit.sh (passo 0.1) validará que referências foram limpas

8. RELATAR
   - Artefatos de código removidos
   - Artefatos de governança atualizados
   - Artefatos de wiki removidos/atualizados
   - Referências cruzadas limpas
```

## Saídas Esperadas
- Feature completamente removida do código
- Governança atualizada sem referências órfãs
- Wiki atualizada sem páginas ou links órfãos
- Pipeline executado com sucesso

## Arquivos de Governança Relacionados
- `.claude/rules/governance-policies.md` §3 — mapa de propagação (entrada "Remoção de Feature")
- `.claude/rules/folder-governance.md` — governança de estrutura de pastas
- `Instructions/wiki/wiki-governance.md` — política de atualização da wiki
- `.claude/rules/governance-audit.md` — checks #6, #11, #25 detectam referências órfãs

## Nota sobre Invocação
Esta skill é ativada quando o assistente identifica que a mensagem do usuário implica remoção de uma feature. Não exige comando especial.

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-04-08 | Criado: skill de remoção de feature com checklist completo de limpeza | Auditoria de governança |
