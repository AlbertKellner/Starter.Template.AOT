## Motivos da alteração

<!-- Descreva os motivos que justificam esta alteração: qual problema foi identificado, qual regra de negócio é implementada, qual comportamento incorreto é corrigido, ou qual melhoria técnica é introduzida. Inclua a causa raiz quando aplicável. -->

## Plano de execução

<!-- Descreva de forma clara e técnica o plano de execução seguido para implementar esta alteração: quais etapas foram planejadas, qual sequência foi adotada e quais decisões técnicas foram tomadas. Use lista numerada. -->

## O que foi realizado

<!-- Descreva de forma completa e técnica tudo o que foi feito neste PR. Inclua: arquivos criados ou modificados, mudanças de comportamento, endpoints adicionados ou alterados, regras de negócio implementadas, e qualquer outro detalhe relevante. Mantenha este campo sempre atualizado com o estado real do PR — remova referências a alterações descartadas e adicione novas quando houver mudanças.

Para PRs de código com endpoints, inclua tabela de comportamento:

| Input | Status | Response |
|---|---|---|
| `GET /exemplo/1` | 200 | `{"campo":"valor"}` |
| `GET /exemplo/999` | 404 | ProblemDetails |

Para PRs de governança, inclua tabela de artefatos:

| Artefato | Ação | Status |
|---|---|---|
| `arquivo.md` | Criado/Modificado | ✅ |
-->

## Validação

<!-- Liste as evidências de que a alteração funciona. Use o formato com emoji de status:

- ✅ `dotnet build` — 0 erros
- ✅ `dotnet test` — N/N testes passando
- ✅ `GET /endpoint` → HTTP 200: `{"campo":"valor"}`
- ✅ `governance-audit.sh` — 0 falhas

Para limitações conhecidas do ambiente, documente o fallback usado:

- ⚠️ Docker indisponível — validação HTTP em modo debug (PENDENCIAS.md #1)
-->

## Checklist

- [ ] Build limpo (`dotnet build` sem erros)
- [ ] Testes passando em modo debug (`dotnet test`)
- [ ] HealthCheck passando (`/health` retorna `Healthy` ou `Degraded` esperado)
- [ ] Endpoints validados via chamada HTTP real (quando aplicável)
- [ ] Governança atualizada antes da implementação (quando aplicável)
- [ ] Título do PR claro, objetivo e tecnicamente descritivo
- [ ] Descrição do PR consistente com o estado real da implementação
- [ ] Título e descrição escritos em português brasileiro
- [ ] Commits seguindo Semantic Commits (`feat:`, `fix:`, `docs:`, `refactor:`, etc.)
