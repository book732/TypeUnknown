using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioMixer _audioMixer;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    public void SetVolume(float volume)
    {
        if (volume <= 0.01)
        {
            _audioMixer.SetFloat("VOLUME", -80f);
        }
        else
        {
            _audioMixer.SetFloat("VOLUME", (float)(Math.Log10(volume) * 20));
        }
        PlayerPrefs.SetFloat("VOLUME", volume);
    }
}
