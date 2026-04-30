using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _startPoint;
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

    private void PlayerRespawn()
    {
        if (_player != null)
        {
            _player.transform.position = _startPoint.transform.position;
            _player.init();
        }
    }

    //esc로 메뉴 UI확인용.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("EscUI", LoadSceneMode.Additive);
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
