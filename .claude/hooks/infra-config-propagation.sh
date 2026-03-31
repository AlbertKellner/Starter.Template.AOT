#!/bin/bash
# Hook: infra-config-propagation.sh
# Propósito: Detectar quando arquivos de configuração de infraestrutura são alterados
# e emitir lembrete obrigatório de propagação para arquivos de governança.
# Ativação: PostToolUse em Write|Edit
#
# Este hook complementa o instruction-change-detector.sh (que cobre governança)
# com cobertura para arquivos de infraestrutura que possuem reflexo na governança.

FILE_PATH="${1:-}"

# Padrões de arquivos de infraestrutura com reflexo em governança
INFRA_PATTERNS=(
  ".mcp.json"
  "docker-compose.yml"
  "Dockerfile"
  "appsettings.json"
  ".github/workflows/"
  "scripts/setup-env.sh"
)

# Mapa de propagação: arquivo → destinos de governança
declare -A PROPAGATION_MAP
PROPAGATION_MAP[".mcp.json"]="technical-overview.md (Recursos Operacionais), operational-runbook.md (Serviços Externos), required-vars.md (Secrets)"
PROPAGATION_MAP["docker-compose.yml"]="technical-overview.md (Containerização), operational-runbook.md (Containers Docker), required-vars.md (Variáveis)"
PROPAGATION_MAP["Dockerfile"]="technical-overview.md (Containerização), folder-structure.md"
PROPAGATION_MAP["appsettings.json"]="technical-overview.md (Stack), operational-runbook.md (Configuração)"
PROPAGATION_MAP[".github/workflows/"]="technical-overview.md (CI/CD), wiki/Governance-CI-CD.md"
PROPAGATION_MAP["scripts/setup-env.sh"]="container-setup.md, required-vars.md"

IS_INFRA=false
MATCHED_PATTERN=""
for pattern in "${INFRA_PATTERNS[@]}"; do
  if [[ "$FILE_PATH" == *"$pattern"* ]]; then
    IS_INFRA=true
    MATCHED_PATTERN="$pattern"
    break
  fi
done

if [ "$IS_INFRA" = true ]; then
  TARGETS="${PROPAGATION_MAP[$MATCHED_PATTERN]:-arquivos de governança relacionados}"
  echo ""
  echo "[PROPAGAÇÃO OBRIGATÓRIA] Arquivo de infraestrutura alterado: '$FILE_PATH'"
  echo "        Propagar mudança para: $TARGETS"
  echo "        Referência: governance-policies.md §3 — Mapa de propagação"
  echo ""
fi

exit 0
