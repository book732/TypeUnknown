using System;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Player _player;
    public static Action OnGameOver;
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
        Cursor.visible = false;
    }

    public void PlayerRespawn()
    {
        if (_player != null)
        {
            _player.gameObject.SetActive(false);
            SceneManager.LoadScene("Main");
            isGameOver = false;
            /*_plwayer.transform.position = startPoint.transform.position;
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
        OnGameOver?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    
}
