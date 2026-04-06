# Log de Erros de Bash

Este arquivo documenta todos os erros de Bash encontrados durante sessões de trabalho neste repositório, incluindo causa raiz e solução adotada. É um log acumulativo — erros não são removidos após resolvidos.

## Template de Registro

```markdown
## Erro [N] — [Título descritivo do problema]

| Campo | Valor |
|---|---|
| **Número** | [N] |
| **Data** | [YYYY-MM-DD] |
| **Comando executado** | `[comando exato que falhou]` |
| **Erro retornado** | `[mensagem de erro exata]` |
| **Causa** | [Explicação técnica objetiva da causa raiz] |
| **Novo comando / solução** | `[comando ou sequência que resolveu]` |
```

---

> **Estado atual**: nenhum erro registrado. Erros serão documentados à medida que forem encontrados durante sessões de trabalho.

---

## Referências

- `docker-compose.yml` — arquivo principal afetado por correções de infraestrutura
- `src/Starter.Template.AOT.Api/Dockerfile` — modificado para suporte a CA customizada e hash symlinks
- `assumptions-log.md` — premissas de ambiente registradas

## Erro 1 — dotnet não encontrado no PATH padrão

| Campo | Valor |
|---|---|
| **Número** | 1 |
| **Data** | 2026-04-02 |
| **Comando executado** | `git checkout -B claude/number-string-endpoint-gxGKO 2>&1 && dotnet --version 2>&1 && docker --version 2>&1` |
| **Erro retornado** | `/bin/bash: line 2: dotnet: command not found` |
| **Causa** | dotnet SDK instalado em `/root/.dotnet` não está no PATH padrão do shell |
| **Novo comando / solução** | `export PATH="/root/.dotnet:$PATH"` antes de qualquer comando dotnet |

## Erro 2 — governance-audit.sh truncado no check 17

| Campo | Valor |
|---|---|
| **Número** | 2 |
| **Data** | 2026-04-02 |
| **Comando executado** | `bash scripts/governance-audit.sh` |
| **Erro retornado** | Exit code 1 — script trunca na verificação 17 (integridade dos hooks) |
| **Causa** | Regex `"[^"]*"` no check 17 truncava em escaped quotes (`\"`), gerando input malformado para o loop `while read` |
| **Novo comando / solução** | Substituir extração por `grep -oP '\.claude/hooks/[a-zA-Z0-9_-]+\.sh' "$SETTINGS" \| sort -u` — extração direta de paths |

## Erro 3 — Health check antes da app estar pronta

| Campo | Valor |
|---|---|
| **Número** | 3 |
| **Data** | 2026-04-02 |
| **Comando executado** | `curl -s -o /dev/null -w "HTTP %{http_code}" http://localhost:5000/health` |
| **Erro retornado** | `HTTP 000` (exit code 7 — connection refused) |
| **Causa** | App em startup com `NotSupportedException` no `MapControllers()` por `IsEnhancedModelMetadataSupported` false ao processar parâmetro de rota |
| **Novo comando / solução** | Adicionar `SuppressInferBindingSourcesForParameters = true` em `ApiBehaviorOptions` no Program.cs |

## Erro 4 — Health check retry (duplicata do Erro 3)

| Campo | Valor |
|---|---|
| **Número** | 4 |
| **Data** | 2026-04-02 |
| **Comando executado** | `curl -s -o /dev/null -w "HTTP %{http_code}" http://localhost:5000/health` |
| **Erro retornado** | `HTTP 000` (exit code 7) |
| **Causa** | Mesmo que Erro 3 — app ainda crashada antes do fix |
| **Novo comando / solução** | Ver Erro 3 |

## Erro 5 — Docker daemon não rodando

| Campo | Valor |
|---|---|
| **Número** | 5 |
| **Data** | 2026-04-02 |
| **Comando executado** | `docker compose up -d --build` |
| **Erro retornado** | `Cannot connect to the Docker daemon at unix:///var/run/docker.sock. Is the docker daemon running?` |
| **Causa** | Docker daemon não iniciado automaticamente no sandbox |
| **Novo comando / solução** | `dockerd > /dev/null 2>&1 &` seguido de `sleep 3` |

## Erro 6 — DNS failure no Docker build

| Campo | Valor |
|---|---|
| **Número** | 6 |
| **Data** | 2026-04-02 |
| **Comando executado** | `docker compose up -d --build` |
| **Erro retornado** | `Temporary failure resolving 'archive.ubuntu.com' / E: Unable to locate package clang` |
| **Causa** | Resolução DNS indisponível dentro de containers Docker neste sandbox — problema de rede do ambiente, não da aplicação |
| **Novo comando / solução** | Pendente — bloqueio de rede do sandbox. CI validará o Docker build |

