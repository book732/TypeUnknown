using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputEventSO inputEventSO;
    private InputSystem_Actions inputActions;
    private bool _isOpenUI;
    private bool _isOpenInven;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void ItemChange(InputAction.CallbackContext context)
    {
        inputEventSO.RaiseChangeItemEvent(context.ReadValue<Vector2>());
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputEventSO.RaiseSprintEvent(true);
        }
        else if (context.canceled)
        {
            inputEventSO.RaiseSprintEvent(false);
        }
    }

    private void OpenUI(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        _isOpenUI = !_isOpenUI;
        inputEventSO.RaiseMenuEvent(_isOpenUI);
    }

    private void OpenInven(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        _isOpenInven = !_isOpenInven;
        inputEventSO.RaiseInvenEvent(_isOpenInven);
    }
    
    private void Look(InputAction.CallbackContext context)
    {
            Vector2 input = context.ReadValue<Vector2>();
            inputEventSO.RaiseLookEvent(input);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            inputEventSO.RaiseJumpEvent(true); 
        }
    }
    
    private void Escape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputEventSO.RaiseEscapeEvent();
        }
    }

    void BindAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed += callback;
        action.canceled += callback;
    }

    void UnBindAction(InputAction action, Action<InputAction.CallbackContext> callback)
    {
        action.performed -= callback;
        action.canceled -= callback;
    }

    private void InteractItem(InputAction.CallbackContext context)
    {
        inputEventSO.RaiseInteractEvent();
        // Debug.Log("Interact Event Raised");

    }

    private void GetItem(InputAction.CallbackContext context)
    {
        inputEventSO.RaisePickupEvent();
    }


    private void OnMove(InputAction.CallbackContext ctx)
    {
            Vector2 input = ctx.ReadValue<Vector2>();
            inputEventSO.RaiseMoveEvent(input);
    }

    private void OnEnable()
    {
        //키바인딩        
        BindAction(inputActions.Player.Move, OnMove);
        BindAction(inputActions.Player.Pick, GetItem);
        BindAction(inputActions.Player.Interact, InteractItem);
        BindAction(inputActions.Player.Jump, Jump);
        BindAction(inputActions.Player.Look, Look);
        BindAction(inputActions.Player.Sprint, Sprint);
        BindAction(inputActions.Player.Crouch, Crouch);
        BindAction(inputActions.Player.ItemChange, ItemChange);
        BindAction(inputActions.Player.Escape, Escape);
        BindAction(inputActions.Player.OpenUI, OpenUI);
        BindAction(inputActions.Player.OpenInven, OpenInven);
    }
    private void OnDisable()
    {
        UnBindAction(inputActions.Player.Jump, Jump);
        UnBindAction(inputActions.Player.Move, OnMove);
        UnBindAction(inputActions.Player.Pick, GetItem);
        UnBindAction(inputActions.Player.Interact, InteractItem);
        UnBindAction(inputActions.Player.ItemChange, ItemChange);
        UnBindAction(inputActions.Player.Escape, Escape);
        UnBindAction(inputActions.Player.Sprint, Sprint);
        UnBindAction(inputActions.Player.Look, Look);
        UnBindAction(inputActions.Player.OpenUI, OpenUI);
        UnBindAction(inputActions.Player.OpenInven, OpenInven);
        inputActions.Player.Disable();
    }
}