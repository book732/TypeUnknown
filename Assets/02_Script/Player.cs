using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5.0f;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    [SerializeField] private InputEventSO inputEventSO;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputEventSO.SubscribeMove(SetMove);

    }

    private void OnDisable()
    {
        inputEventSO.UnSubscribeMove(SetMove);
    }


    private void SetMove(Vector2 input)
    {
        horizontalInput = input.x;
        verticalInput = input.y;

        //   Debug.Log("Move Event Received: " + input);

    }



    private void Movement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }


    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {

    }
}








