using UnityEngine;

namespace CardGame
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField] private GameObject _cameraObject;

        #if !UNITY_EDITOR
        
        private void Awake() => _cameraObject.SetActive(false);

        #endif

        public void ActivateCamera() => _cameraObject.SetActive(true);
    }
}