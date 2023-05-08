using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("UI")]
        public GameObject restartPanel;
        public GameObject menuPanel;
        public GameObject informationPanel;
        [SerializeField] private TextMeshProUGUI _resetPanelHighScore;
        [SerializeField] private TextMeshProUGUI _resetPanelScore;
        [SerializeField] private TextMeshProUGUI _menuPanelHightScore;

        [Header("")]
        public static bool isStart = false;
        public static bool isRestart = false;

        private void Start()
        {
            restartPanel.SetActive(false);

            if (isRestart)
            {
                menuPanel.SetActive(false);
                informationPanel.SetActive(false);
                isStart = true;
            }

            menuPanel.SetActive(!isStart);
            informationPanel.SetActive(!isStart);

            SetScoreOnPanels();
        }

        public void StartGame()
        {
            isStart = true;
            menuPanel.SetActive(false);
        }

        public void CloseInformation()
        {
            informationPanel.SetActive(false);
        }

        public void ReloadCurrentScene()
        {
            ScoreManager.SetScore(PlayerPrefsManager.GetCurrentScore());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartGame()
        {
            isRestart = true;
            PlayerController.currentHealth = PlayerController.maxHealth;
            if (ScoreManager.GetScore() > PlayerPrefsManager.GetHighScore())
                PlayerPrefsManager.SetHighScore(ScoreManager.GetScore());

            PlayerPrefsManager.SetLastLevelIndex(0);
            SetScoreOnPanels();

            ScoreManager.ResetScore();
            PlayerPrefsManager.SetCurrentScore(ScoreManager.GetScore());
            SceneManager.LoadScene(0);
        }

        public void GoNextSceneIfExist()
        {
            int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (levelIndex < SceneManager.sceneCountInBuildSettings)
            {
                PlayerController.currentHealth = PlayerController.maxHealth;
                PlayerPrefsManager.SetLastLevelIndex(levelIndex);
                PlayerPrefsManager.SetCurrentScore(ScoreManager.GetScore());
                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                Debug.Log("Sahne Yok Hacım");
            }
        }

        public void SetScoreOnPanels()
        {
            PlayerPrefsManager.SetCurrentScore(ScoreManager.GetScore());

            if (PlayerPrefsManager.GetCurrentScore() > PlayerPrefsManager.GetHighScore())
                PlayerPrefsManager.SetHighScore(PlayerPrefsManager.GetCurrentScore());

            _menuPanelHightScore.text = "High Score: " + PlayerPrefsManager.GetHighScore().ToString();
            _resetPanelHighScore.text = "High Score: " + PlayerPrefsManager.GetHighScore().ToString();
            _resetPanelScore.text = "Score: " + PlayerPrefsManager.GetCurrentScore().ToString();
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}