## Erro 7 — DNS failure no Docker build (retry com DNS config)

| Campo | Valor |
|---|---|
| **Número** | 7 |
| **Data** | 2026-04-02 |
| **Comando executado** | `docker compose up -d --build` (após configurar DNS 8.8.8.8 em daemon.json) |
| **Erro retornado** | `Temporary failure resolving 'archive.ubuntu.com'` |
| **Causa** | DNS config em daemon.json não propagou para BuildKit. Limitação de rede do sandbox |
| **Novo comando / solução** | Pendente — mesma causa que Erro 6 |

## Erro 8 — Health check antes da app terminar compilação

| Campo | Valor |
|---|---|
| **Número** | 8 |
| **Data** | 2026-04-02 |
| **Comando executado** | `sleep 6 && curl -s -o /dev/null -w "HTTP %{http_code}" http://localhost:5000/health` |
| **Erro retornado** | `HTTP 000` (exit code 7 — connection refused) |
| **Causa** | App ainda compilando (Roslyn) após 6 segundos de espera; precisa de ~15 segundos para iniciar |
| **Novo comando / solução** | Aumentar sleep para 15 segundos ou usar polling loop |

## Erro 9 — git push HTTP 503

| Campo | Valor |
|---|---|
| **Número** | 9 |
| **Data** | 2026-04-02 |
| **Comando executado** | `git push -u origin claude/number-string-endpoint-gxGKO` |
| **Erro retornado** | `error: RPC failed; HTTP 503 curl 22 The requested URL returned error: 503` |
| **Causa** | Servidor git remoto retornando HTTP 503 (Service Unavailable) em todas as tentativas de push |
| **Novo comando / solução** | Retry com backoff exponencial — todas falharam. Aguardar servidor restabelecer |

## Erro 10 — git push HTTP 503 (retry com fetch)

| Campo | Valor |
|---|---|
| **Número** | 10 |
| **Data** | 2026-04-02 |
| **Comando executado** | `git fetch origin && git push origin claude/number-string-endpoint-gxGKO` |
| **Erro retornado** | `error: RPC failed; HTTP 503 curl 22 The requested URL returned error: 503` |
| **Causa** | Mesma causa do Erro 9 — fetch funciona, push bloqueado pelo servidor |
| **Novo comando / solução** | Ver Erro 9 |

## Erro 11 — Captura automática via hook (duplicata do Erro 8)

| Campo | Valor |
|---|---|
| **Número** | 11 |
| **Data** | 2026-04-02 |
| **Comando executado** | `sleep 15 && curl -s -o /dev/null -w "HTTP %{http_code}" http://localhost:5000/health` |
| **Erro retornado** | Capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | Duplicata do Erro 8 — app ainda compilando após 15 segundos |
| **Novo comando / solução** | Ver Erro 8 |

## Erro 12 — MCP servers do GitHub não carregados no startup do Claude Code

| Campo | Valor |
|---|---|
| **Número** | 12 |
| **Data** | 2026-04-02 |
| **Comando executado** | ToolSearch para `mcp__github__*` no início da sessão |
| **Erro retornado** | `No matching deferred tools found` — nenhuma ferramenta MCP do GitHub disponível |
| **Causa** | Inicialização assíncrona dos MCP servers falhou ou excedeu timeout. Endpoint 100% acessível (6/6 testes OK via curl), tokens válidos (ClaudeCode-Bot e Claude-Revisor confirmados). Problema é client-side: Claude Code não completou handshake com os MCP servers no startup. `MCP_TIMEOUT` não estava configurado — timeout padrão insuficiente para payload pesado (20+ tools com ícones base64). |
| **Novo comando / solução** | Adicionar `"MCP_TIMEOUT": "60000"` e `"MCP_TOOL_TIMEOUT": "300000"` em `.claude/settings.json` seção `env`. Adicionar verificação de conectividade MCP ao `session-start.sh` para diagnóstico imediato. |

## Erro 13 — Falso positivo do hook PreToolUse:Bash em comandos com variáveis MCP

| Campo | Valor |
|---|---|
| **Número** | 13 |
| **Data** | 2026-04-02 |
| **Comando executado** | `curl -s -X POST "https://api.githubcopilot.com/mcp/" -H "Authorization: Bearer ${GH_CLAUDE_CODE_MCP_CODIFICADOR}" ...` |
| **Erro retornado** | `[PreToolUse] BLOQUEADO: git push --force detectado. Force push é proibido sem autorização explícita.` |
| **Causa** | O pattern glob `"if": "Bash(git push --force*)"` no hook PreToolUse:Bash casava incorretamente com comandos que expandem variáveis de ambiente. O `*` no final do pattern permitia matching amplo demais quando o conteúdo expandido das variáveis era avaliado. |
| **Novo comando / solução** | Substituir pattern `Bash(git push --force*)` por dois patterns específicos: `Bash(git push --force)` (exato) e `Bash(git push --force *)` (com espaço antes do `*`). Também adicionado pattern separado para `--force-with-lease`. |

