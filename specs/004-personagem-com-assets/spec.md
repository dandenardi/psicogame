# 004 - Personagem de verdade com os assets "Build a Character ~Basics"

Status: concluído

## Objetivo

Trocar os quadrados coloridos placeholder de Freudinho e do paciente (spec 002) pelos
personagens de verdade, montados a partir dos assets de boneco em camadas ("paper doll")
baixados pelo Daniel em `Assets/bases/` (pack "Build a Character ~Basics", de
MaelleMarylloup — ver recomendação e licença registradas no chat; licença: uso pessoal e
comercial permitido, não pode revender os assets soltos, sem crédito obrigatório).

## O que veio no pack

Cada peça é um PNG separado (444×700px, RGBA), pensado pra empilhar por cima um do outro
formando um personagem só. Confirmado em `Assets/bases/`:

- `basics_body` — base do corpo, 5 variações de cor + versão BW (preto/branco, pra recolorir).
- `basics_bottom`, `basics_bottomB`, `basics_bottomC` — 3 estilos de roupa de baixo, 5 cores cada.
- `basics_top`, `basics_topB`, `basics_topC` — 3 estilos de roupa de cima, 5 cores cada.
- `basics_shoes` — sapato, 5 cores.
- `basics_hairA` a `basics_hairE` — 5 estilos de cabelo, a maioria em 5 cores.
- `basics_eyes`, `basics_eyebrow`, `basics_eyelashes` — 5 variações cada.
- `basics_mouth1` a `basics_mouth4` — 4 bocas (sem variação de cor). Inspecionadas visualmente:
  `mouth3` é um sorriso claro (curva pra cima), `mouth1` é neutro/reto, `mouth2` é uma boca
  pequena aberta, `mouth4` tem um detalhe extra (possível careta/triste) — a avaliar de perto
  na implementação.
- `avatar.png`, `cover.png`, `logo.png` — material de divulgação da loja do itch.io, não são
  asset de jogo. Não entram no projeto.
- **Não tem**: poses/direções, animação, nem expressão "feliz/triste" pronta e rotulada — só
  as peças soltas pra montar.

## Requisitos

- Assets organizados em `Assets/Art/Characters/` (não soltos em `Assets/bases/`, que não segue
  a estrutura definida em `ARQUITETURA.md`), com import settings corretos (Sprite 2D, pivot
  consistente entre todas as peças pra empilhar certo, filtro bilinear — não é pixel art).
- Componente `LayeredCharacter` reutilizável: um `GameObject` com um `SpriteRenderer` por
  camada (corpo, baixo, cima, sapato, cabelo, olhos, sobrancelha, cílios, boca), cada um com
  `sortingOrder` crescente na ordem certa de empilhamento.
- Duas aparências fixas definidas: Freudinho e o paciente, usando cores das peças que
  aproximam a paleta já estabelecida (`#6E97AE` pro Freudinho, `#E8927C` pro paciente) — sem
  sistema de customização pro jogador, é só trocar o quadrado placeholder por algo fixo.
- Boca do paciente trocável em runtime — liga no `MoodChanged` do `SessionController` que já
  existe (hoje troca o texto `:)`/`:(` do `TextMesh`): mapear `MoodNeutral` → boca de sorriso,
  `MoodFeeling` → boca neutra/séria. O balão de fala (`TextMesh`) é removido já que a expressão
  passa a estar na própria boca do personagem — mas o balão de pensamento do Freudinho
  (`[ideia]`) continua como está, não muda neste spec.
- `SessionRunner` atualizado pra instanciar `LayeredCharacter` em vez do quadrado colorido,
  mantendo a posição isométrica (spec 003) e a sombra de contato.
- Sessão (spec 002) continua funcionando sem regressão: insight clicável, HUD, estados, testes
  Edit Mode.

## Fora de escopo

- Sistema de customização do jogador escolher roupa/cabelo — aparência fixa só.
- Animação/pose (o pack não tem; personagem continua estático, parado na célula).
- Trocar a boca do Freudinho (só o paciente muda de humor, por ora).
- Mobília/ambiente com assets de verdade — só personagem neste spec (o pack de ambiente
  "Living room Classic" ainda não foi baixado).
- Registro formal da licença num arquivo dedicado do projeto — mencionar no commit já deixa
  rastro suficiente por agora; reavaliar se mais assets de fontes diferentes entrarem depois.

## Critérios de aceite

- [x] Assets movidos/reorganizados pra `Assets/Art/Characters/`, import settings configurados.
- [x] `avatar.png`, `cover.png`, `logo.png` removidos do projeto (não são asset de jogo).
- [x] Apertando Play, Freudinho e o paciente aparecem como personagens montados em camadas, não
      mais quadrados coloridos — posicionados certo na grade isométrica, com sombra de contato.
- [x] Boca do paciente muda visualmente quando o humor muda (ligado via evento; checado por
      inspeção de campo/sprite, não pego no frame exato ao vivo — ver nota em `tasks.md`).
- [x] Mecânica da sessão (spec 002) sem regressão: clique no insight, HUD, estados.
- [x] Testes Edit Mode continuam passando (6/6).
- [x] `git status` limpo após commit — `Assets/bases/` não deve sobrar na raiz.

## Abordagem técnica

- `LayeredCharacter` como `MonoBehaviour` com uma lista/campos de `SpriteRenderer` por slot,
  e um método público (`SetMouth(Sprite)` ou similar) pro `SessionRunner` trocar a boca do
  paciente reagindo ao `MoodChanged` — mesma ideia do `SessionController`/`MentalHealthService`
  desacoplados de `MonoBehaviour`: a lógica de humor não sabe nada sobre sprite, só dispara o
  evento; quem decide o mapeamento humor→sprite é a camada de apresentação.
- Monta por `SpriteRenderer` empilhado (não by "bake" numa textura única): mais simples, e
  precisamos trocar peça individual (boca) em runtime sem reconstruir o personagem inteiro.
- Implementação com a Editor aberta via `unity-editor-mcp`, inspecionando visualmente as peças
  (`Read` do Claude Code lê imagem) pra escolher cor/boca certas antes de aplicar.
