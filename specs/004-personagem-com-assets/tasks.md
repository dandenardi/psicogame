# Tarefas — 004 Personagem de verdade com os assets

## Organização

- [ ] Criar `Assets/Art/Characters/`
- [ ] Mover as peças úteis (`basics_*.png`) pra lá, deixando `avatar.png`/`cover.png`/`logo.png` de fora
- [ ] Apagar `Assets/bases/` depois de mover
- [ ] Configurar import settings (Sprite 2D, pivot consistente, filtro bilinear) nas peças

## Inspeção visual (decisões de conteúdo)

- [ ] Olhar as 5 cores de `top`/`bottom` e escolher as mais próximas de `#6E97AE` (Freudinho) e `#E8927C` (paciente)
- [ ] Confirmar visualmente qual boca é sorriso e qual é neutra/séria (mouth1 a mouth4)
- [ ] Escolher cabelo/olhos/sobrancelha/cílios pra cada personagem (só estética, qualquer combinação razoável)

## Código

- [ ] `LayeredCharacter.cs`: MonoBehaviour com um `SpriteRenderer` por camada, `sortingOrder` crescente, método público pra trocar a boca
- [ ] Atualizar `SessionRunner` pra instanciar `LayeredCharacter` (Freudinho e paciente) em vez do quadrado colorido, mantendo posição isométrica e sombra
- [ ] Ligar `MoodChanged` do `SessionController` na troca de boca do paciente (em vez de alterar o texto do `TextMesh`)
- [ ] Remover o `TextMesh` do balão de fala do paciente (a boca virou a expressão); manter o balão de pensamento do Freudinho como está

## Fechamento

- [ ] Rodar manualmente (Play, via MCP) e confirmar os critérios de aceite do `spec.md`
- [ ] Rodar os testes Edit Mode e confirmar que continuam passando (6/6)
- [ ] Commit
