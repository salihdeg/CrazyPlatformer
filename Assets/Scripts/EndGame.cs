using Managers;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _finishPanel;
    [SerializeField] private TextMeshProUGUI _lastScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _finishPanel.SetActive(true);
            GameManager.isStart = false;
            ScoreManager.SetHighScoreIfGreaterThanBefore();
            _lastScoreText.text = "Score: " + ScoreManager.GetScore();
            _bestScoreText.text = "Best Score: " + PlayerPrefsManager.GetHighScore();
        }
    }
}
