using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class SelectTable : MonoBehaviour
    {
        private SetNewTable _controller;
        private int _buttonId;

        public void ActivateButton(SetNewTable controller, int id)
        {
            _buttonId = id;
            _controller = controller;
            
            GetComponent<Button>().onClick.AddListener(Press);
        }

        public void Press() => _controller.ChangeTableImage(_buttonId);
    }
}