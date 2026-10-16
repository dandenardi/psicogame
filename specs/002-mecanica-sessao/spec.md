# 002 - Mecânica de sessão

Status: concluído

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

- [x] Apertando Play na `Office.unity`, o paciente placeholder aparece na sala.
- [x] O balão de fala muda de placeholder ao longo do tempo (`:)` / `:(`, emoji real adiado — ver "Abordagem técnica").
- [x] Em algum momento aparece `[ideia]` no balão do Freudinho; clicar nele consome barra e soma ⭐ (visível no HUD).
- [x] Deixar o insight passar (não clicar) não penaliza nada.
- [x] Barra de saúde mental no HUD reflete desgaste passivo + consumo de insight, lida do `BalanceConfig`.
- [x] Testes Edit Mode do `SessionController` passam (6/6, `Unity.exe -runTests -testPlatform EditMode`).
- [x] `git status` limpo após commit.

Confirmado tanto por teste ao vivo via `unity-editor-mcp` (play mode dirigido e capturado
remotamente) quanto pelo Daniel na própria Editor.

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

## Bugs reais encontrados no caminho

Nenhum na lógica (`SessionController`/`MentalHealthService` passaram direto nos testes). Todos na integração com a cena, achados só testando de verdade:

1. **Câmera no mesmo Z dos sprites** — `Main Camera` criada em Z=0, igual aos personagens; ficavam dentro do near clip plane, nada renderizava. Fix: câmera em Z=-10.
2. **Referências nulas no `SessionRunner`** — o script de setup atribuiu `patient`/`balance` via `SerializedObject` e salvou sem erro, mas a cena persistiu `{fileID: 0}` pros dois. Causava `NullReferenceException` no primeiro `Awake`. Fix: religado direto pelos GUIDs dos assets.
3. **Sprite de 0,01 unidade** — `Sprite.Create` com 100 pixels-por-unidade numa textura de 1×1 pixel. Sprite tecnicamente existia, só que invisível. Fix: 1 PPU.
4. **HUD ilegível** — `GUI.Label` com estilo padrão renderiza cinza claro sobre o fundo bege da câmera. A lógica sempre funcionou (confirmado via `unity-editor-mcp` antes do fix), mas parecia que clicar no insight "não fazia nada". Fix: texto preto + caixa de fundo.

Pano de fundo: o `.gitignore` tinha um bug de longa data (`Packages/` ignorava a pasta inteira e impedia a exceção de `manifest.json` funcionar — corrigido junto, ver commit do spec), e a ponte `unity-editor-mcp` só ficou disponível a partir da metade da implementação (precisou reiniciar a sessão do Claude Code depois de instalada) — por isso os primeiros bugs foram achados por tentativa e erro com o Daniel testando manualmente, e só os últimos (HUD) com inspeção ao vivo da Editor.
