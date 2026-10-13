using UnityEngine;

namespace Freudinho.Data
{
    /// <summary>
    /// Dados fixos de um tipo de paciente (ver CONTEXTO.md).
    /// Sem instância populada ainda — spec 001 só define os campos.
    /// </summary>
    [CreateAssetMenu(fileName = "NovoPaciente", menuName = "Freudinho/Paciente")]
    public class PatientDefinition : ScriptableObject
    {
        [Header("Identidade")]
        public string displayName;
        public Sprite portrait;
        public RuntimeAnimatorController animatorController;

        [Header("Desgaste e insight")]
        public int desgastePassivo;
        public int custoInsightMin;
        public int custoInsightMax;
        public int estrelasPorInsight = 1;

        [Header("Contratransferência")]
        public bool temContratransferencia;
        [Tooltip("Multiplicador de desgaste quando a contratransferência está ativa (ex.: 1.5)")]
        public float multiplicadorDesgasteContratransferencia = 1f;
        public int bonusInsightContratransferencia;

        [Header("Vínculo e alta")]
        public int vinculoInicial;
        public int metaEstrelasParaAlta;
    }
}
