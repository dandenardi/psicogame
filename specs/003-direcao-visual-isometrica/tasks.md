# Tarefas — 003 Direção visual e câmera isométrica

## Fundação do grid

- [ ] Adicionar `Grid` com `CellLayout: Isometric Z As Y` na `Office.unity`
- [ ] Adicionar `Tilemap` isométrico (ao menos um tile de chão placeholder, cor sólida)
- [ ] Confirmar enquadramento da câmera sobre o grid (sem rotação 3D — ver nota técnica do spec)

## Reposicionar os placeholders existentes

- [ ] Ajustar `SessionRunner` pra posicionar Freudinho/paciente em células do grid isométrico (em vez de coordenadas de mundo fixas)
- [ ] Sombra de contato simples embaixo de cada personagem
- [ ] Conferir que balões (fala/pensamento) continuam ancorados corretamente acima dos personagens na nova posição

## Paleta e referência

- [ ] Registrar a paleta "Aconchegante" em `docs/ARQUITETURA.md` (seção nova)

## Fechamento

- [ ] Rodar manualmente (Play, via MCP e/ou Daniel) e confirmar os critérios de aceite do `spec.md`
- [ ] Rodar os testes Edit Mode da spec 002 e confirmar que continuam passando (sem regressão)
- [ ] Commit
