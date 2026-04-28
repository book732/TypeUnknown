using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _quitButton;

    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _settingButton.onClick.AddListener(OnSettingsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClick);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("Main");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    private void OnSettingsButtonClick()
    {
        
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
