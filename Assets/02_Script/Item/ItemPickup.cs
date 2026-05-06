using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    [SerializeField] public GameObject lightEffectprefab;
    private float itemEffectDownOffset = 0.1f;
    private GameObject lightEffect;
    private void Awake()
    {
        if (lightEffectprefab != null)
        {
            lightEffect = Instantiate(lightEffectprefab, this.transform);
            lightEffect.transform.position = transform.position - this.transform.up * itemEffectDownOffset;
        }

    }
    public void TurnOffLightEffect()
    {
        //var lighEffect = this.GetComponentsInChildren<Light>(true)[1];

        //var LightObject = lighEffect.gameObject;
        lightEffect.SetActive(false);
    }


}

