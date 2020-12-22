using System.Collections;
using System.Collections.Generic;
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
            if (!CardsSaveLoad.HasScore) return;
            _currentScore = CardsSaveLoad.LoadScore();
            ShowScore();
        }
        
        public void Add()
        {
            _currentScore++;
            ShowScore();
            CardsSaveLoad.SaveScore(_currentScore);
        }

        private void ShowScore() => _scoreText.text = _currentScore.ToString();
    }
}