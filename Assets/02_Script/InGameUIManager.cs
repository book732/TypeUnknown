using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder.MeshOperations;
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
    [SerializeField] private Player _player;
    [SerializeField] private Image _mapUI;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Image _itempickupImage;
    [SerializeField] private Image _mentalImage;

    private void Awake()
    {
        _player.OnHpchanged += UpdateMentalUI;

    }

    private void UpdateMentalUI(float mental, float maxMental)
    {

        float percent = mental / maxMental;
        float alpha = 0f;

        if (percent >= 1f)
        {
            alpha = 1f;
        }
        else if (percent >= 0.8f)
        {
            alpha = 55f;
        }
        else if (percent >= 0.6f)
        {
            alpha = 110f;
        }
        else if (percent >= 0.4f)
        {
            alpha = 165f;
        }
        else if (percent >= 0.2f)
        {
            alpha = 220f;
        }
        else
        {
            alpha = 220f;
        }
        //¾ËÆÄ°ª ¸¶Áö¸·¿¡ 255³ª´²Áà¾ßµÊ 
        alpha = alpha / 255f;
        var color = _mentalImage.color;
        color.a = alpha;
        _mentalImage.color = color;
    }

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

    public void OnMapUI(bool isopend)
    {
        SetUIActive(_mapUI.gameObject, isopend);
    }

    public void SetUIActive(GameObject target, bool isActive)
    {
        target.SetActive(isActive);
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
