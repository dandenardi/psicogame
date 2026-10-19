using UnityEngine;
using UnityEngine.Tilemaps;

namespace Freudinho.Presentation
{
    /// <summary>
    /// Grid isométrico (Isometric Z As Y) da sala. Pinta um chão placeholder em losango
    /// (células dentro de um raio de Manhattan) e converte célula em posição de mundo
    /// pra outros scripts (SessionRunner) posicionarem personagens sobre o grid.
    /// Cor sólida é placeholder — arte final troca isso depois (ver spec 003).
    /// </summary>
    [RequireComponent(typeof(Grid))]
    public class IsometricGrid : MonoBehaviour
    {
        [SerializeField] private Tilemap floorTilemap;
        [SerializeField] private Color floorColor = new Color(0.89f, 0.72f, 0.56f); // #E3B88F
        [SerializeField] private int radius = 2;

        private Grid _grid;

        // Lazy em vez de cachear no Awake: a ordem de Awake entre GameObjects não é
        // garantida, e o SessionRunner (em outro GameObject) chama CellToWorld no
        // próprio Awake dele — podia rodar antes deste.
        private Grid GridComponent => _grid != null ? _grid : (_grid = GetComponent<Grid>());

        private void Awake()
        {
            PaintFloor();
        }

        public Vector3 CellToWorld(int col, int row)
        {
            // GetCellCenterWorld, não CellToWorld + offset manual: a conta de
            // "canto -> centro" muda de layout pra layout, e um +cellSize/2 cru só
            // funciona em grid retangular. Em isométrico dava posição assimétrica.
            return GridComponent.GetCellCenterWorld(new Vector3Int(col, row, 0));
        }

        private void PaintFloor()
        {
            if (floorTilemap == null)
            {
                return;
            }

            var tile = CreateSolidTile(floorColor, GridComponent.cellSize.x);
            for (int col = -radius; col <= radius; col++)
            {
                for (int row = -radius; row <= radius; row++)
                {
                    if (Mathf.Abs(col) + Mathf.Abs(row) <= radius)
                    {
                        floorTilemap.SetTile(new Vector3Int(col, row, 0), tile);
                    }
                }
            }
        }

        private static Tile CreateSolidTile(Color color, float cellWidth)
        {
            // Sprite de diamante (não quadrado): célula isométrica precisa de um tile em
            // forma de losango pra ladrilhar sem sobrepor/deixar vão entre os tiles.
            // Textura 2:1, pixelsPerUnit calculado a partir do cellSize.x de verdade do
            // Grid — não um valor fixo, senão desalinha se o cellSize mudar.
            const int w = 64;
            const int h = 32;
            float pixelsPerUnit = w / cellWidth;
            var texture = new Texture2D(w, h);
            var cx = w / 2f;
            var cy = h / 2f;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float dx = Mathf.Abs(x + 0.5f - cx) / cx;
                    float dy = Mathf.Abs(y + 0.5f - cy) / cy;
                    bool inside = dx + dy <= 1f;
                    texture.SetPixel(x, y, inside ? Color.white : new Color(0, 0, 0, 0));
                }
            }
            texture.filterMode = FilterMode.Point;
            texture.Apply();

            var sprite = Sprite.Create(texture, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), pixelsPerUnit);

            var tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.color = color;
            return tile;
        }
    }
}
