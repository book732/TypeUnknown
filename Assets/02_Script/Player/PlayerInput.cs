using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputEventSO inputEventSO;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    private void OnEnable()
    {
        //키바인딩        
        BindAction(inputActions.Player.Move, OnMove);
        BindAction(inputActions.Player.Pick, GetItem);

        //Interect item 이 TODO 입력 이상하게들어오는데 이거수정할것 
        BindAction(inputActions.Player.Interact, InteractItem);
        BindAction(inputActions.Player.Jump, Jump);
        BindAction(inputActions.Player.Look, Look);
        BindAction(inputActions.Player.Sprint, Sprint);
        BindAction(inputActions.Player.Crouch, Crouch);
        BindAction(inputActions.Player.ItemChange, ItemChange);
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

    }

    private void Look(InputAction.CallbackContext context)
    {

    }

    private void Jump(InputAction.CallbackContext context)
    {

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
        //  Debug.Log(input);
        inputEventSO.RaiseMoveEvent(input);
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        UnBindAction(inputActions.Player.Move, OnMove);
        UnBindAction(inputActions.Player.Pick, GetItem);
        UnBindAction(inputActions.Player.Interact, InteractItem);
        UnBindAction(inputActions.Player.ItemChange, ItemChange);


        inputActions.Player.Disable();
    }
}