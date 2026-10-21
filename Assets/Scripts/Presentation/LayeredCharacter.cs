using UnityEngine;

namespace Freudinho.Presentation
{
    /// <summary>
    /// Personagem montado em camadas (paper doll) a partir das peças soltas do pack
    /// "Build a Character ~Basics" (ver specs/004-personagem-com-assets). Cada slot é um
    /// SpriteRenderer próprio, empilhado por sortingOrder — permite trocar uma peça
    /// (a boca, pro humor) sem reconstruir o personagem inteiro.
    /// </summary>
    public class LayeredCharacter : MonoBehaviour
    {
        private SpriteRenderer _body;
        private SpriteRenderer _bottom;
        private SpriteRenderer _shoes;
        private SpriteRenderer _top;
        private SpriteRenderer _hair;
        private SpriteRenderer _eyebrow;
        private SpriteRenderer _eyes;
        private SpriteRenderer _eyelashes;
        private SpriteRenderer _mouth;

        public struct Outfit
        {
            public Sprite Body;
            public Sprite Bottom;
            public Sprite Shoes;
            public Sprite Top;
            public Sprite Hair;
            public Sprite Eyebrow;
            public Sprite Eyes;
            public Sprite Eyelashes;
            public Sprite Mouth;
        }

        public void Build(Outfit outfit)
        {
            _body = AddLayer("Body", outfit.Body, 0);
            _bottom = AddLayer("Bottom", outfit.Bottom, 1);
            _shoes = AddLayer("Shoes", outfit.Shoes, 2);
            _top = AddLayer("Top", outfit.Top, 3);
            _hair = AddLayer("Hair", outfit.Hair, 4);
            _eyebrow = AddLayer("Eyebrow", outfit.Eyebrow, 5);
            _eyes = AddLayer("Eyes", outfit.Eyes, 6);
            _eyelashes = AddLayer("Eyelashes", outfit.Eyelashes, 7);
            _mouth = AddLayer("Mouth", outfit.Mouth, 8);
        }

        public void SetMouth(Sprite mouth)
        {
            if (_mouth != null)
            {
                _mouth.sprite = mouth;
            }
        }

        private SpriteRenderer AddLayer(string name, Sprite sprite, int sortingOrder)
        {
            var go = new GameObject(name);
            go.transform.SetParent(transform, false);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            return renderer;
        }
    }
}
