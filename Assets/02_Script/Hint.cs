using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private GameObject _panelHint;
    [SerializeField] private TMP_Text _textHint;
    [SerializeField] private TMP_Text _textWayHint;
    [SerializeField][TextArea] private string[] _hint;
    [SerializeField] [TextArea] private string[] _wayHint;
    
    private void Awake()
    {
        _panelHint.SetActive(false);
    }

    public void ShowHint(int number)
    {
        _panelHint.SetActive(true);
        _textHint.text = _hint[number];
        if (number == 0)
        {
            _textWayHint.text = _wayHint[0];
        }
        else if (number == 1)
        {
            _textWayHint.text = _wayHint[1];
        }
        else if (number == 2)
        {
            _textWayHint.text = _wayHint[2];
        }
        else
        {
            _textWayHint.text = _wayHint[3];
        }
    }

    public void CloseHint()
    {
        _panelHint.SetActive(false);
    }
}
