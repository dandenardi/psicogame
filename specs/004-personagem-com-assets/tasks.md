# Tarefas — 004 Personagem de verdade com os assets

## Organização

- [x] Criar `Assets/Art/Characters/`
- [x] Mover as peças úteis (`basics_*.png`) pra lá, deixando `avatar.png`/`cover.png`/`logo.png` de fora
- [x] Apagar `Assets/bases/` depois de mover
- [x] Configurar import settings (Sprite 2D, pivot consistente, filtro bilinear) nas peças

## Inspeção visual (decisões de conteúdo)

- [x] Olhar as 5 cores de `top`/`bottom` e escolher as mais próximas de `#6E97AE` (Freudinho) e `#E8927C` (paciente)
      — `top3`/`bottom3` (azul-acinzentado) pro Freudinho, `top2`/`bottom2` (vermelho-tijolo) pro paciente
- [x] Confirmar visualmente qual boca é sorriso e qual é neutra/séria — `mouth3` = sorriso (humor neutro/default),
      `mouth1` = reto/sério (humor "sentindo", janela de insight)
- [x] Escolher cabelo/olhos/sobrancelha/cílios — `hairA_3`/`hairA_2`, `eyebrow1`/`eyes1`/`eyelashes1` pros dois

## Código

- [x] `LayeredCharacter.cs`: MonoBehaviour com um `SpriteRenderer` por camada, `sortingOrder` crescente, método público pra trocar a boca
- [x] `CharacterOutfitAsset.cs`: ScriptableObject agrupando as 10 referências de sprite por personagem (em vez de 18 campos soltos no `SessionRunner`)
- [x] Atualizar `SessionRunner` pra instanciar `LayeredCharacter` (Freudinho e paciente) em vez do quadrado colorido, mantendo posição isométrica e sombra
- [x] Ligar `MoodChanged` do `SessionController` na troca de boca do paciente — evento mudou de `Action<string>` (texto do balão) pra `Action<bool>` (`true` = sentindo), já que a apresentação não precisa mais de string de display
- [x] Remover o `TextMesh` do balão de fala do paciente; manter o balão de pensamento do Freudinho como está

## Fechamento

- [x] Rodar manualmente (Play, via MCP) e confirmar os critérios de aceite do `spec.md`
- [x] Rodar os testes Edit Mode e confirmar que continuam passando (6/6)
- [x] Commit

## Bugs achados durante a implementação

- [x] **Pivot ignorado**: script de import settings setava `spritePivot` mas não `spriteAlignment = Custom` —
      sem isso a Unity ignora o pivot customizado e usa centro. Resultado: personagens posicionados pelo
      centro do sprite, não pelo pé; sombra de contato ficava escondida atrás do corpo (centralizada nele,
      não nos pés). Fix: `TextureImporterSettings.spriteAlignment = Custom` + `spritePivot`, via
      `ReadTextureSettings`/`SetTextureSettings` (a API direta `importer.spriteAlignment` não existe nessa
      versão da Unity).
- [x] Chão (Tilemap) competindo em sortingOrder com os personagens — mesmo problema de profundidade do
      spec 003, reapareceu porque a sombra agora é um objeto separado com order próprio. Fix: chão fixado
      em `sortingOrder -10`, bem atrás de tudo, em vez de ficar ajustando a ordem de cada objeto nessa
      camada toda vez que algo novo entra na cena.

## Nota sobre verificação ao vivo

Confirmei a troca de boca por inspeção de código/campo (evento ligado corretamente, sprites resolvem pro
asset certo), mas não peguei o frame exato da janela de insight numa captura de tela ao vivo — a sessão
roda mais rápido que o intervalo de poll nesta automação (mesmo comportamento documentado no spec 002).
Baixo risco: a lógica em si (`SessionController`) já tem teste automatizado cobrindo quando o evento
dispara; o que não foi visto ao vivo é só o resultado visual do sprite trocado.

## Pendência de memória (contexto, não é tarefa do spec)

Boa parte desta sessão rodou sob pressão de memória real na máquina do Daniel (um pente de RAM queimado).
Isso causou desconexões intermitentes do `unity-editor-mcp` e, num momento, falha do próprio PowerShell
por esgotamento de working set. Evitei rodar instância batch da Unity em paralelo com a Editor aberta
depois que isso ficou claro, preferindo esperar a reconexão do MCP e editar arquivo direto quando possível
(os dois `.asset` de figurino foram escritos à mão, não gerados pela Editor).
