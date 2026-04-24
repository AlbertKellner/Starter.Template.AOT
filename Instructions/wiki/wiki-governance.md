# Governança da Documentação (Hugo Pages)

## Propósito

Este arquivo define como o assistente deve criar, manter e evoluir a documentação publicada via Hugo no GitHub Pages deste repositório.

---

## Migração Wiki → Hugo Pages

A documentação foi migrada da GitHub Wiki para Hugo Pages (tema Docsy). A pasta `wiki/` permanece no repositório como referência histórica, mas a fonte canônica da documentação pública é agora `docs/content/`.

| Antes | Depois |
|-------|--------|
| Fonte: `wiki/*.md` | Fonte: `docs/content/**/*.md` |
| Publicação: `wiki-publish.yml` → GitHub Wiki | Publicação: `hugo-pages.yml` → GitHub Pages |
| URL: GitHub Wiki do repositório | URL: `https://AlbertKellner.github.io/Starter.Template.AOT/` |

---

## Princípio Fundamental

> A documentação prioriza clareza, previsibilidade e facilidade de navegação.
> Toda página deve ser verificável diretamente nos arquivos do repositório ou na governança.
> Documentação especulativa ou aspiracional não pertence ao site.

---

## Organização por Seções

A documentação é organizada em três seções no Hugo:

| Seção | Pasta | Propósito |
|-------|-------|-----------|
| **Governança** | `docs/content/governanca/` | Diretrizes, padrões, restrições, decisões técnicas e operacionais |
| **Domínio e Negócio** | `docs/content/dominio/` | Regras de negócio, features implementadas, conceitos de domínio |
| **Claude** | `docs/content/claude/` | Skills, hooks, convenções e comportamentos do assistente |

---

## Estrutura de Arquivos Hugo

```
docs/
├── hugo.toml              # Configuração principal (tema Docsy, baseURL, idioma)
├── go.mod                 # Dependência do tema Docsy via Hugo Modules
├── content/
│   ├── _index.md          # Home page com navegação por seção
│   ├── governanca/        # 11 páginas de governança
│   ├── dominio/           # Visão geral, regras, features
│   │   └── features/      # Uma página por feature
│   └── claude/            # 5 páginas sobre o Claude
└── static/                # Assets estáticos (se necessário)
```

---

## Padrão de Front-Matter

Toda página Hugo deve ter front-matter YAML:

```yaml
---
title: "Título da Página"
linkTitle: "Título Curto"
weight: 10
description: "Descrição breve para SEO e navegação"
---
```

O `weight` controla a ordem na sidebar. Valores menores aparecem primeiro.

---

## Publicação

A publicação é automática via GitHub Actions (`hugo-pages.yml`):
- Trigger: push na `main` que altere `docs/**`
- Também pode ser disparado manualmente via `workflow_dispatch`
- O workflow instala Hugo + Go, resolve o tema Docsy via Hugo Modules, builda e faz deploy no GitHub Pages

---

## Referências

- Workflow: `.github/workflows/hugo-pages.yml`
- Configuração Hugo: `docs/hugo.toml`
- Tema: [Docsy](https://www.docsy.dev/)
