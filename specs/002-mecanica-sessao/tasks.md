# Tarefas — 002 Mecânica de sessão

## Dados

- [x] Popular `BalanceConfig.asset` com os números de `CONTEXTO.md` (barra inicial 80, máxima 100, etc.)
- [x] Criar `PatientDefinition.asset` de um paciente só (Dona Lurdes: desgaste 9, custo insight 5-7, 1 ⭐) pra provar o ciclo

## Lógica (C# puro, testável)

- [x] Implementar `MentalHealthService`: barra atual, desgaste passivo, consumir insight, limiar de distração
- [x] Implementar `SessionController` como máquina de estados (chega → tick → janela de insight → sai), emitindo eventos
- [x] Testes Edit Mode (`Assets/Tests/EditMode/`) cobrindo as transições do `SessionController` — 6/6 passando

## Cena e apresentação

- [x] Sprite placeholder do Freudinho (quadrado colorido) na `Office.unity`
- [x] Sprite placeholder do paciente (quadrado colorido, cor diferente)
- [x] Balão de fala acima do paciente, trocando de conteúdo ao longo da sessão
- [x] Balão de pensamento acima do Freudinho, mostrando o placeholder de insight na janela
- [x] Input: clicar no balão de insight dispara consumo de barra + ganho de ⭐
- [x] HUD: número da barra e contador de ⭐
- [x] `SessionRunner` (MonoBehaviour fino) na cena, liga `SessionController` + `MentalHealthService` ao loop do Unity

**Desvio da abordagem técnica original** (documentado aqui, não no `spec.md`, pra manter o
registro de decisão perto da execução): troquei Canvas World Space + TextMeshPro por
`TextMesh` legado pros balões, e Canvas screen-space por `OnGUI` pro HUD. Motivo: evita
a dependência de import do TMP Essentials (gatilho de import que só dispara de verdade
dentro do Editor interativo) e evita montar hierarquia de Canvas/EventSystem/Button às
cegas via script batch, sem conseguir verificar visualmente o resultado. `OnMouseDown` +
`BoxCollider2D` substituiu o Button. Tudo trocável depois sem mexer na lógica (`SessionController`/
`MentalHealthService` não sabem nada sobre isso). Emoji Unicode também não foi tentado —
fui direto pro placeholder de texto (`:)`, `:(`, `[ideia]`) pelo mesmo motivo de não
conseguir verificar renderização de fonte remotamente.

## Fechamento

- [ ] Rodar manualmente (Play) e confirmar os critérios de aceite do `spec.md` — **pendente, precisa do Daniel**
- [x] Rodar os testes Edit Mode e confirmar que passam
- [ ] Commit
