using UnityEngine;

public class HintTrigger : MonoBehaviour
{
    [SerializeField] Hint hint;


    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {

            hint.ShowHint();


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            hint.CloseHint();
            Destroy(gameObject);
        }
    }
}
