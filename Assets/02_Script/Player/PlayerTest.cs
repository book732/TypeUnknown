using Unity.VisualScripting;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private float speed = 5.0f;
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isNearItem = false;
    private Inventory inventory;
    [SerializeField] private InputEventSO inputEventSO;
    private GameObject ActiveItem;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] Transform HandTransform;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = new Inventory();
        inventoryUI.InitInventory(inventory);
        inventory.OnActiveInventorySlotchanged += GenerateAndInitializeActiveItem;

    }


    private void OnEnable()
    {
        inputEventSO.SubscribeMove(SetMove);
        inputEventSO.SubscribePickup(Detectitem);
        inputEventSO.SubscribeInteract(InteractItem);

    }

    //TODO 매번생성하고없애서 성능낭비 심함 OBJECTPOOLING으로바꾸거나 이제같으면계속쓰느식으로 
    private void GenerateAndInitializeActiveItem(int activeslotIndex)
    {



        //이전에생성한 아이템이있으면 destroy  하는 코드  같으면 그대로빠져나가고 
        if (ActiveItem != null)
        {
            Destroy(ActiveItem);
        }
        //인덱스번호에해당하는아이템data로 아이템생성및 이전아이템있으면 없애는코드 

        if (inventory.items == null) return;

        if (inventory.items[activeslotIndex] == null) return;


        var item = inventory.items[activeslotIndex].ItemData.ItemPrefab;
        if (item == null) return;




        ActiveItem = Instantiate(item, HandTransform.position, item.transform.rotation);
        var collider = ActiveItem.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        ActiveItem.transform.SetParent(HandTransform);



    }



    private void InteractItem()
    {
        inventory.UseActiveItem(this.gameObject);

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
        Vector3 startposition = transform.position;
        Vector3 halfextension = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 direction = transform.forward;
        Quaternion rotation = transform.rotation;
        float distance = 1f;

        //TODO BOXOVERLAP 으로바꿀것 
        if (Physics.BoxCast(transform.position, halfextension, direction, out hit, rotation, distance))
        {


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
    void OnDrawGizmos()
    {

        Vector3 center = transform.position;
        Vector3 halfExtents = new Vector3(0.5f, 0.5f, 0.5f);
        Quaternion rot = transform.rotation;
        Vector3 dir = transform.forward;
        float dist = 1f;


        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, rot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);


        Vector3 endCenter = center + dir * dist;
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(endCenter, rot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center, endCenter);
    }



    private void FixedUpdate()
    {

        Movement();
    }
}
