---
title: "Starter.Template.AOT"
linkTitle: "Home"
weight: 1
---

# Starter.Template.AOT

API web construída com **ASP.NET Core** em **.NET 10**, compilada com **Native AOT**. Implementa autenticação por **JWT Bearer Token**, logging estruturado com **Serilog** e arquitetura **Vertical Slice**.

Esta documentação organiza todo o conhecimento do projeto em quatro agrupamentos temáticos: Governança, Domínio e Negócio, Claude, e Referências Técnicas.

---

## Governança

Diretrizes, padrões, restrições, decisões técnicas e operacionais deste repositório.

| Página | Descrição |
|--------|-----------|
| [Arquitetura]({{< relref "governanca/arquitetura" >}}) | Estilo Vertical Slice, estrutura de pastas, componentes e fluxo de request |
| [Padrões de Desenvolvimento]({{< relref "governanca/padroes-desenvolvimento" >}}) | Vertical Slice, CQRS, UseCase, Decorator, validação em Input |
| [Convenções de Código]({{< relref "governanca/convencoes-codigo" >}}) | Nomenclatura, namespaces, variáveis, padrão de logging SNP-001 |
| [Testes]({{< relref "governanca/testes" >}}) | Estratégia de testes, padrões e cobertura |
| [Segurança]({{< relref "governanca/seguranca" >}}) | Autenticação JWT, proteção de endpoints |
| [Observabilidade]({{< relref "governanca/observabilidade" >}}) | Correlation ID, Serilog, Datadog Agent |
| [CI/CD e Deploy]({{< relref "governanca/ci-cd" >}}) | Pipelines de build, execução e validação |
| [Integrações]({{< relref "governanca/integracoes" >}}) | Padrão Refit + Polly, Memory Cache, APIs externas |
| [Operação]({{< relref "governanca/operacao" >}}) | Pré-requisitos, configuração, build, Docker |
| [Qualidade e Manutenção]({{< relref "governanca/qualidade" >}}) | Tratamento de exceções, Problem Details |
| [Restrições e Decisões]({{< relref "governanca/decisoes" >}}) | Decisões arquiteturais, restrições AOT, evolução |

---

## Domínio e Negócio

| Página | Descrição |
|--------|-----------|
| [Visão Geral do Domínio]({{< relref "dominio/visao-geral" >}}) | Propósito da aplicação e conceitos de domínio |
| [Regras de Negócio]({{< relref "dominio/regras-negocio" >}}) | Índice das regras de negócio com links para as Features |

### Funcionalidades (Features)

| Página | Endpoint | Descrição |
|--------|----------|-----------|
| [Health Check]({{< relref "dominio/features/health" >}}) | `GET /health` | Verificação de disponibilidade da aplicação |
| [Listar Drives]({{< relref "dominio/features/disk-drives-get-all" >}}) | `GET /disk-drives` | Lista drives disponíveis no sistema |
| [Escanear Drive]({{< relref "dominio/features/disk-structure-get-by-drive" >}}) | `GET /disk-structure/{drive}` | Escaneia estrutura completa de um drive |
| [Buscar Pasta]({{< relref "dominio/features/disk-structure-get-by-folder" >}}) | `GET /disk-structure/{drive}/folder/{folder}` | Busca estrutura de pasta específica |

---

## Claude

| Página | Descrição |
|--------|-----------|
| [Visão Geral]({{< relref "claude/visao-geral" >}}) | Sistema de governança operacional e pipeline de validação |
| [Skills]({{< relref "claude/skills" >}}) | Catálogo de skills disponíveis por tipo de ativação |
| [Hooks]({{< relref "claude/hooks" >}}) | Hooks configurados e seus comportamentos |
| [Convenções e Restrições]({{< relref "claude/convencoes" >}}) | Comportamentos obrigatórios, linguagem, restrições |
| [Recursos Avançados]({{< relref "claude/recursos-avancados" >}}) | Frontmatter, agentes dedicados, contexto dinâmico, proteção |
