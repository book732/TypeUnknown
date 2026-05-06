using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Player _player;
    public GameObject startPoint;
    public bool isGamePause = false;
    public bool isGameOver = false;
    
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

        _player.PlayerDead += PlayerRespawn;
    }

    public void PlayerRespawn()
    {
        if (_player != null)
        {
            SceneManager.LoadScene("Main");
            /*_player.transform.position = startPoint.transform.position;
            _player.init();*/
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePause = false;
    }
    
    public void GameOver()
    {
        isGameOver = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
