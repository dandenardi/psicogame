# Jogo do Freudinho: arquitetura técnica

Continuação de [`CONTEXTO.md`](./CONTEXTO.md), registrando as decisões de stack e arquitetura tomadas em 08/10/2026. Os protótipos HTML foram POC de mecânica (balões de sessão) e não têm reaproveitamento visual — a direção de arte mudou para visão isométrica, estilo app de gestão (referência: Game Dev Tycoon).

## Decisões de stack

- **Engine: Unity.** Considerado Godot (mais leve, gratuito, 2D de primeira classe) e descartado em favor do Unity por causa da natureza freemium do projeto: Unity Gaming Services dá remote config, mediação de anúncio e analytics de fábrica — exatamente o que falta pro ajuste de balanceamento contínuo que já está pendente desde o protótipo 3. O custo é C# e uma curva de entrada maior, trade aceito explicitamente pelo Daniel em troca de não esbarrar em limitação técnica mais adiante.
- **Plataformas:** mobile (Android/iOS) + web, prioridade confirmada. Unity exporta para as duas a partir do mesmo projeto.
- **Linguagem:** C#. Daniel vem de web (JS/TS) com alguma experiência em React Native; sem experiência prévia em Unity/C#.

## Decisão de composição de cena

- **Cena única integrada.** Não existe uma tela de "escritório" separada de uma tela de "sessão". O paciente é um sprite que entra na sala isométrica, senta, e os balões de fala/pensamento (mecânica já validada nos protótipos) flutuam sobre ele dentro da própria cena — como em The Sims. Os móveis comprados na lojinha aparecem fisicamente na sala, reforçando visualmente a progressão (escritório começa detonado, vai enchendo conforme o dinheiro entra).
- **Um paciente por vez.** Mantém fiel ao desenho original de vínculo/desgaste (pensado pra atenção de um paciente por vez). Não há sala de espera com múltiplos pacientes visíveis simultaneamente — isso ficaria mais parecido com a referência visual (vários funcionários), mas quebraria a mecânica de vínculo já testada. Pode ser revisitado depois se o jogo pedir mais "vida" na cena.

## Estrutura de cenas Unity

Gameplay inteiro roda em **uma cena persistente** (`Office.unity`), evitando carregamento de cena a cada sessão/dia — tudo é troca de estado, não troca de cena:

```
Scenes/
  Boot.unity      → inicialização, carga de config remoto, save, vai pra Office
  Office.unity    → cena principal: sala isométrica + HUD + overlays (loja, pausa, fim de dia)
```

Dentro de `Office.unity`, o fluxo é orquestrado por máquinas de estado em C# puro (não GameObjects separados por tela):

- **Ciclo do dia:** `Preparo → Sessões (loop) → Intervalo (escolha) → Turno pago → Verificação de barra zerada → Noite/Sono → Próximo dia`
- **Ciclo da sessão:** `Paciente chega e senta → Tick de desgaste/balões → Janela de insight (quando aparece) → Paciente sai (alta, remarcação, desistência ou fim normal)`

## Modelo de dados (ScriptableObjects)

Unity permite modelar dados de jogo como assets (`ScriptableObject`), editáveis no editor sem recompilar — importante pra ajuste de balanceamento:

- **`PatientDefinition`**: nome, sprite/animator, desgaste passivo, faixa de custo de insight, ⭐ por insight, flag de contratransferência (e seu multiplicador), meta de ⭐ pra alta, vínculo inicial.
- **`FurnitureDefinition`**: nome, custo, efeito mecânico (redução de desgaste, bônus de pausa, bônus de ⭐, bônus de sono), prefab visual e posição na grade isométrica.
- **`BalanceConfig`**: todos os números soltos do `CONTEXTO.md` (barra inicial/máxima, recuperação do sono, custo de turno, valor por sessão, limiares de vínculo etc.) centralizados num asset só — ponto único de ajuste fino, e ponto de entrada natural pro Remote Config mais adiante (ver seção própria).

Esses assets ficam em `Assets/Data/` (instâncias) com os scripts que os definem em `Assets/Scripts/Data/`.

## Sistemas (C# puro, desacoplado de MonoBehaviour onde possível)

Pensado assim de propósito: a lógica de jogo (contas de vínculo, economia, desgaste) é testável por fora do Unity, o que ajuda a pegar cedo um problema do tipo "robô que aproveita todo insight trava o jogo em 3 dias" — já observado no protótipo 3.

- **`DayCycleController`** — orquestra o estado do dia (acima).
- **`SessionController`** — orquestra o estado da sessão atual, emite eventos de balão/insight.
- **`EconomyService`** — dinheiro, custo de turno, compras na loja.
- **`BondService`** — cálculo de vínculo (0–5), regras de alta/desistência/remarcação.
- **`MentalHealthService`** — barra de saúde mental: desgaste, recuperação, limiar de distração.
- **`PatientGenerator`** — quem aparece, lista de espera, substituição após alta.
- **`SaveService`** — persistência local (estado do dia, vínculos, dinheiro, móveis comprados).
- **`RemoteConfigService`** — camada fina sobre Unity Remote Config; em dev/offline cai pro `BalanceConfig` local.

