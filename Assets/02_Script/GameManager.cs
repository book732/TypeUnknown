using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
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
