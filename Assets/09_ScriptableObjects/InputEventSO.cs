using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputEventSO", menuName = "Scriptable Objects/InputEventSO")]
public class InputEventSO : ScriptableObject
{
    //리스너 목록
    public Action<Vector2> OnMove;
    public Action OnJump;
    public Action<Vector2> OnLook;


    public void SubscribeMove(Action<Vector2> listener)
    {
        OnMove += listener;
    }

    public void UnSubscribeMove(Action<Vector2> listener)
    {
        OnMove -= listener;
    }


    public void RaiseMoveEvent(Vector2 input)
    {
        OnMove?.Invoke(input);
        //  Debug.Log("Move Event Raised: " + input);
    }

}
