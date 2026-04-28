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


        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
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
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Disable();
    }
}