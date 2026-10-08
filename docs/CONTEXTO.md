# Jogo do Freudinho: contexto do projeto

Resumo da conversa de design de 08/10/2026, para continuar o trabalho em outro ambiente. Os três protótipos citados estão nesta pasta como HTML autônomo (abrem direto no navegador, sem build).

## A ideia

Jogo de simulação em que o jogador é um psicoterapeuta recém-formado, o "Freudinho" (personalização do personagem fica para depois). Ele monta o consultório, atende, ganha dinheiro e melhora. O diferencial é que o jogador precisa cuidar da própria saúde mental: uma barra que os atendimentos drenam e que se recupera com tempo, autocuidado e terapia.

Formato pretendido: jogo mobile freemium, tom descontraído.

## Decisões tomadas

- **Tom leve, sem conteúdo clínico.** A sessão não tem falas. O paciente "fala" em balões de emoji, para o jogo não pesar nem dar gatilho em histórias parecidas com a vida do jogador. Um primeiro protótipo com transcrição de sessão foi descartado por ficar denso e difícil até para psicólogo.
- **Dois balões.** Balão de fala do paciente e balão de pensamento do Freudinho. A oportunidade de intervenção aparece no balão de pensamento, como um insight (💡), quando o emoji do paciente muda para um sentimento.
- **Uma ação só.** Tocar no insight. Aproveitar custa saúde mental e rende ⭐ ao paciente. Deixar passar nunca penaliza.
- **Cansaço visível.** Abaixo da metade da barra o Freudinho se distrai (🍕, 🏖️) e perde insights.
- **Sem "corte" como intervenção.** Encerrar a sessão por iniciativa do terapeuta não é realista. Virou botão de pânico lúdico: "inventar uma emergência", uma vez por dia, paciente sai irritado e não paga.
- **A decisão interessante está no dia, não na sessão.** Gastar a barra agora ou guardar para um paciente mais pesado depois. No teste do protótipo 2 isso funcionou: o jogador deixou insights passarem de propósito.
- **Dinheiro como chamariz e como pressão.** Sem atender não há dinheiro para melhorar o consultório e a si mesmo. Custo de sala por turno (sublocação), em vez de aluguel mensal, para não punir quem ficou dias sem abrir o jogo.
- **Vínculo com o paciente faz parte da mecânica.** Ficar sem atender afeta o vínculo e pode levar à perda do paciente, como na vida real. A perda é gradual e avisada.
- **Monetização (direção, não fechada).** Não vender recarga da barra nem proteção de vínculo. Vender o que não mexe nelas: decoração, personalização, casos especiais; anúncio opcional.

## Protótipos

| Arquivo | O que testa | Situação |
|---|---|---|
| `prototipo-1-sessao.html` | Uma sessão com transcrição e quatro intervenções | Descartado (denso demais) |
| `prototipo-2-dia.html` | Um dia com cinco pacientes em balões | Aprovado como ponto de partida |
| `prototipo-3-tres-dias.html` | Três dias com vínculo, remarcação, turno e lojinha | Ainda não avaliado pelo Daniel |

## Regras e números do protótipo 3

Todos os números são estimativa inicial, sem ajuste fino.

- **Barra:** começa em 80, máximo 100. Dormir recupera 55 (70 com yoga).
- **Sessão:** cerca de 20 s. Desgaste passivo por paciente: Dona Lurdes 9, Caio 15, Seu Armando 22, Bia 18, Téo 13.
- **Insight:** custo 5 a 9 conforme o paciente; rende 1 ⭐ (2 nos pacientes pesados).
- **Contratransferência:** Bia "mexe com você": desgaste ×1,5 e insight +2, a menos que o Freudinho vá à própria análise no almoço.
- **Vínculo (0 a 5):** sessão com 2+ insights dá +1; distração na sessão −1; emergência −2; remarcar −1 (−2 se for em dias seguidos); desmarcar em cima da hora por barra zerada −2. Vínculo 4+ dá +1 ⭐ por insight; vínculo 0 ou 1 aumenta o desgaste em 30%. Em 0 o paciente fica "por um fio" e só desiste no tropeço seguinte.
- **Alta:** vínculo 5 e ⭐ acumuladas até a meta do paciente (8 a 14). Rende R$ 200 e abre vaga para a lista de espera.
- **Dinheiro:** R$ 120 por sessão, turno de R$ 150 por dia cobrado à noite.
- **Intervalos:** uma escolha entre três (café, respirar, relatórios, almoço, análise, encaixe, caminhada, supervisão), trocando dinheiro por barra ou o contrário.
- **Lojinha:** poltrona (R$ 250, −20% de desgaste), planta (R$ 120, +4 por pausa), curso de escuta (R$ 300, +1 ⭐ por insight), yoga (R$ 180, +15 por noite).
- **Folga avisada:** uma vez em três dias; vínculo congelado, barra cheia, sem receita nem turno.
- **Barra zerada:** o dia acaba e os pacientes restantes são desmarcados em cima da hora.

## Em aberto

- Equilíbrio do protótipo 3. Um robô que aproveitava todo insight travou nos três dias; falta o teste de uma pessoa jogando com cuidado.
- Se a contratransferência combina com o tom leve.
- Como "sessão opcional" convive com agenda e vínculo no formato freemium (tempo real, energia, férias avisadas).
- Risco de bola de neve: melhoria facilita ganhar dinheiro, que compra mais melhoria.
- Geração de pacientes e arcos, personalização do personagem, tecnologia do jogo final.
