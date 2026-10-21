using UnityEngine;

namespace Freudinho.Presentation
{
    /// <summary>
    /// Conjunto fixo de peças (pack "Build a Character ~Basics") pra montar um
    /// LayeredCharacter. Sem sistema de customização — é só agrupar as 9 referências
    /// de sprite pra não espalhar campo solto no SessionRunner (ver spec 004).
    /// </summary>
    [CreateAssetMenu(fileName = "NovoFigurino", menuName = "Freudinho/Figurino de Personagem")]
    public class CharacterOutfitAsset : ScriptableObject
    {
        public Sprite body;
        public Sprite bottom;
        public Sprite shoes;
        public Sprite top;
        public Sprite hair;
        public Sprite eyebrow;
        public Sprite eyes;
        public Sprite eyelashes;
        public Sprite mouthNeutral;
        public Sprite mouthFeeling;

        public LayeredCharacter.Outfit ToOutfit(bool feeling = false)
        {
            return new LayeredCharacter.Outfit
            {
                Body = body,
                Bottom = bottom,
                Shoes = shoes,
                Top = top,
                Hair = hair,
                Eyebrow = eyebrow,
                Eyes = eyes,
                Eyelashes = eyelashes,
                Mouth = feeling ? mouthFeeling : mouthNeutral
            };
        }
    }
}