## Erro 14 — docker build falha por DNS indisponível no BuildKit (tentativa 1)

| Campo | Valor |
|---|---|
| **Número** | 14 |
| **Data** | 2026-04-04 |
| **Comando executado** | `docker build -t starter-template-debug -f src/Starter.Template.AOT.Api/Dockerfile src/ 2>&1` |
| **Erro retornado** | `E: Unable to locate package clang / E: Unable to locate package zlib1g-dev` — `Temporary failure resolving 'archive.ubuntu.com'` |
| **Causa** | DNS não funciona dentro de containers BuildKit neste sandbox. O `apt-get update` dentro do Dockerfile falha ao resolver `archive.ubuntu.com`. Ver Erro 16 para análise completa com `--network=host`. |
| **Novo comando / solução** | Ver Erro 16: workaround via `dotnet publish` no host (que tem clang instalado) + imagem runtime-only sem `apt-get`. CI/CD com GitHub Actions funciona normalmente pois tem DNS. |

## Erro 15 — docker build falha por DNS indisponível no BuildKit (tentativa 2)

| Campo | Valor |
|---|---|
| **Número** | 15 |
| **Data** | 2026-04-04 |
| **Comando executado** | `docker build -t starter-template-debug -f src/Starter.Template.AOT.Api/Dockerfile src/ 2>&1` |
| **Erro retornado** | `E: Unable to locate package clang / E: Unable to locate package zlib1g-dev` — `Temporary failure resolving 'archive.ubuntu.com'` |
| **Causa** | Mesma causa do Erro 14: DNS não funciona dentro de containers BuildKit neste sandbox. Segunda tentativa com o mesmo comando, mesma falha. |
| **Novo comando / solução** | Ver Erro 16: workaround via `dotnet publish` no host + imagem runtime-only sem `apt-get`. CI/CD com GitHub Actions funciona normalmente pois tem DNS. |

## Erro 16 — Captura automática via hook

| Campo | Valor |
|---|---|
| **Número** | 16 |
| **Data** | 2026-04-04 |
| **Comando executado** | `docker build --network=host -t starter-template-debug -f src/Starter.Template.AOT.Api/Dockerfile src/` |
| **Erro retornado** | `E: Unable to locate package clang / E: Unable to locate package zlib1g-dev` — `Temporary failure resolving 'archive.ubuntu.com'` |
| **Causa** | Mesma causa dos Erros 6 e 7: DNS não funciona dentro de containers BuildKit neste sandbox. `--network=host` não resolve porque o problema é no namespace de rede do BuildKit, não do Docker em si. |
| **Novo comando / solução** | Workaround para sandbox: (1) `dotnet publish` no host (que tem clang instalado); (2) `docker build -f Dockerfile.runtime /tmp/aot-publish/` usando imagem runtime-only sem apt-get. CI com GitHub Actions funciona normalmente pois tem DNS. |

## Erro 17 — ls de .deb inexistentes (erro esperado de diagnóstico)

| Campo | Valor |
|---|---|
| **Número** | 17 |
| **Data** | 2026-04-04 |
| **Comando executado** | `dpkg -l zlib1g-dev 2>&1 \| grep ^ii; ls /var/cache/apt/archives/clang*.deb /var/cache/apt/archives/zlib1g*.deb 2>&1` |
| **Erro retornado** | `ls: cannot access '/var/cache/apt/archives/clang*.deb': No such file or directory` |
| **Causa** | Diagnóstico de ambiente: verificação se os pacotes estavam cacheados no host. `clang` e `zlib1g-dev` estão instalados mas os `.deb` não estão em `/var/cache/apt/archives`. |
| **Novo comando / solução** | Não bloqueante — usado apenas para diagnóstico. |

## Erro 18 — docker stop em container já removido

| Campo | Valor |
|---|---|
| **Número** | 18 |
| **Data** | 2026-04-04 |
| **Comando executado** | `docker stop funny_ptolemy 2>&1; docker rm funny_ptolemy 2>&1` |
| **Erro retornado** | `Error response from daemon: No such container: funny_ptolemy` |
| **Causa** | Container criado com `--rm` já se auto-removeu ao terminar. A tentativa de stop chegou após a remoção automática. |
| **Novo comando / solução** | Não bloqueante — uso de `--rm` é correto para containers de diagnóstico temporários.  |

## Erro 19 — EnhancedModelMetadataActivator falha silenciosamente em Native AOT

