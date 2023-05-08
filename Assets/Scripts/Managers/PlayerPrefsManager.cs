using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Managers
{
    public class PlayerPrefsManager : MonoBehaviour
    {
        //public static PlayerPrefsManager Instance;

        //private void Awake()
        //{
        //    if (Instance == null) Instance = this;
        //    else Destroy(gameObject);
        //    DontDestroyOnLoad(gameObject);
        //}

        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt("HighScore", 0);
        }

        public static void SetHighScore(int highScore)
        {
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        public static int GetCurrentScore()
        {
            return PlayerPrefs.GetInt("Score", 0);
        }

        public static void SetCurrentScore(int score)
        {
            PlayerPrefs.SetInt("Score", score);
        }

        public static int GetLastLevelIndex()
        {
            return PlayerPrefs.GetInt("LevelIndex", 0);
        }

        public static void SetLastLevelIndex(int levelIndex)
        {
            PlayerPrefs.SetInt("LevelIndex", levelIndex);
        }

        public static void SetVolumeStatus(int volume)
        {
            PlayerPrefs.SetInt("Volume", volume);
        }

        public static int GetVolumeStatus()
        {
            return PlayerPrefs.GetInt("Volume", 1);
        }

    }
}
