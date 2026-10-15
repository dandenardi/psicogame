using NUnit.Framework;
using UnityEngine;
using Freudinho.Data;
using Freudinho.Systems;

namespace Freudinho.Tests.EditMode
{
    public class SessionControllerTests
    {
        private BalanceConfig _balance;
        private PatientDefinition _patient;
        private MentalHealthService _mentalHealth;
        private SessionController _session;

        [SetUp]
        public void SetUp()
        {
            _balance = ScriptableObject.CreateInstance<BalanceConfig>();
            _balance.barraInicial = 80;
            _balance.barraMaxima = 100;
            _balance.duracaoSessaoSegundos = 20f;
            _balance.duracaoJanelaInsightSegundos = 4f;

            _patient = ScriptableObject.CreateInstance<PatientDefinition>();
            _patient.displayName = "Paciente de teste";
            _patient.desgastePassivo = 9;
            _patient.custoInsightMin = 5;
            _patient.custoInsightMax = 9;
            _patient.estrelasPorInsight = 1;

            _mentalHealth = new MentalHealthService(_balance);
            _session = new SessionController(_patient, _balance, _mentalHealth);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_balance);
            Object.DestroyImmediate(_patient);
        }

        [Test]
        public void PrimeiroTick_PacienteChega_AplicaDesgastePassivo()
        {
            _session.Tick(0f);

            Assert.AreEqual(SessionState.InProgress, _session.State);
            Assert.AreEqual(71, _mentalHealth.Current); // 80 - 9
        }

        [Test]
        public void NoMeioDaSessao_AbreJanelaDeInsight()
        {
            _session.Tick(0f);
            _session.Tick(10f); // metade de 20s

            Assert.AreEqual(SessionState.InsightWindow, _session.State);
        }

        [Test]
        public void ResolverInsight_ConsomeBarraERendeEstrela()
        {
            _session.Tick(0f);
            _session.Tick(10f);
            int barraAntes = _mentalHealth.Current;

            _session.ResolveInsight();

            Assert.AreEqual(barraAntes - _patient.custoInsightMin, _mentalHealth.Current);
            Assert.AreEqual(1, _session.StarsEarned);
            Assert.AreEqual(SessionState.InProgress, _session.State);
        }

        [Test]
        public void DeixarPassar_FechaJanelaSemPenalidade()
        {
            _session.Tick(0f);
            _session.Tick(10f); // abre janela
            int barraAntes = _mentalHealth.Current;

            _session.Tick(5f); // passa dos 4s de janela sem clicar

            Assert.AreEqual(SessionState.InProgress, _session.State);
            Assert.AreEqual(barraAntes, _mentalHealth.Current);
            Assert.AreEqual(0, _session.StarsEarned);
        }

        [Test]
        public void FimDaSessao_DisparaSessionEnded()
        {
            bool ended = false;
            _session.SessionEnded += () => ended = true;

            _session.Tick(0f);
            _session.Tick(25f); // além dos 20s

            Assert.IsTrue(ended);
            Assert.AreEqual(SessionState.Ended, _session.State);
        }

        [Test]
        public void Distraido_NaoAbreJanelaDeInsight()
        {
            // drena a barra abaixo da metade antes da sessão começar
            for (int i = 0; i < 10; i++)
            {
                _mentalHealth.ApplyDrain(10);
            }

            _session.Tick(0f);
            _session.Tick(10f); // chegaria na metade da sessão

            Assert.AreEqual(SessionState.InProgress, _session.State);
        }
    }
}
