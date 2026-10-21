using UnityEngine;
using Freudinho.Data;
using Freudinho.Systems;

namespace Freudinho.Presentation
{
    /// <summary>
    /// MonoBehaviour fino que liga SessionController/MentalHealthService ao loop do
    /// Unity e monta a apresentação da sessão. Personagens de verdade desde o spec 004
    /// (pack "Build a Character ~Basics"); balão de pensamento do Freudinho ainda é
    /// TextMesh legado (ver specs/002 e specs/004).
    /// </summary>
    public class SessionRunner : MonoBehaviour
    {
        [SerializeField] private PatientDefinition patient;
        [SerializeField] private BalanceConfig balance;
        [SerializeField] private IsometricGrid isometricGrid;
        [SerializeField] private CharacterOutfitAsset freudinhoOutfit;
        [SerializeField] private CharacterOutfitAsset pacienteOutfit;

        private MentalHealthService _mentalHealth;
        private SessionController _session;
        private CharacterOutfitAsset _pacienteOutfitRef;
        private LayeredCharacter _pacienteCharacter;
        private TextMesh _thoughtBubble;

        private const string InsightLabel = "[ideia]";

        private void Awake()
        {
            _mentalHealth = new MentalHealthService(balance);
            _session = new SessionController(patient, balance, _mentalHealth);

            _pacienteOutfitRef = pacienteOutfit;
            _session.MoodChanged += feeling => _pacienteCharacter.SetMouth(_pacienteOutfitRef.ToOutfit(feeling).Mouth);
            _session.InsightWindowOpened += OnInsightWindowOpened;
            _session.InsightWindowClosed += OnInsightWindowClosed;

            var freudinhoPos = isometricGrid.CellToWorld(-1, 1);
            var freudinhoGO = BuildLayeredCharacter("Freudinho", freudinhoPos, freudinhoOutfit);
            AddContactShadow(freudinhoGO.transform);
            var thoughtBubbleGO = CreateBubble(freudinhoGO.transform, Vector3.up * 1.9f, out _thoughtBubble);
            var insightCollider = thoughtBubbleGO.AddComponent<BoxCollider2D>();
            insightCollider.size = new Vector2(1.8f, 0.7f);
            insightCollider.enabled = false;
            thoughtBubbleGO.AddComponent<ClickRelay>().Clicked += () => _session.ResolveInsight();

            var patientPos = isometricGrid.CellToWorld(1, -1);
            var patientGO = BuildLayeredCharacter("Paciente", patientPos, pacienteOutfit);
            AddContactShadow(patientGO.transform);
            _pacienteCharacter = patientGO.GetComponent<LayeredCharacter>();
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

        private static GUIStyle _hudStyle;

        private void OnGUI()
        {
            // Estilo padrão do OnGUI é cinza claro sobre o fundo bege da câmera — quase
            // invisível. Texto preto, maior, com fundo opaco por trás de cada linha.
            if (_hudStyle == null)
            {
                _hudStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 18,
                    normal = { textColor = Color.black }
                };
            }

            DrawHudLine(10, $"Saúde mental: {_mentalHealth.Current}/{_mentalHealth.Max}");
            DrawHudLine(34, $"Estrelas: {_session.StarsEarned}");
            DrawHudLine(58, $"Estado da sessão: {_session.State}");
        }

        private static void DrawHudLine(int y, string text)
        {
            var rect = new Rect(10, y, 340, 24);
            GUI.Box(rect, GUIContent.none);
            GUI.Label(rect, text, _hudStyle);
        }

        private static GameObject BuildLayeredCharacter(string name, Vector3 position, CharacterOutfitAsset outfit)
        {
            var go = new GameObject(name);
            go.transform.position = position;

            var character = go.AddComponent<LayeredCharacter>();
            character.Build(outfit.ToOutfit(feeling: false));

            return go;
        }

        private static void AddContactShadow(Transform parent)
        {
            var go = new GameObject("Shadow");
            go.transform.SetParent(parent);
            go.transform.localPosition = new Vector3(0f, 0f, 0.01f);
            go.transform.localScale = new Vector3(0.7f, 0.22f, 1f);

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.color = new Color(0.29f, 0.23f, 0.2f, 0.4f);
            renderer.sortingOrder = -1;
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

            var renderer = go.GetComponent<MeshRenderer>();
            renderer.sortingOrder = 20;

            return go;
        }
    }
}
