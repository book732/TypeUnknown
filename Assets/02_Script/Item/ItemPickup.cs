using System;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] public ItemData itemData;
    [SerializeField] public GameObject lightEffectprefab;
    private float itemEffectDownOffset = 0.1f;

    private void Awake()
    {
        if (lightEffectprefab != null)
        {
            var lightEffect = Instantiate(lightEffectprefab, this.transform);
            lightEffect.transform.position = transform.position - this.transform.up * itemEffectDownOffset;
        }

    }
    public void TurnOffLightEffect()
    {
        var lighEffect = this.GetComponentInChildren<Light>();

        var LightObject = lighEffect.gameObject;
        LightObject.SetActive(false);
    }


}