| Campo | Valor |
|---|---|
| **Número** | 19 |
| **Data** | 2026-04-04 |
| **Comando executado** | Inicialização da aplicação via `./Starter.Template.AOT.Api` (binário AOT publicado) |
| **Erro retornado** | `DynamicMethod falhou: Dynamic code generation is not supported on this platform.` / `FieldInfo.SetValue falhou: Cannot set initonly static field after its owning type is initialized.` / `IsEnhancedModelMetadataSupported não pôde ser ativado — model binding pode falhar` |
| **Causa** | `EnhancedModelMetadataActivator` tenta definir `ModelMetadata.IsEnhancedModelMetadataSupported = true` via `DynamicMethod` (não suportado em AOT) e `FieldInfo.SetValue` (bloqueado para campos initonly após inicialização do tipo). Em Native AOT, nenhuma das duas abordagens funciona. O warning era enganoso: o model binding NÃO falha pois `FallbackSimpleTypeModelBinderProvider` e `NullModelBinderProvider` já substituem todos os providers que dependem desse flag. |
| **Novo comando / solução** | Corrigido em `EnhancedModelMetadataActivator.cs`: verificação de `RuntimeFeature.IsDynamicCodeSupported` antes de tentar reflection. Em modo AOT, log é emitido em nível `Debug` (não `Warning`) e activator retorna imediatamente. |

## Erro 20 — gh CLI indisponível durante auto-pr-review (graphql de review threads)

| Campo | Valor |
|---|---|
| **Número** | 20 |
| **Data** | 2026-04-04 |
| **Comando executado** | `gh api graphql -f query='{ repository(...) { pullRequest(number: 30) { reviewThreads ... } } }'` |
| **Erro retornado** | Capturado automaticamente pelo hook bash-error-capture.sh — `gh: command not found` ou exit code não-zero por ausência do CLI |
| **Causa** | A skill `auto-pr-review` tentou usar `gh api graphql` para listar review threads, mas o CLI `gh` não está instalado/autenticado neste ambiente. A skill usa MCP GitHub para todas as interações com a API do GitHub — o uso de `gh` foi uma tentativa de fallback desnecessária. |
| **Novo comando / solução** | Não bloqueante — a revisão foi concluída com sucesso via ferramentas MCP. Usar exclusivamente `mcp__github__*` e `mcp__github-revisor__*` para interações GitHub; não depender do CLI `gh`. |

## Erro 21 — Captura automática via hook

| Campo | Valor |
|---|---|
| **Número** | 21 |
| **Data** | 2026-04-06 |
| **Comando executado** | `GH_TOKEN=$(printenv GH_CLAUDE_CODE_MCP_CODIFICADOR) gh pr create \
  --repo "albertkellner/starter.template.aot" \
  --base main \
  --head claude/github-actions-workflow-QA9Ke \
  --title "ci(workflows): adicionar workflow de desenvolvimento e transição pós-refinamento" \
  --body "$(cat <<'EOF'
## Motivos da alteração

O repositório já possuía workflow de refinamento automático de issues, mas faltava o workflow que executa o desenvolvimento após o plano estar aprovado. Também faltava a transição automática de labels ao final do refinamento.

## Plano de execução

1. Criar workflow `claude-development.yml` com ciclo completo: desenvolvimento → review automático → review humano
2. Adicionar job `post-refinement` ao workflow de refinamento existente
3. Estabelecer fluxo de labels: `em-refinamento` → `refinado` → `iniciar-desenvolvimento` → `em desenvolvimento` → `review-automatico` → `review-humano`

## O que foi realizado

### Novo workflow: `claude-development.yml`
- **Job `prepare`**: remove todas as labels, adiciona `em desenvolvimento`, comenta na issue
- **Job `development`**: executa `claude-code-action@v1` com modelo Sonnet, baseado no último plano do Codificador. Inclui restrições de segurança (secrets bloqueados, governança protegida)
- **Job `post-development`**: busca PR via `gh pr list`, comenta conclusão, troca label para `review-automatico`, adiciona Claude-Revisor como reviewer (dispara `claude-reviewer.yml` existente). Se PR não encontrado, troca para `falha-desenvolvimento`
- **Job `wait-review`**: polling a cada 60s verificando aprovação do Claude-Revisor (timeout 30min)
- **Job `post-review`**: comenta aprovação, troca label para `review-humano`, inclui link do PR

### Alteração: `claude-refinement.yml`
- Novo job `post-refinement` após o refinamento: remove labels, adiciona `refinado`, comenta instruindo o usuário a adicionar `iniciar-desenvolvimento`

### Labels necessárias no repositório
- `refinado`, `iniciar-desenvolvimento`, `em desenvolvimento`, `review-automatico`, `review-humano`, `falha-desenvolvimento`
EOF
)" 2>&1` |
| **Erro retornado** | Capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | A ser investigada pelo assistente |
| **Novo comando / solução** | Pendente |
