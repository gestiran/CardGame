using CardGame.SaveLoad;
using UnityEngine;

namespace CardGame
{
    public class LoadLastSession : MonoBehaviour
    {
        [SerializeField] private GameObject _loadButton;
        
        private void Start()
        {
            if (!CardsSaveLoad.HasSaved && !CardsSaveLoad.HasScore && !CardsSaveLoad.HasObjectsActives) _loadButton.SetActive(false);
        }

        public void Load()
        {
            FindObjectOfType<GameScore>().LoadScore();
            FindObjectOfType<CardsMovemant>().LoadGame();
        }
    }
}