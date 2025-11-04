using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenuPanel;

    public void SetActive(bool active)
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(active);
        }
        else
        {
            gameObject.SetActive(active);
        }
    }

    public void SetPauseMenuPanel(GameObject panel)
    {
        pauseMenuPanel = panel;
    }

    public void OnResumeClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPaused(false);
        }
    }

    public void OnQuitClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
    }

    public void OnSensitivityChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMouseSensitivity(value);
        }
    }

    public void OnFOVChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetFieldOfView(value);
        }
    }

    public void OnVolumeChanged(float value)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetMasterVolume(value);
        }
    }
}
