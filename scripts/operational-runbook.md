# Runbook Operacional do Projeto

## Propósito

Este arquivo é o ponto de entrada único para qualquer pessoa que precise executar, depurar ou validar este projeto. Concentra as informações operacionais mais consultadas — portas, URLs, comandos, credenciais de teste, problemas recorrentes e dependências externas — em um único local, eliminando a necessidade de consultar múltiplos arquivos para responder perguntas operacionais básicas.

---

## Referência Rápida — Portas, URLs e Serviços

| Serviço | Contexto | Host:Porta | URL de Verificação | Propósito |
|---|---|---|---|---|
| Aplicação | Docker (`docker compose`) | `localhost:8080` | `GET http://localhost:8080/health` | Aplicação em modo Release/Native AOT via container |
| Aplicação | Debug (`dotnet run`) | `localhost:5000` | `GET http://localhost:5000/health` | Aplicação em modo debug local |
| Aplicação | CI (GitHub Actions) | `localhost:5000` | `GET http://localhost:5000/health` | Execução no pipeline de CI |
| Datadog Agent | Docker (interno) | `datadog-agent:8126` | — | Recebe traces e logs da aplicação via rede Docker |
| Datadog Agent | CI | `localhost:8126` | — | Recebe traces e logs no pipeline de CI |

> **Nota**: A porta `8080` é usada apenas via Docker. Em modo debug e no CI, a porta padrão é `5000` (configurável via `ASPNETCORE_URLS`).

---

## Credenciais de Teste

> **Nota**: As credenciais abaixo são exemplos do template. Devem ser substituídas conforme o domínio do projeto.

| Item | Valor | Onde Está Definido |
|---|---|---|
| Usuário de teste | Definido no UseCase de login | UseCase da Feature de login em `Features/Command/` |
| Senha de teste | Definida no UseCase de login | Idem |
| ID do usuário | Definido no UseCase de login | Idem |
| JWT Secret (dev) | `super-secret-key-for-development-only-change-in-production` | `appsettings.json` → `Jwt:Secret` |

> **Aviso**: Estas credenciais são hardcoded para desenvolvimento. Em produção, devem ser substituídas por mecanismo seguro.

---

## Comandos Essenciais por Etapa

### 1. Build
```bash
dotnet build src/Starter.Template.AOT.Api/Starter.Template.AOT.Api.csproj
```

### 2. Execução em modo debug
```bash
dotnet run --project src/Starter.Template.AOT.Api/Starter.Template.AOT.Api.csproj
# Verificar: curl http://localhost:5000/health
```

### 3. Testes unitários
```bash
dotnet test src/Starter.Template.AOT.UnitTest/Starter.Template.AOT.UnitTest.csproj
```

### 4. Docker (Release/Native AOT)
```bash
docker compose up -d --build    # build + start
# Verificar: curl http://localhost:8080/health
```

### 5. Login (obter Bearer Token)
```bash
# Substituir userName e password pelas credenciais definidas no UseCase de login
TOKEN=$(curl -s -X POST http://localhost:8080/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"<usuario>","password":"<senha>"}' \
  | grep -o '"token":"[^"]*"' | cut -d'"' -f4)
echo $TOKEN
```

### 6. Chamar endpoint autenticado
```bash
# Substituir pela rota do endpoint implementado
curl -H "Authorization: Bearer $TOKEN" "http://localhost:8080/<rota-do-endpoint>"
```

### 7. Logs do container da aplicação
```bash
docker logs $(docker compose ps -q app) --tail 50
```

### 8. Parar containers
```bash
docker compose down
```

---

## Endpoints Disponíveis

| Método | Rota | Autenticação | Descrição | Regra de Negócio |
|---|---|---|---|---|
| `GET` | `/health` | Não | Verificação de disponibilidade (app + Datadog Agent) | RN-005 |
| `GET` | `/disk-drives` | Não | Lista unidades de disco disponíveis (índice, nome, tipo) | — |
| `GET` | `/disk-items/{driveIndex}` | Não | Árvore completa de pastas/arquivos de uma unidade com cores | — |
| `GET` | `/disk-items/{driveIndex}/folder/{*folderPath}` | Não | Subárvore de pasta específica dentro de uma unidade | — |

---

## Tempos Médios do CI (GitHub Actions)

