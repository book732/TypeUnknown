using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject _panelHint;
    [SerializeField] private TMP_Text _textHint;
    [SerializeField][TextArea] private string[] _hint;
    
    private void Awake()
    {
        _panelHint.SetActive(false);
    }

    public void ShowHint(int number)
    {
        _panelHint.SetActive(true);
        _textHint.text = _hint[number];
    }

    public void CloseHint()
    {
        _panelHint.SetActive(false);
    }
}
