using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5.0f;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isNearItem = false;
    private Inventory inventory;
    [SerializeField] private InputEventSO inputEventSO;
    public event Action<ItemData> OnInventoryChanged;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = new Inventory();

    }

    private void OnEnable()
    {
        inputEventSO.SubscribeMove(SetMove);
        inputEventSO.SubscribePickup(Detectitem);
        inputEventSO.SubscribeInteract(InteractItem);

    }



    private void InteractItem()
    {
        Debug.Log("Interacted with item");
    }

    private void OnDisable()
    {
        inputEventSO.UnSubscribeMove(SetMove);
        inputEventSO.UnSubscribePickup(Detectitem);
        inputEventSO.UnSubscribeInteract(InteractItem);

    }

    private void Detectitem()
    {

        if (Existitem() != null)
        {
            ItemData itemData = Existitem();
            inventory.AddItem(new ItemInstance(itemData));
            Debug.Log("Picked up: " + itemData.ItemName);
            OnInventoryChanged?.Invoke(itemData);

        }

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

    private ItemData Existitem()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);

            if (hit.collider.CompareTag("Item"))
            {
                ItemData itemData = hit.collider.GetComponent<ItemPickup>().itemData;
                hit.collider.GetComponent<ItemPickup>().EatedItem();
                return itemData;


            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }







    private void FixedUpdate()
    {
        Movement();
    }
}








