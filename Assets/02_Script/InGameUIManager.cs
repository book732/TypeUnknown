using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Button _quitMenuButton;
    [SerializeField] private Canvas _canvas;
    //Ãß°¡º»
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Image _itempickupImage;


    private void OnEnable()
    {
        LoadVolume();
        GameManager.Instance.PauseGame();
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _settingButton.onClick.AddListener(OnSettingsButtonClick);
        _resumeButton.onClick.AddListener(OnResumeGameClick);
        _settingPanel.SetActive(false);
        _volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
        _quitMenuButton.onClick.AddListener(OnQuitMenuButtonClick);
    }

    private void OnDisable()
    {
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        _resumeButton.onClick.RemoveListener(OnResumeGameClick);
        _volumeSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetVolume);
        _quitMenuButton.onClick.RemoveListener(OnQuitMenuButtonClick);
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

    private void OnResumeGameClick()
    {
        SceneManager.UnloadSceneAsync("EscUI");
        GameManager.Instance.ResumeGame();
    }

    private void LoadVolume()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("VOLUME", 1f);
    }
}
