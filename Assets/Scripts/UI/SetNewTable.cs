using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class SetNewTable : MonoBehaviour
    {
        [SerializeField] private GameObject _button;
        [SerializeField] private Image _tableImage;
        [SerializeField] private Sprite[] _tableList;

        private int _id;
        
        private const string _tableVisualCode = "TableVisual";
        
        public void ChangeTableImage(int id)
        {
            _tableImage.sprite = _tableList[id];
            SaveTable(id);
        }
        
        private void Awake()
        {
            FillList();
            ChangeTableImage(LoadTable());
        }

        private void OnEnable() => FillList();

        private void FillList()
        {
            if (transform.childCount != 0) return;
                
            for (int spriteId = 0; spriteId < _tableList.Length; spriteId++)
            {
                GameObject button = Instantiate(_button, transform);
                button.GetComponent<Image>().sprite = _tableList[spriteId];
                button.GetComponent<SelectTable>().ActivateButton(this, spriteId);
            }
        }

        private void SaveTable(int id) => PlayerPrefs.SetInt(_tableVisualCode, id);

        private int LoadTable() => PlayerPrefs.GetInt(_tableVisualCode, 0);
    }
}