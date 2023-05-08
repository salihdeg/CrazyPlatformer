using Managers;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Toggle _toggle;

    private void Start()
    {
        int volume = PlayerPrefsManager.GetVolumeStatus();
        AudioListener.volume = volume;
        if (volume == 0)
        {
            _toggle.isOn = true;
        }
        else
        {
            _toggle.isOn = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetActiveStatusAndPauseIfNeed();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetActiveStatusAndPauseIfNeed()
    {
        if (_panel.activeInHierarchy)
        {
            Time.timeScale = 1;
            _panel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            _panel.SetActive(true);
        }
    }

    public void Continue()
    {
        Time.timeScale = 1;
        _panel.SetActive(false);
    }

    public void ChangeVolumeStatus(bool status)
    {
        if (status)
        {
            AudioListener.volume = 0;
            PlayerPrefsManager.SetVolumeStatus(0);
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefsManager.SetVolumeStatus(1);
        }
    }
}
