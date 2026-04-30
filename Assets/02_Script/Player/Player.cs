using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;


public class Player : MonoBehaviour
{

    [SerializeField] private float _stamina; // mp 역할
    [SerializeField] private float _mental; // hp 역할
    [SerializeField] private AudioClip _cantSprint;
    [SerializeField] private AudioClip _walkFootSound;
    [SerializeField] private AudioClip _runFootSound;
    private AudioSource _playerAudioSource;
    public Action PlayerDead;
    
    private float _speed;
    private bool _isSprint;
    private float _usingStamina;
    private float _recoverStamina;
    private bool _isCantSprint;
    
    //inventory settings
    private Rigidbody rb;
    private float horizontalInput;
    private float verticalInput;
    private bool isNearItem = false;
    private Inventory inventory;
    private GameObject ActiveItem;
    [SerializeField] private InputEventSO inputEventSO;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] Transform HandTransform;
    
    
    //monster tag 
    private string _monsterHandTag;
    
    //monster timer
    private float _timer = 0f;
    private float _timer2 = 0f;
    //escape settings
    private int _escapeClickCount;
    private int _requiredEscapeClicks;
    [SerializeField] private bool _isPlayerStop = false;
    [SerializeField] private CinemachineFollow _camera;
    [SerializeField] private GameObject _cameraPos;
    float timeElapsed = 0f;
    private Vector3 _baseCamera;

    //카메라등 처음 Player만들때만 할당.
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventory = new Inventory();
        inventoryUI.InitInventory(inventory);
        inventory.OnActiveInventorySlotchanged += GenerateAndInitializeActiveItem;
        _monsterHandTag = "MonsterHand";
        _camera = GetComponentInChildren<CinemachineFollow>();
        _baseCamera = _cameraPos.transform.localPosition;
        _playerAudioSource = GetComponent<AudioSource>();
        init();
    }

    //리스폰시 초기화 작업 위해 카메라,오디오소스 등은 Awake함수로 이동.
    public void init()
    {
        _usingStamina = 10f;
        _stamina = 100f;
        _mental = 100f;
        _recoverStamina = 30f;
        _speed = 5f;
        _timer2 = 0f;
        _isPlayerStop = false;
        _escapeClickCount = 0;
        _requiredEscapeClicks = 10;
    }

    private void OnEnable()
    {
        inputEventSO.SubscribeMove(SetMove);
        inputEventSO.SubscribePickup(Detectitem);
        inputEventSO.SubscribeInteract(InteractItem);
        inputEventSO.SubscribeEscape(TryEscape);
        inputEventSO.SubscribeSprint(Sprint);
        StartCoroutine(PlayerSprint());
    }
    
    private void OnDisable()
    {
        inputEventSO.UnSubscribeMove(SetMove);
        inputEventSO.UnSubscribePickup(Detectitem);
        inputEventSO.UnSubscribeInteract(InteractItem);
        inputEventSO.UnSubscribeEscape(TryEscape);
        inputEventSO.UnSubscribeSprint(Sprint);
        StopCoroutine(PlayerSprint());
    }

    private void FixedUpdate()
    {
        _timer2 += Time.fixedDeltaTime;
        if (_isPlayerStop)
        {
            _timer += Time.fixedDeltaTime;
            //player는 움직일수 없는상태.
            PlayerView();
            if (_timer > 1f)
            {
                PlayerTakeDamage(5f);
                _timer = 0f;
            }
            return;
        }
        Movement();
    }

    private void ChangeSound(AudioClip clip)
    {
        if (_playerAudioSource.clip != clip)
        {
            _playerAudioSource.clip = clip;
            _playerAudioSource.Play();
        }
    }
    
    private IEnumerator PlayerSprint()
    {
        while (true)
        {
            if (_isSprint && _stamina>0f)
            {
                _stamina -= _usingStamina*Time.deltaTime;
                _stamina = Mathf.Max(_stamina, 0f);
                _speed = 10f;
                ChangeSound(_runFootSound);

                if (_stamina <= 0f)
                {
                    _speed = 0.5f;
                    ChangeSound(_cantSprint);
                    yield return new WaitForSecondsRealtime(3.0f);
                }
            }
            //!_isSprint || _stamina<=0
            else
            {
                _stamina += _recoverStamina*Time.deltaTime;
                _stamina = Mathf.Min(_stamina, 100f);
                _speed = 5f;
                ChangeSound(_walkFootSound);
            }

            yield return new WaitForSeconds(0.003f);
        }
    }
    
    private void Movement()
    {
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * _speed * Time.deltaTime;
        rb.MovePosition(transform.position + movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_monsterHandTag) == true)
        {
            _isPlayerStop = true;
        }
    }
    public void PlayerTakeDamage(float damage)
    {
        _mental -= damage;

        if (_mental <= 0)
        {
            PlayerDead?.Invoke();
        }
    }
    
    private void TryEscape()
    {
        // 잡혀있는 상태가 아니면 무시
        if (!_isPlayerStop) return;

        _escapeClickCount++;
        Debug.Log($"탈출 시도 중! ({_escapeClickCount} / {_requiredEscapeClicks})");

        // 요구 횟수를 채우면 탈출 성공
        if (_escapeClickCount >= _requiredEscapeClicks)
        {
            Debug.Log("탈출 성공!");
            _camera.FollowOffset = _baseCamera;
            _isPlayerStop = false;
            _escapeClickCount = 0; // 카운트 초기화
            _timer = 0f;           // 데미지 타이머 초기화
        }

    }

    private void PlayerView()
    {
        float t = timeElapsed / 0.5f;
        _camera.FollowOffset = Vector3.Lerp(_baseCamera, new Vector3(0f,0.88f,0.21f),t);
        timeElapsed += Time.deltaTime;
    }
    
    
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




        ActiveItem = Instantiate(item, HandTransform.position, Quaternion.identity);
        var collider = ActiveItem.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        ActiveItem.transform.SetParent(HandTransform);
    }



    private void InteractItem()
    {
        Debug.Log("Interacted with item");
    }

    private void Sprint(bool _isRun)
    {
        _isSprint =  _isRun;
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
    
    private ItemData Existitem()
    {
        RaycastHit hit;
        Vector3 startposition = transform.position;
        Vector3 halfextension = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 direction = transform.forward;
        Quaternion rotation = transform.rotation;
        float distance = 1f;


        if (Physics.BoxCast(transform.position, halfextension, direction, out hit, rotation, distance))
        {
            //TODO 이거원인찾기 그리고기즈모다시그리기 

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

}