Esta tabela registra os tempos médios observados dos jobs do pipeline de CI. O assistente deve consultar esta tabela antes de iniciar o polling do CI (passo 11) para calibrar os intervalos de espera.

| Job | Tempo médio | Intervalo de polling recomendado | Última atualização |
|---|---|---|---|
| Compilação | ~75s | 80s antes do primeiro check | 2026-04-08 |
| Execução | ~120s | 60s entre checks subsequentes | 2026-04-08 |
| Pipeline total (wall clock) | ~195s | — | 2026-04-08 |

> **Nota**: Estes valores são estimativas iniciais. Devem ser atualizados periodicamente com base nos dados reais de `scripts/pipeline-timing.sh`. Quando os tempos observados divergirem significativamente (>30%) dos registrados, atualizar esta tabela.

**Como atualizar**: após a conclusão do CI, executar `scripts/pipeline-timing.sh` com os check runs do PR e comparar com os valores desta tabela. Se houver divergência significativa, atualizar os tempos médios e a data.

---

## Problemas Recorrentes e Soluções

Esta tabela consolida os problemas de ambiente mais frequentes, extraídos de `bash-errors-log.md`. Para cada sintoma, a causa e a solução já são conhecidas.

| # | Sintoma | Causa | Solução | Ref |
|---|---|---|---|---|

> **Nota**: Esta tabela é preenchida à medida que problemas são encontrados e documentados em `bash-errors-log.md`. No template base, não há problemas recorrentes registrados.

---

## Padrões de Erro de Implementação Conhecidos

Esta tabela consolida padrões recorrentes de erro de lógica/implementação que causam falha em `dotnet test` ou `dotnet run`. O assistente deve consultar esta tabela antes de implementar novas features para prevenir erros já conhecidos.

| # | Padrão de Erro | Sintoma | Prevenção | Ref |
|---|---|---|---|---|
| 1 | Tipo não registrado no DI | `InvalidOperationException: Unable to resolve service for type` | Registrar UseCase, Repository e interfaces no `Program.cs` antes de testar | — |
| 2 | Tipo não adicionado ao AppJsonContext | Serialização falha em AOT; resposta vazia ou 500 | Adicionar `[JsonSerializable(typeof(NovaTipo))]` em `Infra/Json/AppJsonContext.cs` | DA-009 |
| 3 | Controller não preservado pelo trimmer AOT | 404 em endpoints após publish AOT | Adicionar `[DynamicDependency]` em `AotControllerPreservation` | DA-009 |
| 4 | Namespace de Slice inclui subpasta do componente | Colisão `Namespace.Tipo`; erro de compilação | Namespace para na Feature, não na subpasta (ver naming-conventions.md) | — |
| 5 | Model de Feature usando tipo de Shared/ExternalApi | Acoplamento de contrato; teste falha ao mockar | Criar Output model próprio em `<Feature>Models/` e mapear (DA-020) | DA-020 |
| 6 | Feature com endpoint sem wiki page | governance-audit falha nos checks 7, 20, 24, 37 | Criar `wiki/Feature-<Nome>.md`, adicionar linha em `wiki/Governance-Architecture.md` e no runbook. Consultar checklist do PAD-001 | PAD-001 |

> **Nota**: Esta tabela é preenchida à medida que padrões são identificados em sessões de desenvolvimento. O assistente deve verificar proativamente se algum padrão se aplica à implementação atual.

**Política de evolução**: quando um erro de lógica/implementação ocorrer pela segunda vez em sessões distintas (mesmo padrão, causa raiz similar), o assistente deve adicionar o padrão a esta tabela como medida preventiva. A adição é parte da tarefa atual — não requer sessão dedicada.

---

## Serviços Externos e Dependências

| Serviço | URL Base | Autenticação | Variável de Configuração | Impacto se Indisponível |
|---|---|---|---|---|
| Datadog | `app.datadoghq.com` | API Key | `DD_API_KEY` no ambiente/`.env` | `/health` retorna `Degraded` ou `Unhealthy`; logs não fluem ao Datadog |
| Datadog MCP | `mcp.datadoghq.com` | API Key + App Key | `DD_API_KEY` + `DD_APP_KEY` no ambiente | Ferramentas MCP do Datadog ficam indisponíveis para o assistente |
| GitHub MCP (Codificador) | `api.githubcopilot.com` | Bearer Token (PAT) | `GH_CLAUDE_CODE_MCP_CODIFICADOR` no ambiente | Ferramentas MCP do GitHub (Codificador) ficam indisponíveis (criação/atualização de PRs, monitoramento de Actions) |
| GitHub MCP (Revisor) | `api.githubcopilot.com` | Bearer Token (PAT) | `GH_CLAUDE_CODE_MCP_REVISOR` no ambiente | Ferramentas MCP do GitHub (Revisor) ficam indisponíveis (revisão automática de PRs) |

