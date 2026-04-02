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

## Erro 14 — pkill retorna exit code 144 ao encerrar app (falso positivo)

| Campo | Valor |
|---|---|
| **Número** | 14 |
| **Data** | 2026-04-02 |
| **Comando executado** | `pkill -f "dotnet run" 2>/dev/null; echo "App encerrada"` |
| **Erro retornado** | Exit code 144 — capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | `pkill` envia SIGTERM ao processo dotnet; o shell reporta exit code 128+16=144 (SIGTERM). Comportamento esperado ao encerrar processos em background — não é erro real. |
| **Novo comando / solução** | Falso positivo — nenhuma ação necessária. O hook captura todos os exit codes não-zero indiscriminadamente. |

## Erro 15 — pkill + cat de log vazio (falso positivo)

| Campo | Valor |
|---|---|
| **Número** | 15 |
| **Data** | 2026-04-02 |
| **Comando executado** | `sleep 2 && pkill -f "dotnet run" 2>/dev/null; sleep 1; cat /tmp/app.log 2>/dev/null` |
| **Erro retornado** | Exit code 144 — capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | Mesmo que Erro 14 — pkill retorna 144 (SIGTERM). O arquivo /tmp/app.log estava vazio porque a app não foi redirecionada para ele nessa execução. |
| **Novo comando / solução** | Falso positivo — ver Erro 14 |

## Erro 16 — pkill de processos dotnet (falso positivo)

| Campo | Valor |
|---|---|
| **Número** | 16 |
| **Data** | 2026-04-02 |
| **Comando executado** | `pkill -f "dotnet" 2>/dev/null; sleep 2; echo "done"` |
| **Erro retornado** | Exit code 144 — capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | Mesmo que Erro 14 — pkill retorna 144 ao encerrar processos dotnet |
| **Novo comando / solução** | Falso positivo — ver Erro 14 |

## Erro 17 — pkill ao encerrar app após validação (falso positivo)

| Campo | Valor |
|---|---|
| **Número** | 17 |
| **Data** | 2026-04-02 |
| **Comando executado** | `pkill -f "dotnet run" 2>/dev/null; sleep 1; echo "App encerrada"` |
| **Erro retornado** | Exit code 144 — capturado automaticamente pelo hook bash-error-capture.sh |
| **Causa** | Mesmo que Erro 14 — pkill retorna 144 ao encerrar app em background |
| **Novo comando / solução** | Falso positivo — ver Erro 14 |
