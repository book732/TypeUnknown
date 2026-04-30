using UnityEngine;

[CreateAssetMenu(fileName = "TurnOnflashAction", menuName = "Scriptable Objects/TurnOnflashAction")]
public class TurnOnflashAction : ItemAction
{
    private bool isTurnOn = false;
    public override void Execute(GameObject user)
    {
        var light = user.GetComponentInChildren<Light>();

        isTurnOn = !isTurnOn;
        light.enabled = isTurnOn;

    }

}
