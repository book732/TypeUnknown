using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private Button _quitMenuButton;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _inGameUI;
    //Ãß°¡º»
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Image _itempickupImage;
    
    private void OnEnable()
    {
        LoadVolume();
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _settingButton.onClick.AddListener(OnSettingsButtonClick);
        _inGameUI.SetActive(false);
        _settingPanel.SetActive(false);
        _volumeSlider.onValueChanged.AddListener(AudioManager.Instance.SetVolume);
        _quitMenuButton.onClick.AddListener(OnQuitMenuButtonClick);

    }

    private void OnDisable()
    {
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        _volumeSlider.onValueChanged.RemoveListener(AudioManager.Instance.SetVolume);
        _quitMenuButton.onClick.RemoveListener(OnQuitMenuButtonClick);
    }

    private void OnSettingsButtonClick()
    {
        _settingPanel.SetActive(true);
    }

    public void OnPickUpUI(bool isExisit)
    {
        _itempickupImage.gameObject.SetActive(isExisit);
    }

    private void OnQuitButtonClick()
    {
        SceneManager.LoadScene("Menu");
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
