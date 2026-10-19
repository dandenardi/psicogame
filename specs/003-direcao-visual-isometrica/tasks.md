# Tarefas — 003 Direção visual e câmera isométrica

## Fundação do grid

- [x] Adicionar `Grid` com `CellLayout: Isometric Z As Y` na `Office.unity`
- [x] Adicionar `Tilemap` isométrico (tile de chão placeholder, cor sólida — na prática virou
      sprite em losango gerado em runtime, ver `IsometricGrid.cs`)
- [x] Confirmar enquadramento da câmera sobre o grid (sem rotação 3D)

## Reposicionar os placeholders existentes

- [x] Ajustar `SessionRunner` pra posicionar Freudinho/paciente em células do grid isométrico
- [x] Sombra de contato simples embaixo de cada personagem
- [x] Conferir que balões continuam ancorados corretamente acima dos personagens

## Paleta e referência

- [x] Registrar a paleta "Aconchegante" em `docs/ARQUITETURA.md`

## Fechamento

- [x] Rodar manualmente (Play, via MCP) e confirmar os critérios de aceite do `spec.md`
- [x] Rodar os testes Edit Mode da spec 002 e confirmar que continuam passando (6/6)
- [x] Commit

## Bugs achados durante a implementação (via MCP ao vivo, não em batch cego)

- [x] `IsometricGrid.CellToWorld` dava `NullReferenceException`: ordem de `Awake()` entre
      GameObjects não é garantida — `SessionRunner` podia rodar antes do `IsometricGrid`
      cachear o `Grid`. Fix: propriedade lazy em vez de cache no `Awake`.
- [x] Tile de chão era um quadrado liso, não losango — grid isométrico não desenha diamante
      sozinho, o *sprite do tile* precisa ser desenhado em forma de losango. Fix: textura
      gerada em runtime com máscara de diamante.
- [x] Tamanho do sprite do tile não batia com `cellSize` do Grid — ladrilhos ficavam com vão
      entre eles. Fix: `pixelsPerUnit` calculado a partir do `cellSize.x` real, não fixo.
- [x] Centralização de célula pra posição de mundo feita na mão (`CellToWorld + cellSize/2`)
      dava posição assimétrica em grid isométrico (só funciona em grid retangular). Fix:
      `Grid.GetCellCenterWorld`, método nativo que lida com qualquer `CellLayout`.
- [x] Freudinho e paciente nas células `(-1,0)`/`(1,0)` ficavam em profundidades (Y) diferentes
      — colunas diferentes deslocam Y em isométrico. Fix: células `(-1,1)`/`(1,-1)`, mesma
      soma `col+row`, mesma profundidade aparente.
- [x] Sombra de contato invisível: ficava quase toda atrás do próprio sprite do personagem
      (sorting order do personagem maior, sobrepondo). Fix: sombra empurrada mais pra baixo
      do centro do personagem, saindo de debaixo do quadrado.
