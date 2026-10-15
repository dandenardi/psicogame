using UnityEngine;
using Freudinho.Data;
using Freudinho.Systems;

namespace Freudinho.Presentation
{
    /// <summary>
    /// MonoBehaviour fino que liga SessionController/MentalHealthService ao loop do
    /// Unity e monta a apresentação da sessão. Tudo aqui é placeholder (spec 002):
    /// quadrados coloridos no lugar de sprite de personagem, TextMesh legado no lugar
    /// de emoji de verdade (ver "Abordagem técnica" em specs/002-mecanica-sessao/spec.md).
    /// </summary>
    public class SessionRunner : MonoBehaviour
    {
        [SerializeField] private PatientDefinition patient;
        [SerializeField] private BalanceConfig balance;

        private MentalHealthService _mentalHealth;
        private SessionController _session;
        private TextMesh _speechBubble;
        private TextMesh _thoughtBubble;

        private const string InsightLabel = "[ideia]";

        private void Awake()
        {
            _mentalHealth = new MentalHealthService(balance);
            _session = new SessionController(patient, balance, _mentalHealth);

            _session.MoodChanged += mood => _speechBubble.text = mood;
            _session.InsightWindowOpened += OnInsightWindowOpened;
            _session.InsightWindowClosed += OnInsightWindowClosed;

            var freudinhoGO = BuildCharacter("Freudinho", new Vector3(-2f, 0f, 0f), new Color(0.45f, 0.62f, 0.9f));
            var thoughtBubbleGO = CreateBubble(freudinhoGO.transform, Vector3.up * 1.2f, out _thoughtBubble);
            var insightCollider = thoughtBubbleGO.AddComponent<BoxCollider2D>();
            insightCollider.size = new Vector2(1.8f, 0.7f);
            insightCollider.enabled = false;
            thoughtBubbleGO.AddComponent<ClickRelay>().Clicked += () => _session.ResolveInsight();

            var patientGO = BuildCharacter("Paciente", new Vector3(2f, 0f, 0f), new Color(0.95f, 0.6f, 0.3f));
            CreateBubble(patientGO.transform, Vector3.up * 1.2f, out _speechBubble);
        }

        private void Update()
        {
            _session.Tick(Time.deltaTime);
        }

        private void OnInsightWindowOpened()
        {
            _thoughtBubble.text = InsightLabel;
            SetInsightColliderEnabled(true);
        }

        private void OnInsightWindowClosed()
        {
            _thoughtBubble.text = string.Empty;
            SetInsightColliderEnabled(false);
        }

        private void SetInsightColliderEnabled(bool value)
        {
            if (_thoughtBubble.TryGetComponent<BoxCollider2D>(out var collider))
            {
                collider.enabled = value;
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 320, 24), $"Saúde mental: {_mentalHealth.Current}/{_mentalHealth.Max}");
            GUI.Label(new Rect(10, 34, 320, 24), $"Estrelas: {_session.StarsEarned}");
            GUI.Label(new Rect(10, 58, 320, 24), $"Estado da sessão: {_session.State}");
        }

        private static GameObject BuildCharacter(string name, Vector3 position, Color color)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.localScale = new Vector3(1.5f, 1.5f, 1f);

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = color;

            return go;
        }

        private static GameObject CreateBubble(Transform parent, Vector3 localOffset, out TextMesh textMesh)
        {
            var go = new GameObject("Bubble");
            go.transform.SetParent(parent);
            go.transform.localPosition = localOffset;

            textMesh = go.AddComponent<TextMesh>();
            textMesh.characterSize = 0.2f;
            textMesh.fontSize = 48;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.color = Color.black;
            textMesh.text = string.Empty;

            return go;
        }
    }
}
