using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class ReloadScene : MonoBehaviour
    {
        public void Reload() => SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}