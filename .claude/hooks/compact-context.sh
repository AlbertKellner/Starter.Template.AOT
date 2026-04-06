#!/bin/bash
# Hook: PreCompact/PostCompact — Preserve governance context during compaction
# Usage: bash compact-context.sh pre|post
# - PreCompact: saves pipeline state to /tmp/.claude-state-<hash>/.compact-state
# - PostCompact: emits saved state for context re-injection
# Exit 0 always (informative, never blocking)

set -euo pipefail

REPO_ROOT="$(git rev-parse --show-toplevel 2>/dev/null || echo ".")"
_PROJECT_ID="$(echo "$REPO_ROOT" | md5sum 2>/dev/null | head -c 8 || echo "default")"
STATE_DIR="/tmp/.claude-state-${_PROJECT_ID}"
mkdir -p "$STATE_DIR" 2>/dev/null || true
STATE_FILE="$STATE_DIR/.compact-state"

mode="${1:-}"

case "$mode" in
  pre)
    # Save current state before compaction
    {
      echo "COMPACT_TIMESTAMP=$(date +%s)"
      echo "CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD 2>/dev/null || echo 'unknown')"

      # Check if pre-planning was done
      if [[ -f "$STATE_DIR/.pre-planning-done" ]]; then
        echo "PRE_PLANNING_DONE=true"
      else
        echo "PRE_PLANNING_DONE=false"
      fi

      # Check if in pr-analysis mode
      if [[ -f "$STATE_DIR/.pr-analysis-context" ]]; then
        echo "PR_ANALYSIS_ACTIVE=true"
        head_ref=$(grep 'head_ref=' "$STATE_DIR/.pr-analysis-context" | cut -d= -f2 || echo "")
        echo "PR_HEAD_REF=$head_ref"
      else
        echo "PR_ANALYSIS_ACTIVE=false"
      fi

      # Session timer state
      if [[ -f "$STATE_DIR/.session-timer" ]]; then
        echo "SESSION_TIMER_EXISTS=true"
      fi
    } > "$STATE_FILE"

    echo "[PreCompact] Estado do pipeline salvo em $STATE_FILE"
    ;;

  post)
    # Restore context after compaction
    if [[ -f "$STATE_FILE" ]]; then
      source "$STATE_FILE"

      echo "[PostCompact] Contexto restaurado:"
      echo "  Branch: ${CURRENT_BRANCH:-unknown}"
      echo "  Consulta pré-planejamento: ${PRE_PLANNING_DONE:-false}"

      if [[ "${PR_ANALYSIS_ACTIVE:-false}" == "true" ]]; then
        echo "  Modo: Análise de PR (head_ref: ${PR_HEAD_REF:-unknown})"
      fi

      # Clean up state file
      rm -f "$STATE_FILE"
    else
      echo "[PostCompact] Nenhum estado salvo encontrado."
    fi
    ;;

  *)
    echo "[compact-context.sh] Uso: bash compact-context.sh pre|post" >&2
    ;;
esac

exit 0
