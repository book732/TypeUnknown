using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputEventSO", menuName = "Scriptable Objects/InputEventSO")]
public class InputEventSO : ScriptableObject
{
    //리스너 목록
    public Action<Vector2> OnMove;
    public Action OnJump;
    public Action<Vector2> OnLook;
    public Action OnPickup;
    public Action OnInteract;
    public Action<Vector2> OnChangeItem;
    public Action OnEscape;
    public Action<bool> OnSprint;


    #region 탈출
    public void SubscribeEscape(Action listener)
    {
        OnEscape += listener;
    }

    public void UnSubscribeEscape(Action listener)
    {
        OnEscape -= listener;
    }

    public void RaiseEscapeEvent()
    {
        OnEscape?.Invoke();
        //  Debug.Log("Move Event Raised: " + input);
    }

    #endregion
    
    
    #region 이동
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
    #endregion


    #region 아이템줍기
    public void SubscribePickup(Action listener)
    {
        OnPickup += listener;
    }

    public void UnSubscribePickup(Action listener)
    {
        OnPickup -= listener;
    }


    public void RaisePickupEvent()
    {
        OnPickup?.Invoke();
    }
    #endregion



    #region 아이템상호작용
    public void SubscribeInteract(Action listener)
    {
        OnInteract += listener;
    }

    public void UnSubscribeInteract(Action listener)
    {
        OnInteract -= listener;
    }


    public void RaiseInteractEvent()
    {
        OnInteract?.Invoke();
    }
    #endregion


    #region 아이템키변경 
    public void SubscribeChangeItem(Action<Vector2> listener)
    {
        OnChangeItem += listener;
    }

    public void UnSubscribeChangeItem(Action<Vector2> listener)
    {
        OnChangeItem -= listener;
    }


    public void RaiseChangeItemEvent(Vector2 input)
    {
        OnChangeItem?.Invoke(input);
    }
    #endregion

    #region 스프린트

    public void SubscribeSprint(Action<bool> listener)
    {
        OnSprint += listener;
    }

    public void UnSubscribeSprint(Action<bool> listener)
    {
        OnSprint -= listener;
    }
    public void RaiseSprintEvent(bool _isRun)
    {
        OnSprint?.Invoke(_isRun);
    }

    #endregion
}
