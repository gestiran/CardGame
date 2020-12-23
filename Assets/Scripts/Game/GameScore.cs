using CardGame.SaveLoad;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public class GameScore : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;

        private int _currentScore;

        public void LoadScore()
        {
            if (!GameSaveLoad.HasScore) return;
            _currentScore = GameSaveLoad.LoadScore();
            ShowScore();
        }
        
        public void Add()
        {
            _currentScore++;
            ShowScore();
            GameSaveLoad.SaveScore(_currentScore);
        }

        private void ShowScore() => _scoreText.text = _currentScore.ToString();
    }
}