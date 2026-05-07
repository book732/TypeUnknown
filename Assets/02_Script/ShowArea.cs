using UnityEngine;


public class AlarmDestination : MonoBehaviour
{
    [SerializeField] private GameObject _church;
    [SerializeField] private GameObject _barn;
    [SerializeField] private GameObject _mansion;
    [SerializeField] private int _selecteIndex;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_selecteIndex == 0)
            {
                _mansion.SetActive(false);
                _barn.SetActive(false);
                _church.SetActive(true);
            }
            else if (_selecteIndex == 1)
            {
                _mansion.SetActive(false);
                _church.SetActive(false);
                _barn.SetActive(true);
            }
            else if (_selecteIndex == 2)
            {
                _mansion.SetActive(true);
                _church.SetActive(false);
                _barn.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _mansion.SetActive(false);
            _church.SetActive(false);
            _barn.SetActive(false);
        }
    }
}
