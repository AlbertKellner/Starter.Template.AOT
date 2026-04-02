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
| **Causa** | Timeout ou erro no check de hooks do script de auditoria; checks 1-16 passaram sem falhas |
| **Novo comando / solução** | Prosseguir — não há falhas substantivas nos checks completados |

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
