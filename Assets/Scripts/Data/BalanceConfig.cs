using UnityEngine;

namespace Freudinho.Data
{
    /// <summary>
    /// Todo número de balanceamento do jogo centralizado aqui (ver CONTEXTO.md).
    /// Ponto único de ajuste fino; futuro alvo de sobrescrita por Remote Config
    /// (ver RemoteConfigService e a seção "Balanceamento e Remote Config" em ARQUITETURA.md).
    /// Sem instância populada ainda — spec 001 só define os campos.
    /// </summary>
    [CreateAssetMenu(fileName = "BalanceConfig", menuName = "Freudinho/Config de Balanceamento")]
    public class BalanceConfig : ScriptableObject
    {
        [Header("Barra de saúde mental")]
        public int barraInicial = 80;
        public int barraMaxima = 100;
        public int recuperacaoSono = 55;
        public int recuperacaoSonoComYoga = 70;

        [Header("Dinheiro")]
        public int valorPorSessao = 120;
        public int custoTurno = 150;

        [Header("Vínculo (0 a 5)")]
        public int bonusVinculoDoisMaisInsights = 1;
        public int penalidadeVinculoDistracao = -1;
        public int penalidadeVinculoEmergencia = -2;
        public int penalidadeVinculoRemarcacao = -1;
        public int penalidadeVinculoRemarcacaoDiasSeguidos = -2;
        public int penalidadeVinculoDesmarcacaoBarraZerada = -2;
        public int limiarVinculoAltaBonusEstrela = 4;
        public int limiarVinculoBaixoPenalidadeDesgaste = 1;
        [Tooltip("Ex.: 0.3 para +30%")]
        public float multiplicadorDesgasteVinculoBaixo = 0.3f;

        [Header("Folga avisada")]
        [Tooltip("A cada quantos dias o jogador pode tirar uma folga avisada")]
        public int intervaloDiasFolga = 3;
    }
}
