using UnityEngine;

[CreateAssetMenu(fileName = "OpenMapAction", menuName = "Scriptable Objects/OpenMapAction")]
public class OpenMapAction : ItemAction
{
    //스크립터블오브젝트에 이런 상태값을넣어놓으면안되지만 일단멀티기능은없기에 임시적으로이렇게해놓음 
    private bool isOpend = false;
    public override void Execute(GameObject user)
    {
        var player = user.GetComponent<Player>();
        if (player != null)
        {
            isOpend = !isOpend;
            player.uIManager.OnMapUI(isOpend);
        }
    }
}
