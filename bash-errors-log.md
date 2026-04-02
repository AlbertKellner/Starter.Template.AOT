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

## Erro 1 — git push HTTP 503

| Campo | Valor |
|---|---|
| **Número** | 1 |
| **Data** | 2026-04-02 |
| **Comando executado** | `git push -u origin claude/number-string-endpoint-gxGKO` |
| **Erro retornado** | `error: RPC failed; HTTP 503 curl 22 The requested URL returned error: 503` |
| **Causa** | Servidor git remoto retornando HTTP 503 (Service Unavailable) em todas as tentativas de push |
| **Novo comando / solução** | Retry com backoff exponencial — todas falharam. Aguardar servidor restabelecer |

## Erro 2 — git push HTTP 503 (retry com fetch)

| Campo | Valor |
|---|---|
| **Número** | 2 |
| **Data** | 2026-04-02 |
| **Comando executado** | `git fetch origin && git push origin claude/number-string-endpoint-gxGKO` |
| **Erro retornado** | `error: RPC failed; HTTP 503 curl 22 The requested URL returned error: 503` |
| **Causa** | Mesma causa do Erro 1 — fetch funciona, push bloqueado pelo servidor |
| **Novo comando / solução** | Ver Erro 1 |
