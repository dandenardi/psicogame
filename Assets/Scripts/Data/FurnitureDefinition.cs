using UnityEngine;

namespace Freudinho.Data
{
    /// <summary>
    /// Item compravel na lojinha (ver CONTEXTO.md) e sua presença física na sala isométrica.
    /// Sem instância populada ainda — spec 001 só define os campos.
    /// </summary>
    [CreateAssetMenu(fileName = "NovoMovel", menuName = "Freudinho/Móvel")]
    public class FurnitureDefinition : ScriptableObject
    {
        [Header("Identidade")]
        public string displayName;
        public int custo;
        public GameObject prefabVisual;

        [Header("Efeito mecânico")]
        [Tooltip("Negativo reduz desgaste, ex.: poltrona")]
        public int bonusReducaoDesgaste;
        [Tooltip("Bônus de barra por pausa, ex.: planta")]
        public int bonusBarraPorPausa;
        [Tooltip("Bônus de ⭐ por insight, ex.: curso de escuta")]
        public int bonusEstrelaPorInsight;
        [Tooltip("Bônus de recuperação por noite, ex.: yoga")]
        public int bonusRecuperacaoNoturna;
    }
}
