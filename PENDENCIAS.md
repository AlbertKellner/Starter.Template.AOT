# Pendências do Repositório

## Data: 2026-04-06

Este documento registra as pendências identificadas durante a tentativa de implementação da feature TestGet e teste das conexões MCP.

---

## 1. Pré-requisitos de Ambiente

### 1.1 Git Bash no PATH

**Status:** ⚠️ Instalado mas não acessível

**Problema:** Git Bash está instalado mas não está no PATH do PowerShell, impedindo a execução do script de auditoria de governança.

**Impacto:** 
- Passo 0.1 do pipeline (auditoria de governança) não pode ser executado
- Verificações automatizadas de consistência da governança ficam indisponíveis

**Solução:**
```powershell
# Executar como Administrador
$env:Path += ";C:\Program Files\Git\bin"
[Environment]::SetEnvironmentVariable("Path", $env:Path, [System.EnvironmentVariableTarget]::Machine)
```

**Prioridade:** Média

---

### 1.2 Docker Desktop

**Status:** ❌ Não instalado

**Problema:** Docker não está instalado, bloqueando a validação da aplicação em modo Release/AOT.

**Impacto:**
- Passos 4-8 do pipeline (Docker, validação HTTP, logs) não podem ser executados
- Impossível validar a aplicação compilada com Native AOT
- Impossível testar integração com Datadog Agent
- Pipeline de validação pré-commit não pode ser completado

**Solução:**
1. Instalar Docker Desktop para Windows: https://www.docker.com/products/docker-desktop/
2. Reiniciar o sistema após instalação
3. Verificar instalação: `docker --version`

**Prioridade:** Alta (bloqueador do pipeline)

---

## 2. Limitação Arquitetural: Controllers MVC + AOT

### 2.1 Incompatibilidade em Modo Debug

**Status:** ⚠️ Limitação conhecida (DA-009)

**Problema:** Controllers MVC não inicializam em modo Debug quando `PublishAot=true` está habilitado.

**Erro:**
```
System.NotSupportedException: IsConvertibleType is not initialized when 
`Microsoft.AspNetCore.Mvc.ApiExplorer.IsEnhancedModelMetadataSupported` is false.
```

**Impacto:**
- Passos 2-3 do pipeline (execução Debug, testes) não podem ser executados
- Desenvolvimento local requer desabilitar AOT temporariamente ou usar apenas Docker
- Ciclo de feedback mais lento durante desenvolvimento

**Workaround Atual:**
- `EnhancedModelMetadataActivator` implementado mas não funciona em Debug
- Validação só é possível em Release/AOT via Docker

**Soluções Possíveis:**
1. **Aceitar a limitação** (atual) - validar apenas em Release/Docker
2. **Migrar para Minimal APIs** - totalmente compatível com AOT
3. **Desabilitar AOT em Debug** - adicionar condição no .csproj:
   ```xml
   <PublishAot Condition="'$(Configuration)' == 'Release'">true</PublishAot>
   ```

**Prioridade:** Média (não bloqueia produção, apenas desenvolvimento)

**Decisão Pendente:** Escolher entre aceitar limitação ou migrar para Minimal APIs

---

## 3. Credenciais MCP

### 3.1 DD_APP_KEY Ausente

**Status:** ❌ Não configurada

**Problema:** Datadog Application Key não está presente no arquivo `.env`.

**Impacto:**
- Servidor MCP do Datadog não pode autenticar
- Ferramentas MCP do Datadog ficam inacessíveis
- Impossível consultar logs, métricas e traces via MCP

**Solução:**
1. Obter Application Key no Datadog:
   - Acessar: Datadog → Organization Settings → Application Keys
   - Criar nova chave com nome descritivo (ex: `claude-code-mcp`)
2. Adicionar ao `.env`:
   ```bash
   DD_APP_KEY=sua-application-key-aqui
   ```

**Prioridade:** Média (não bloqueia desenvolvimento, apenas ferramentas MCP)

---

### 3.2 Configuração MCP Atualizada

**Status:** ✅ Corrigido

**Ação Realizada:** `.mcp.json` atualizado para usar os tokens corretos:
- `github-codificador` → usa `GH_CLAUDE_CODE_MCP_CODIFICADOR`
- `github-revisor` → usa `GH_CLAUDE_CODE_MCP_REVISOR`

