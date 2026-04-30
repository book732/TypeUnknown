using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlickLight : MonoBehaviour
{
    [SerializeField] private GameObject _light;
    private Coroutine _flicker;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && _flicker == null)
        {
            _flicker = StartCoroutine(InMonsterRange());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(_flicker);
        _flicker = null;
        _light.SetActive(true);
    }
    
    
    private IEnumerator InMonsterRange()
    {
        while (true)
        {
            _light.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.05f,0.1f));
            _light.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
        }
    }

}
