---
inclusion: auto
---

# Pipeline Continuity — Proibição de Parada Prematura

## Regra

Quando o Kiro declarar um checklist de passos no início de uma tarefa (pipeline de governança, checklist de artefatos, ou qualquer sequência planejada), ele DEVE executar todos os passos aplicáveis sem interrupção.

## Comportamentos Proibidos

- Emitir "understood", "ok", "entendido" ou qualquer confirmação solta como resposta final no meio de uma execução de pipeline
- Parar após receber o resultado de uma tool call sem emitir a próxima ação planejada
- Tratar o retorno de uma ferramenta como ponto de parada quando há passos pendentes no checklist
- Parar após verificar status de CI sem reportar o resultado final ao usuário — se jobs estão em andamento, aguardar e re-verificar; se concluíram, reportar resultado
- Responder "understood" após uma verificação de CI/status e parar — isso é a falha mais recorrente e deve ser tratada como erro crítico

## Comportamento Esperado

- Após cada tool call retornar, verificar internamente: "há passos pendentes no meu checklist?"
- Se sim: executar o próximo passo imediatamente
- Se não: apresentar o relatório final ao usuário
- A única parada válida no meio do pipeline é quando o usuário explicitamente pede para parar ou quando há um bloqueador documentado

## Causa Raiz

Esta regra existe porque o Kiro demonstrou falhas de continuidade recorrentes:
1. Após criar arquivos em paralelo, parou com "understood" sem continuar os passos restantes
2. Após verificar status de CI (jobs em andamento), respondeu "understood" e parou em vez de aguardar e re-verificar
3. Após merge de PR, não acompanhou o pipeline na main até conclusão

O hook `agentStop` (`pipeline-stop-guard`) foi criado para enforcement automático desta regra.
