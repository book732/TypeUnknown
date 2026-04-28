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
    [SerializeField] private AudioMixer  _audioMixer; 
    
    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
        _settingButton.onClick.AddListener(OnSettingsButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
        _settingPanel.SetActive(false);
        _quitMenuButton.onClick.AddListener(OnQuitMenuButtonClick);
        _volumeSlider.onValueChanged.AddListener(SetVolume);
    }


    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartButtonClick);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClick);
        _quitButton.onClick.RemoveListener(OnQuitButtonClick);
        _quitMenuButton.onClick.RemoveListener(OnQuitMenuButtonClick);
        
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene("Main");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    private void OnSettingsButtonClick()
    {
        _settingPanel.SetActive(true);
    }

    private void OnQuitButtonClick()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    private void OnQuitMenuButtonClick()
    {
        _settingPanel.SetActive(false);
    }

    private void SetVolume(float volume)
    {
        if (volume <= 0.01)
        {
            _audioMixer.SetFloat("Volume", -80f);
        }
        else
        {
            _audioMixer.SetFloat("Volume", (float)(Math.Log10(volume) * 20));
        }
    }
}