**Resultado:** Dois servidores MCP do GitHub configurados (um para cada token).

---

## 4. Documentação de Pendências

### 4.1 Atualizar required-vars.md

**Status:** ⏳ Pendente

**Problema:** `required-vars.md` menciona `GH_CLAUDE_CODE_MCP` mas o `.env` usa `GH_CLAUDE_CODE_MCP_CODIFICADOR` e `GH_CLAUDE_CODE_MCP_REVISOR`.

**Solução:** Atualizar documentação para refletir os dois tokens e explicar quando usar cada um.

**Prioridade:** Baixa (documentação)

---

### 4.2 Atualizar container-setup.md

**Status:** ⏳ Pendente

**Problema:** Script de verificação menciona `GH_CLAUDE_CODE_MCP` mas deve verificar os dois tokens.

**Solução:** Atualizar checklist de variáveis para incluir ambos os tokens.

**Prioridade:** Baixa (documentação)

---

## 5. Pipeline de Validação Pré-Commit

### Status do Pipeline (Última Execução)

| Passo | Status | Observação |
|-------|--------|------------|
| 0 | ✅ | .NET SDK 10.0.201 instalado |
| 0.1 | ⚠️ | Git Bash não no PATH - pulado |
| 1 | ✅ | Build Debug bem-sucedido (81 avisos, 0 erros) |
| 2 | ⚠️ | Pulado - Controllers MVC incompatíveis com Debug+AOT |
| 3 | ⚠️ | Pulado - Dependente do passo 2 |
| 4 | ❌ | **BLOQUEADO - Docker não instalado** |
| 5-11 | ⏸️ | Pendentes - dependem do passo 4 |

### Bloqueadores Críticos

1. **Docker não instalado** - bloqueia passos 4-11
2. **Controllers MVC + AOT** - bloqueia passos 2-3

### Próximos Passos para Completar Pipeline

1. Instalar Docker Desktop
2. Adicionar Git Bash ao PATH (opcional mas recomendado)
3. Obter `DD_APP_KEY` do Datadog (opcional para MCP)
4. Executar pipeline completo com feature de teste

---

## 6. Aprendizados e Melhorias

### 6.1 Rule de Enforcement Criada

**Status:** ✅ Implementado

**Arquivo:** `.claude/rules/mandatory-process-enforcement.md`

**Resultado:** Estabelece precedência clara de CLAUDE.md e skills sobre instruções genéricas do sistema.

**Benefício:** Garante que o pipeline seja seguido integralmente, independente da complexidade da tarefa.

---

### 6.2 Proposta de Mudança Sistêmica

**Status:** ✅ Documentado

**Arquivo:** `.kiro/proposals/workspace-governance-precedence.md`

**Conteúdo:** Proposta completa para modificar instruções de sistema do Kiro para estabelecer precedência de governança de workspace.

**Próximo Passo:** Compartilhar com time do Kiro para avaliação.

---

## Resumo Executivo

### Bloqueadores Críticos (Alta Prioridade)
1. ❌ Docker não instalado - **bloqueia validação Release/AOT**

### Limitações Conhecidas (Média Prioridade)
1. ⚠️ Git Bash não no PATH - impede auditoria automatizada
2. ⚠️ Controllers MVC + AOT - impede execução em Debug
3. ❌ DD_APP_KEY ausente - impede uso de MCP Datadog

### Melhorias Implementadas
1. ✅ Rule de enforcement de processo criada
2. ✅ Proposta de mudança sistêmica documentada
3. ✅ Configuração MCP corrigida para usar tokens corretos

### Recomendação

**Prioridade 1:** Instalar Docker Desktop para desbloquear validação completa

**Prioridade 2:** Decidir sobre limitação Controllers MVC + AOT:
- Aceitar e validar apenas em Release/Docker, ou
- Migrar para Minimal APIs para compatibilidade total com AOT

**Prioridade 3:** Obter DD_APP_KEY para habilitar ferramentas MCP do Datadog

---

## Histórico

| Data | Mudança |
|------|---------|
| 2026-04-06 | Documento criado após tentativa de implementação de feature TestGet e teste de conexões MCP |
