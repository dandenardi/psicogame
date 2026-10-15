using UnityEngine;
using Freudinho.Data;

namespace Freudinho.Systems
{
    /// <summary>
    /// Barra de saúde mental do Freudinho: desgaste, consumo por insight e limiar de
    /// distração (CONTEXTO.md: abaixo da metade, o Freudinho se distrai e perde insights).
    /// </summary>
    public class MentalHealthService
    {
        public int Current { get; private set; }
        public int Max { get; }

        public bool IsDistracted => Current < Max / 2;

        public MentalHealthService(BalanceConfig balance)
        {
            Max = balance.barraMaxima;
            Current = balance.barraInicial;
        }

        public void ApplyDrain(int amount)
        {
            Current = Mathf.Clamp(Current - amount, 0, Max);
        }

        public void ConsumeInsight(int cost)
        {
            ApplyDrain(cost);
        }
    }
}
