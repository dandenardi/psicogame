using System;
using Freudinho.Data;

namespace Freudinho.Systems
{
    public enum SessionState
    {
        PatientArriving,
        InProgress,
        InsightWindow,
        Ended
    }

    /// <summary>
    /// Máquina de estados de uma sessão: paciente chega, desgaste passivo, uma janela de
    /// insight no meio da sessão, sessão termina. Dirigida por Tick, sem MonoBehaviour —
    /// testável fora do Unity (ver docs/ARQUITETURA.md).
    ///
    /// Simplificações deste spec (002): desgaste passivo aplicado de uma vez só na
    /// chegada (não distribuído ao longo da sessão); só uma janela de insight, no meio
    /// da sessão; custo do insight usa o mínimo da faixa do paciente. Ciclo completo
    /// (múltiplas janelas, humor dinâmico de verdade) fica pra specs futuros.
    /// </summary>
    public class SessionController
    {
        public SessionState State { get; private set; } = SessionState.PatientArriving;
        public int StarsEarned { get; private set; }

        public event Action PatientArrived;
        public event Action<string> MoodChanged;
        public event Action InsightWindowOpened;
        public event Action InsightWindowClosed;
        public event Action InsightResolved;
        public event Action SessionEnded;

        private const string MoodNeutral = ":)";
        private const string MoodFeeling = ":(";

        private readonly PatientDefinition _patient;
        private readonly BalanceConfig _balance;
        private readonly MentalHealthService _mentalHealth;

        private float _elapsed;
        private float _insightWindowElapsed;
        private bool _insightOffered;

        public SessionController(PatientDefinition patient, BalanceConfig balance, MentalHealthService mentalHealth)
        {
            _patient = patient;
            _balance = balance;
            _mentalHealth = mentalHealth;
        }

        public void Tick(float deltaTime)
        {
            if (State == SessionState.Ended)
            {
                return;
            }

            if (State == SessionState.PatientArriving)
            {
                _mentalHealth.ApplyDrain(_patient.desgastePassivo);
                State = SessionState.InProgress;
                PatientArrived?.Invoke();
                MoodChanged?.Invoke(MoodNeutral);
                return;
            }

            _elapsed += deltaTime;

            if (_elapsed >= _balance.duracaoSessaoSegundos)
            {
                if (State == SessionState.InsightWindow)
                {
                    CloseInsightWindow();
                }

                State = SessionState.Ended;
                SessionEnded?.Invoke();
                return;
            }

            if (State == SessionState.InsightWindow)
            {
                _insightWindowElapsed += deltaTime;
                if (_insightWindowElapsed >= _balance.duracaoJanelaInsightSegundos)
                {
                    CloseInsightWindow();
                }

                return;
            }

            TryOpenInsightWindow();
        }

        public void ResolveInsight()
        {
            if (State != SessionState.InsightWindow)
            {
                return;
            }

            _mentalHealth.ConsumeInsight(_patient.custoInsightMin);
            StarsEarned += _patient.estrelasPorInsight;
            InsightResolved?.Invoke();
            CloseInsightWindow();
        }

        private void TryOpenInsightWindow()
        {
            if (_insightOffered || _mentalHealth.IsDistracted)
            {
                return;
            }

            float insightAt = _balance.duracaoSessaoSegundos / 2f;
            if (_elapsed >= insightAt)
            {
                _insightOffered = true;
                _insightWindowElapsed = 0f;
                State = SessionState.InsightWindow;
                MoodChanged?.Invoke(MoodFeeling);
                InsightWindowOpened?.Invoke();
            }
        }

        private void CloseInsightWindow()
        {
            State = SessionState.InProgress;
            MoodChanged?.Invoke(MoodNeutral);
            InsightWindowClosed?.Invoke();
        }
    }
}