Comunicação entre sistemas e apresentação via eventos (C# events ou um event bus simples), não referência direta — assim a UI/animação reage sem acoplar lógica de jogo a objeto de cena.

## Camada de apresentação (isométrico)

- **Grade isométrica**: Tilemap isométrico do Unity (2D Tilemap Editor) pro chão/paredes da sala.
- **Profundidade (Y-sort)**: personagens e móveis ordenados por posição Y na cena a cada frame/movimento — abordagem padrão em jogos 2D isométricos no Unity (sem precisar de 3D).
- **Personagens**: Freudinho + paciente atual, cada um com Animator Controller (estados: idle, sentado, saindo).
- **Balões**: Canvas world-space (ou sprite billboard) ancorado acima da cabeça do personagem, disparado pelos eventos do `SessionController` — é a mesma mecânica validada nos protótipos, só migrando de DOM/CSS pra cena de jogo.
- **HUD**: Canvas screen-space fixo (barra de saúde mental, dinheiro, vínculo, dia/turno) — não depende da câmera isométrica.
- **Loja**: painel de UI modal; comprar um item atualiza o `EconomyService` **e** spawna/upgrade o prefab do móvel na sala — mecânica e visual sempre em sincronia.

## Direção visual (spec 003)

Mockup de referência: [Freudinho — Direções Visuais](https://claude.ai/artifact/JaLr7fcGtkjsvFEmXhg21h), direção "Aconchegante" — pastel, formas orgânicas arredondadas, câmera isométrica de canto fixo (tipo Final Fantasy Tactics).

Paleta:

| Uso | Cor | Hex |
|---|---|---|
| Parede clara (lado da luz) | 🟨 | `#F4E9DC` |
| Parede escura (lado da sombra) | 🟫 | `#D6B990` |
| Chão | 🟧 | `#E3B88F` |
| Freudinho | 🟦 | `#6E97AE` |
| Paciente | 🟠 | `#E8927C` |
| Texto/contorno | ⬛ | `#4A3B32` |

Fonte de referência: Baloo 2 (Google Fonts) — ainda não aplicada (balão continua em `TextMesh` legado, ver spec 002).

**Implementado de verdade (não é só o Tilemap isométrico genérico citado abaixo — isso aqui é o que existe hoje em `Office.unity`):** `Grid` com `CellLayout: Isometric Z As Y`, `Tilemap` de chão pintado via código (`IsometricGrid.cs`, losango por raio de Manhattan a partir do centro), câmera ortográfica comum (sem rotação 3D — o losango vem do grid, não da câmera) reenquadrada pra cobrir a área. Personagens posicionados via `Grid.GetCellCenterWorld`, não coordenada de mundo fixa. "Paredes" como no mockup ainda não existem — ficam pra quando tiver sprite de parede de verdade; hoje é só o chão.

## Estrutura de pastas proposta

```
Assets/
  Art/
    Characters/
    Furniture/
    UI/
  Prefabs/
    Characters/
    Furniture/
    UI/
  Scenes/
    Boot.unity
    Office.unity
  Scripts/
    Core/           → GameStateManager, event bus, interfaces de serviço
    Data/           → definições de ScriptableObject (PatientDefinition, FurnitureDefinition, BalanceConfig)
    Systems/        → EconomyService, BondService, MentalHealthService, SessionController, DayCycleController, PatientGenerator, SaveService, RemoteConfigService
    Presentation/    → CharacterController, YSorter, BubbleUI, HUDController, ShopUI, OfficeFurniturePlacer
  Data/              → instâncias dos ScriptableObjects (Patients/*.asset, Furniture/*.asset, BalanceConfig.asset)
```

## Balanceamento e Remote Config desde o dia 1

Pendência herdada do `CONTEXTO.md`. Estratégia: todo número de jogo vive no `BalanceConfig` (asset local) desde o primeiro protótipo em Unity — nunca hardcoded em script. Quando o jogo estiver publicável, o `RemoteConfigService` passa a sobrescrever esses valores com o que vier do Unity Remote Config, sem mudar uma linha de lógica. Isso permite ajustar desgaste, custo de insight, limiares de vínculo etc. em produção sem nova build — resolvendo o problema de equilíbrio relatado no teste do protótipo 3 (robô que travava aproveitando todo insight).

## Em aberto

- Produção de arte: sprites de personagem (Freudinho + pacientes) e tileset isométrico do escritório — ninguém definiu estilo visual nem fonte dos assets ainda.
- Se/quando revisitar "vários pacientes visíveis ao mesmo tempo" — mecânica de vínculo precisaria ser repensada.
- Integração real de IAP/anúncio (RevenueCat vs. SDK nativo do Unity) — adiado até ter economia jogável pra validar.
- Setup de projeto Unity ainda não criado neste diretório.
