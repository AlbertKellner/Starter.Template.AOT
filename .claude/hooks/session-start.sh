#!/bin/bash
# Hook: SessionStart — Auto-validate environment and inject context
# Runs once when a new session starts.
# - Cleans stale session state files
# - Prints branch context
# - Verifies critical environment variables
# Exit 0 always (informative, never blocking)

set -euo pipefail

REPO_ROOT="$(git rev-parse --show-toplevel 2>/dev/null || echo ".")"
STALE_THRESHOLD_SEC=14400 # 4 hours

# --- Clean stale session state files ---
for state_file in "$REPO_ROOT/.claude/.pre-planning-done" "$REPO_ROOT/.claude/.pr-analysis-context" "$REPO_ROOT/.claude/.compact-state"; do
  if [[ -f "$state_file" ]]; then
    file_age=$(( $(date +%s) - $(stat -c %Y "$state_file" 2>/dev/null || echo 0) ))
    if (( file_age > STALE_THRESHOLD_SEC )); then
      rm -f "$state_file"
    fi
  fi
done

# --- Inject branch context ---
current_branch=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo "unknown")
recent_commits=$(git log --oneline -3 2>/dev/null || echo "no commits")

echo "[SessionStart] Branch: $current_branch"
echo "[SessionStart] Últimos commits:"
echo "$recent_commits" | sed 's/^/  /'

# --- Verify critical environment variables ---
missing_vars=()
for var in DD_API_KEY GH_CLAUDE_CODE_MCP_CODIFICADOR; do
  if [[ -z "${!var:-}" ]]; then
    missing_vars+=("$var")
  fi
done

if (( ${#missing_vars[@]} > 0 )); then
  echo "[SessionStart] AVISO: Variáveis ausentes: ${missing_vars[*]}"
  echo "[SessionStart] Consultar scripts/required-vars.md para detalhes."
fi

exit 0
