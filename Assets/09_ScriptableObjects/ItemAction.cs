using UnityEngine;

[CreateAssetMenu(fileName = "ItemAction", menuName = "Scriptable Objects/ItemAction")]
public abstract class ItemAction : ScriptableObject
{

    public abstract void Execute(GameObject user);


}
