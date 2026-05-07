using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingPanel : MonoBehaviour
{
    [SerializeField] private Button _endRestartButton;
    [SerializeField] private Button _endQuitButton;

    private void OnEnable()
    {
        Cursor.visible = true;
        _endRestartButton.onClick.AddListener(EndingRestartButton);
        _endQuitButton.onClick.AddListener(EndingQuitButton);
    }

    private void OnDisable()
    {
        _endRestartButton.onClick.RemoveListener(EndingRestartButton);
        _endQuitButton.onClick.RemoveListener(EndingQuitButton);
    }
    
    private void EndingRestartButton()
    {
        SceneManager.LoadScene("Menu");
    }
    
    private void EndingQuitButton()
    {
        GameManager.Instance.QuitGame();
    }
}
