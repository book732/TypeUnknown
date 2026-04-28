using UnityEngine;

[CreateAssetMenu(fileName = "ItemAction", menuName = "Scriptable Objects/ItemAction")]





public class ItemAction : ScriptableObject
{

    [SerializeField] private string actionKey;


    public string ActionKey => actionKey;
}