---

## Containers Docker

| Serviço (docker-compose) | Container Name | Imagem | Portas Expostas |
|---|---|---|---|
| `app` | (gerado pelo compose) | Build local via Dockerfile | `8080:8080` |
| `datadog-agent` | `dd-agent` | `registry.datadoghq.com/agent:7` | Nenhuma exposta ao host (rede interna) |

### Variáveis de Ambiente do Datadog Agent (docker-compose)

| Variável | Valor | Propósito |
|---|---|---|
| `DD_API_KEY` | Secret do ambiente | Autenticação com Datadog |
| `DD_SITE` | `datadoghq.com` | Região do Datadog |
| `DD_ENV` | `${DD_ENV:-local}` | Ambiente para filtragem (local, ci, build) |
| `DD_HOSTNAME` | `starter-template-aot-local` | Hostname fixo para evitar erro de detecção |
| `DD_LOGS_ENABLED` | `true` | Coleta de logs ativa |
| `DD_LOGS_CONFIG_CONTAINER_COLLECT_ALL` | `true` | Coleta de todos os containers |
| `DD_CONVERT_DD_SITE_FQDN_ENABLED` | `false` | Desabilita FQDN com trailing dot (incompatível com proxy) |
| `DD_DOGSTATSD_NON_LOCAL_TRAFFIC` | `true` | Aceita métricas de outros containers |

---

## Inicialização da GitHub Wiki

A GitHub Wiki precisa ser inicializada manualmente **uma única vez** antes que a publicação automática via `wiki-publish.yml` funcione. Sem esta etapa, o workflow falha ao tentar clonar o repositório wiki.

### Procedimento

1. Acessar o repositório no GitHub → aba **Wiki**
2. Clicar em **Create the first page**
3. Salvar a página com qualquer conteúdo (será sobrescrita pelo workflow)
4. Ir para aba **Actions** → workflow **Publicar Wiki** → **Run workflow** (`workflow_dispatch`)
5. Verificar que o workflow concluiu com sucesso
6. Confirmar na aba Wiki que as 20 páginas de `wiki/` estão publicadas

> **Referência**: `Instructions/wiki/wiki-governance.md` — governança completa da Wiki

---

## Referências Cruzadas

- `scripts/required-vars.md` — detalhes de cada secret e variável de ambiente, ciclo de vida de credenciais
- `scripts/container-setup.md` — dependências de sistema e configuração do container
- `scripts/setup-env.sh` — script de bootstrap (modelo declarativo)
- `.claude/rules/environment-readiness.md` — política de validação de ambiente do agente
- `bash-errors-log.md` — histórico completo e detalhado de todos os erros encontrados
- `docker-compose.yml` — definição dos containers
- `src/Starter.Template.AOT.Api/Dockerfile` — build multi-stage com Native AOT

---

## Histórico de Mudanças

| Data | Mudança | Referência |
|---|---|---|
| 2026-03-19 | Criado: runbook operacional unificado com portas, URLs, comandos, troubleshooting e dependências externas | Instrução do usuário |
| 2026-03-30 | Adicionado: seção de inicialização da GitHub Wiki com procedimento passo-a-passo | Verificação de conformidade de governança |
| 2026-04-08 | Adicionado: seção "Tempos Médios do CI" com intervalos de polling recomendados para calibração do passo 11 | Análise de causa raiz — polling com valores arbitrários |
| 2026-04-08 | Adicionado: seção "Padrões de Erro de Implementação Conhecidos" — checklist de erros recorrentes de lógica/implementação para prevenção proativa | Auditoria de governança — rodada 7 |
| 2026-04-08 | Adicionado: padrão de erro #6 — feature com endpoint sem wiki page | Análise de causa raiz — omissão recorrente de wiki |
