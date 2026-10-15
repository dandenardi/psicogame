# 002 - Mecânica de sessão

Status: rascunho

## Objetivo

Trazer pra dentro da cena `Office.unity` a mecânica já validada nos protótipos (ver [`CONTEXTO.md`](../../docs/CONTEXTO.md)): um paciente aparece, "fala" em balão de emoji, e de vez em quando surge um insight (💡) no balão de pensamento do Freudinho — tocar nele custa saúde mental e rende ⭐. É o primeiro pedaço jogável de verdade, construído em cima dos stubs do spec 001.

## Requisitos

- Um paciente (dado fixo pra este spec, só pra provar o ciclo — o `PatientGenerator`/roster completo fica pro spec do ciclo de dia) aparece na sala isométrica como sprite placeholder (sem arte final ainda, ver "Em aberto" em `ARQUITETURA.md`).
- Balão de fala acima do paciente, trocando de emoji ao longo da sessão (estado de humor).
- Balão de pensamento acima do Freudinho (placeholder também), mostrando 💡 quando o paciente muda pra um emoji de sentimento — essa é a janela de insight.
- Tocar no insight: consome saúde mental (via `MentalHealthService`) e rende ⭐ ao paciente (conforme `PatientDefinition`). Deixar passar nunca penaliza.
- `SessionController` implementado como máquina de estados de verdade (hoje é só stub): paciente chega e senta → tick de desgaste/balões → janela de insight (quando aparece) → paciente sai.
- HUD mínimo (Canvas screen-space): número da barra de saúde mental e contador de ⭐ do paciente atual, só pra enxergar o efeito da mecânica — sem estética ainda.
- `MentalHealthService` com lógica real de desgaste passivo e consumo por insight (usando os campos do `BalanceConfig`).
- Teste automatizado (Unity Test Framework, Edit Mode) cobrindo as transições do `SessionController`: chega → insight abre quando paciente muda de humor → insight resolvido → sessão termina. Reforça a testabilidade que `ARQUITETURA.md` definiu como objetivo desde o início.

## Critérios de aceite

- [ ] Apertando Play na `Office.unity`, o paciente placeholder aparece na sala.
- [ ] O balão de fala muda de emoji ao longo do tempo.
- [ ] Em algum momento aparece 💡 no balão do Freudinho; clicar nele consome barra e soma ⭐ (visível no HUD).
- [ ] Deixar o insight passar (não clicar) não penaliza nada.
- [ ] Barra de saúde mental no HUD reflete desgaste passivo + consumo de insight, lida do `BalanceConfig`.
- [ ] Testes Edit Mode do `SessionController` passam (`unity` CLI ou Test Runner do Editor).
- [ ] `git status` limpo após commit.

## Fora de escopo

- Vínculo, dinheiro, turno, ciclo de dia com múltiplos pacientes, lojinha — ficam pros próximos specs.
- Arte final de personagem/sala — placeholder só (sprite colorido simples).
- Pipeline definitivo de emoji (ver decisão em "Abordagem técnica" abaixo — pode precisar de ajuste quando a arte entrar).
- Animação de personagem (entrar andando, sentar) — por agora o paciente só aparece já posicionado.
- Remote Config real — `BalanceConfig` local é a única fonte por enquanto.

## Abordagem técnica

- **Placeholder visual**: sprite quadrado colorido (gerado em runtime via `Texture2D`/`Sprite.Create`, sem depender de asset de arte) representando Freudinho e o paciente, criados em código pelo `SessionRunner` ao entrar em Play. Troca por arte de verdade é tarefa futura, não deste spec.
- **Balões**: implementados com `TextMesh` (texto 3D legado, não TextMeshPro) acima de cada personagem — decisão tomada durante a implementação, não na hora de escrever este spec (ver `tasks.md` desta pasta pro motivo: evita depender do import do TMP Essentials, que só dispara de verdade dentro do Editor interativo). Conteúdo é placeholder de texto (`:)`, `:(`, `[ideia]`), não emoji Unicode de verdade — mesma razão.
- **Clique no insight**: `BoxCollider2D` + `OnMouseDown` (via um `ClickRelay` reutilizável) no balão de pensamento, em vez de Canvas/EventSystem/Button.
- **HUD**: `OnGUI` imediato (não Canvas), só números — reforça que é placeholder, sem estética.
- **`SessionController`**: máquina de estados simples em C# puro (`enum` de estado + método `Tick`), sem `MonoBehaviour` — dirigida por um `MonoBehaviour` fino (`SessionRunner`) na cena que só chama `Tick` a cada frame. Mantém a testabilidade fora do Unity que `ARQUITETURA.md` pede.
- **Assembly definitions**: `Freudinho.Runtime` (Scripts/) e `Freudinho.Tests.EditMode` (Tests/EditMode/), separação explícita de verdade, não monta tudo em `Assembly-CSharp`.
- **Testes**: `Assets/Tests/EditMode/SessionControllerTests.cs`, Unity Test Framework — 6 casos, rodados via `Unity.exe -runTests -testPlatform EditMode`.
