using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HealAction", menuName = "Scriptable Objects/HealAction")]


public class HealAction : ItemAction
{
    [SerializeField] float healAmount = 30f;
    public override void Execute(GameObject user)
    {
        var player = user.GetComponent<Player>();
        player.RestoreHealth(healAmount);
    }

}
