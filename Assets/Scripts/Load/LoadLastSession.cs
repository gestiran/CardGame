using CardGame.SaveLoad;
using UnityEngine;

namespace CardGame
{
    public class LoadLastSession : MonoBehaviour
    {
        [SerializeField] private GameObject _loadButton;
        
        private void Start()
        {
            if (!GameSaveLoad.HasSaved && !GameSaveLoad.HasScore && !GameSaveLoad.HasObjectsActives && !GameSaveLoad.HasSuit) _loadButton.SetActive(false);
        }

        public void Load()
        {
            FindObjectOfType<GameScore>().LoadScore();
            FindObjectOfType<CardsMovemant>().LoadGame();
        }
    }
}