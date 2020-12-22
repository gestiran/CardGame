using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardGame
{
    public class LoadNewScreen : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private GameObject _loadingObject;
        
        [Header("Parameters")]
        [SerializeField] private int _sceneBuildId;
        [SerializeField] private float _waitingTime;
        
        [Header("Animate parameters")]
        [SerializeField] private bool _isAnimate;
        [SerializeField] private float _animateSpeed;

        private int _currentSceneId;
        private int currentTime;
        
        private void Start()
        {
            _currentSceneId = SceneManager.GetActiveScene().buildIndex;
            
            if (_isAnimate) StartCoroutine(StartAnimation());
            
            StartLoad();
        }

        private void StartLoad()
        {
            currentTime = System.DateTime.Now.Second;
            StartCoroutine(SetScene());
            SceneManager.LoadSceneAsync(_sceneBuildId, LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private IEnumerator SetScene()
        {
            yield return new WaitForSeconds(_waitingTime - (System.DateTime.Now.Second - currentTime));
            SceneManager.UnloadSceneAsync(_currentSceneId);

            while (SceneManager.sceneCount < 1) yield return new WaitForFixedUpdate();
            
            FindObjectOfType<GameCamera>().ActivateCamera();
            StopAllCoroutines();
        }

        private IEnumerator StartAnimation()
        {
            if (_loadingObject == null) yield break;
            
            for (int i = 0; i < _waitingTime * 60; i++)
            {
                _loadingObject.transform.rotation = Quaternion.Euler(
                    _loadingObject.transform.rotation.eulerAngles.x,
                    _loadingObject.transform.rotation.eulerAngles.y,
                _loadingObject.transform.rotation.eulerAngles.z + _animateSpeed);
                yield return new WaitForFixedUpdate();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_currentSceneId));

    }
}