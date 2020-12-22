using UnityEngine;

namespace CardGame
{
    public class SwitchActiveCanvas : MonoBehaviour
    {
        [SerializeField] private Canvas _target;

        public void Switch() => _target.enabled = !_target.enabled;
    }
}