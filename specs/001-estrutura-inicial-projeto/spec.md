# 001 - Estrutura inicial do projeto Unity

Status: rascunho

## Objetivo

Criar o esqueleto do projeto em `C:\programming\freudinho` seguindo a arquitetura definida em [`ARQUITETURA.md`](../../docs/ARQUITETURA.md), para que os próximos specs (mecânica de sessão, economia, vínculo etc.) tenham onde encaixar código e assets.

## Requisitos

- Repositório git inicializado na raiz do projeto, com `.gitignore` apropriado pra Unity.
- Projeto Unity (template 2D, versão LTS mais recente) criado em `C:\programming\freudinho`.
- Estrutura de pastas em `Assets/` conforme `ARQUITETURA.md` (`Art`, `Prefabs`, `Scenes`, `Scripts/{Core,Data,Systems,Presentation}`, `Data`).
- Cenas `Boot.unity` e `Office.unity` criadas (vazias, apenas placeholders — `Boot` carrega e troca pra `Office`).
- Scripts-base (stubs, sem lógica) para os serviços listados em `ARQUITETURA.md`: `GameStateManager`, `EconomyService`, `BondService`, `MentalHealthService`, `SessionController`, `DayCycleController`, `PatientGenerator`, `SaveService`, `RemoteConfigService` — assinatura e responsabilidade documentada em comentário curto.
- ScriptableObjects-base: `PatientDefinition`, `FurnitureDefinition`, `BalanceConfig`, com os campos já definidos (sem instância/asset populado ainda).

## Critérios de aceite

- [ ] Projeto abre no Unity Hub sem erro.
- [ ] Estrutura de pastas bate com `ARQUITETURA.md`.
- [ ] `Boot.unity` carrega e troca pra `Office.unity` sem erro (lógica mínima, sem gameplay real).
- [ ] Scripts-base compilam sem erro (mesmo sem lógica).
- [ ] `git status` limpo após commit inicial (`Library/`, `Temp/`, `Obj/`, `Build/` etc. ignorados).

## Fora de escopo

- Qualquer lógica de jogo real (desgaste, vínculo, economia) — fica pros próximos specs.
- Arte/sprites — estilo visual ainda não definido (ver "Em aberto" em `ARQUITETURA.md`).
- Remote Config real — por ora só a interface do serviço.

## Bloqueio conhecido

Unity Hub e o Editor **não estão instalados** nesta máquina. Instalação exige instalador gráfico e login de conta Unity — passo manual do Daniel, não automatizável por aqui. O restante do spec (git, pastas, docs, stubs de script como texto) pode ser preparado antes disso; a criação do projeto Unity propriamente dito (`ProjectSettings/`, `Packages/`, cenas `.unity`) depende do Editor instalado.

## Abordagem técnica

Aplica a estrutura de pastas e responsabilidades já definidas em `ARQUITETURA.md`; este spec é a execução prática daquele documento, não uma nova decisão de arquitetura.
