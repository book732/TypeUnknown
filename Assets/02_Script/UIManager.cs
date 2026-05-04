using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Button _quitMenuButton;

    private void OnEnable()
    {
        LoadVolume();
        _startButton.onClick.AddListener(OnStartButtonClick);
        _settingButton.onClick.AddListener(OnSettingsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _settingPanel.SetActive(false);
        _quitMenuButton.onClick.AddListener(OnQuitMenuButtonClick);
        _volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
    }


    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClick);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
        _quitMenuButton.onClick.RemoveListener(OnQuitMenuButtonClick);
        _volumeSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetVolume);
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    private void OnSettingsButtonClick()
    {
        _settingPanel.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        GameManager.Instance.QuitGame();
    }
    private void OnQuitMenuButtonClick()
    {
        _settingPanel.SetActive(false);
    }

    private void LoadVolume()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("VOLUME", 1f);
    }
}
