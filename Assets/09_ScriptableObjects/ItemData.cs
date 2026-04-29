using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private ItemAction itemAction;
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private bool isConsumable;


    public string ItemName => itemName;
    public Sprite ItemIcon => itemIcon;

    public bool IsConsumable => isConsumable;

    public ItemAction ItemAction => itemAction;

    public GameObject ItemPrefab => itemPrefab;
}
