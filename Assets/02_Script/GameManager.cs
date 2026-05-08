using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Player _player;
    public static Action OnGameOver;
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
        Cursor.lockState = CursorLockMode.Locked;
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
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
