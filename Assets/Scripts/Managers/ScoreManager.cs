using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TextMeshProUGUI _inGameScoreText;

        private static int _score = 0;

        private void Update()
        {
            UpdateInGameScoreText();
        }

        public void UpdateInGameScoreText()
        {
            _inGameScoreText.text = GetScore().ToString();
        }

        public static void AddScore(int score)
        {
            _score += score;
        }

        public static void RemoveScore(int score)
        {
            _score -= score;
        }

        public static int GetScore()
        {
            return _score;
        }

        public static void AddScoreDefault()
        {
            _score += 10;
        }

        public static void ResetScore()
        {
            _score = 0;
        }

        public static void SetScore(int score)
        {
            _score = score;
        }

        public static void SetHighScoreIfGreaterThanBefore()
        {
            if (PlayerPrefsManager.GetHighScore() < _score)
            {
                PlayerPrefsManager.SetHighScore(_score);
            }
        }
    }
}