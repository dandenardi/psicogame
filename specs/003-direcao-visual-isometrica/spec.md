# 003 - Direção visual e câmera isométrica

Status: concluído

## Objetivo

Fixar a direção visual decidida no mockup de design (ver "Referência de design" abaixo) e trazer a câmera/grade isométrica de verdade pra `Office.unity`, substituindo a visão frontal atual (câmera olhando reto pra dois quadrados num fundo liso) pela perspectiva de canto fixo, tipo Final Fantasy Tactics, que o Daniel definiu.

## Referência de design

Mockup aprovado: [Freudinho — Direções Visuais](https://claude.ai/artifact/JaLr7fcGtkjsvFEmXhg21h) — direção "Aconchegante": pastel, formas orgânicas arredondadas, câmera de canto fixa (chão em losango, duas paredes de fundo formando o canto — uma mais clara/lado da luz, outra mais escura/lado da sombra).

Paleta de referência:
- Parede clara (lado da luz): `#F4E9DC`
- Parede escura (lado da sombra): `#D6B990`
- Chão: `#E3B88F`
- Freudinho: `#6E97AE`
- Paciente: `#E8927C`
- Texto/contorno: `#4A3B32`

Fonte de referência: Baloo 2 (Google Fonts) — redonda, amigável. Avaliar se dá pra usar em TextMeshPro mais adiante (fora de escopo deste spec — balão ainda é `TextMesh` legado).

## Requisitos

- Grid/Tilemap da `Office.unity` configurado como isométrico (`Isometric Z As Y`), não retangular.
- Câmera ajustada pra enquadrar a grade isométrica corretamente (ver nota técnica abaixo — a câmera em si não precisa de rotação 3D; o efeito de losango vem do próprio grid).
- Placeholders do `SessionRunner` (Freudinho, paciente, balões) reposicionados pra funcionar nessa grade, sem quebrar a mecânica da spec 002 (sessão, insight, HUD continuam funcionando).
- Paleta documentada num lugar de referência pra specs futuros puxarem dela (ver tarefa em `tasks.md`).
- Sombra de contato simples embaixo de cada personagem (já prototipada no mockup), pra ajudar a vender a profundidade.

## Nota técnica importante

O mockup de design mostra duas "paredes" formando o canto do cômodo — isso dá a impressão de câmera 3D rotacionada. **Não é assim que o Tilemap isométrico da Unity funciona.** O jeito padrão (`Grid.CellLayout = Isometric Z As Y`) mantém a câmera ortográfica normal, reta; o efeito de losango vem de como o grid distribui as células. "Paredes" como no mockup, na prática, são sprites altos (mobília/parede) posicionados nas células de fundo, não geometria 3D de verdade.

Pra este spec: configuramos o grid isométrico de verdade (chão em losango, personagens nas células certas). As duas paredes do mockup — como elemento visual — ficam pra quando tivermos sprites de parede de verdade (fora de escopo: ainda estamos em placeholder). Vale deixar isso explícito pro Daniel validar que está de acordo antes de implementar.

## Critérios de aceite

- [x] Grid da `Office.unity` configurado como `Isometric Z As Y`.
- [x] Apertando Play, o chão aparece em losango (não mais quadrado/retangular).
- [x] Freudinho e paciente aparecem posicionados sobre células do grid isométrico, com sombra de contato.
- [x] Sessão (spec 002) continua funcionando: balões, insight clicável, HUD, estados.
- [x] Paleta registrada em `docs/ARQUITETURA.md` pra reuso futuro.
- [x] Testes Edit Mode da spec 002 continuam passando (6/6, sem regressão).
- [x] `git status` limpo após commit.

Confirmado ao vivo via `unity-editor-mcp` (play mode dirigido e capturado remotamente, ver
`tasks.md` pros bugs achados e corrigidos nessa sessão).

## Fora de escopo

- Sprites de parede de verdade (o efeito "canto fechado" do mockup fica conceitual até termos arte).
- Rotação de câmera pros outros 3 cantos — Daniel confirmou intenção de câmera fixa por agora.
- Arte final de personagem/mobília — placeholder continua.
- Troca de `TextMesh` por TextMeshPro com a fonte Baloo 2 — mecanismo de balão não muda neste spec.

## Abordagem técnica

- Usar o componente `Grid` + `Tilemap` nativos da Unity, com `CellLayout` isométrico, em vez de posicionamento manual por pixel como hoje.
- Implementação feita com a Editor aberta via `unity-editor-mcp` (MCP ao vivo), não mais batch mode — permite inspecionar/corrigir em tempo real em vez do ciclo "edita, pede pro Daniel testar, repete" das specs anteriores.
- Cores centralizadas: avaliar se entram no `BalanceConfig` (não, são visuais, não balanceamento) ou num novo `VisualConfig`/constantes — decidir na implementação.
