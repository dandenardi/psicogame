using UnityEngine;
using UnityEngine.SceneManagement;

namespace Freudinho.Core
{
    /// <summary>
    /// Único papel da cena Boot: carregar a cena Office assim que o jogo inicia.
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        private const string OfficeSceneName = "Office";

        private void Start()
        {
            SceneManager.LoadScene(OfficeSceneName);
        }
    }
}
