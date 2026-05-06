using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [SerializeField] Hint hint;
    [SerializeField] private int _hintNumber;

    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            hint.ShowHint(_hintNumber);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            hint.CloseHint();
        }
    }
}
