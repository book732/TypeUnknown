using UnityEngine;

[CreateAssetMenu(fileName = "TurnOnflashAction", menuName = "Scriptable Objects/TurnOnflashAction")]
public class TurnOnflashAction : ItemAction
{
    //스크립터블오브젝트에 이런 상태값을넣어놓으면안되지만 일단멀티기능은없기에 임시적으로이렇게해놓음 
    private bool isTurnOn = false;
    public override void Execute(GameObject user)
    {
        var light = user.GetComponentInChildren<Light>();

        isTurnOn = !isTurnOn;
        light.enabled = isTurnOn;

    }

}
