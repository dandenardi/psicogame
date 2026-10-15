using System;
using UnityEngine;

namespace Freudinho.Presentation
{
    /// <summary>
    /// Repassa OnMouseDown como evento C#, pra objetos de UI mundo (balão) não
    /// precisarem de Canvas/EventSystem só pra detectar clique.
    /// </summary>
    public class ClickRelay : MonoBehaviour
    {
        public event Action Clicked;

        private void OnMouseDown()
        {
            Clicked?.Invoke();
        }
    }
}
