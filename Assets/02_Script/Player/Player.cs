using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines.Interpolators;


public class Player : MonoBehaviour
{

    [SerializeField] private float _stamina; // mp 역할
    [SerializeField] private float _mental; // hp 역할
    [SerializeField] private float _maxMental = 100f; //최대 hp역할 
    [SerializeField] private AudioClip _cantSprint;
    [SerializeField] private AudioClip _walkFootSound;
    [SerializeField] private AudioClip _runFootSound;
    [SerializeField] private AudioClip _defaultSound;
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private CanvasGroup _inventoryGroup;
    [SerializeField] private GameObject _deadUI;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private AudioSource _hitAudioSource;
    [SerializeField] private AudioSource _openInventoryAudioSource;
    [SerializeField] private AudioSource _healAudioSource;
    private AudioSource _playerAudioSource;
    public Action PlayerDead;
    
    //player move & jump

    private bool isMoving;
    private float _speed;
    private bool _isSprint;
    private float _usingStamina;
    private float _recoverStamina;
    private PlayerState _playerState;
    private bool _isJump = false;
    [SerializeField] private LayerMask groundLayer;
    private bool _isDead;

    
    //player moveSettings
    private Rigidbody _rb;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector2 _rotationInput;
    private float _verticalRotation;
    private float _horizontalRotation;

    //inventory settings
    private bool isNearItem = false;
    private Inventory inventory;
    private ItemPickup ActiveItem;
    [SerializeField] private InputEventSO inputEventSO;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] Transform HandTransform;
    private ItemPickup exisitingItem;


    //monster tag 
    private string _monsterHandTag = "MonsterHand";
    private string _monster = "Monster";
    //monster timer
    private float _timer = 0f;

    //escape settings
    private int _escapeClickCount;
    private int _requiredEscapeClicks;
    [SerializeField] private bool _isPlayerStop = false;
    [SerializeField] private CinemachineFollow _camera;
    [SerializeField] private GameObject _cameraPos;
    float timeElapsed = 0f;
    private Transform _currMonsterTr;

    //monster
    private KeyMonster _keyMonster;
    //escape screen settings 
    private Vector3 _baseCamera;
    private CinemachineImpulseSource _impulseSource;
    //private Vector3 _monsterPos;
    private Vector3 _playerEscapeDir;
    //UI settings
    [SerializeField] public InGameUIManager uIManager;
    //HP settings
    public event Action<float, float> OnHpchanged;


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        inventory = new Inventory();
        inventoryUI.InitInventory(inventory);
        inventory.OnActiveInventorySlotchanged += GenerateAndInitializeActiveItem;
        _camera = GetComponentInChildren<CinemachineFollow>();
        _baseCamera = _cameraPos.transform.localPosition;
        _playerAudioSource = GetComponent<AudioSource>();
        init();
        HideInventory();
        inventory.OnslotChanged += HandleSlotChanged;
    }

    private void HandleSlotChanged(int activeitemindex, ItemInstance instance)
    {
        if (activeitemindex == inventory.ActiveIndex)
        {
            GenerateAndInitializeActiveItem(activeitemindex);
        }
    }

    //리스폰시 초기화 작업 위해 카메라,오디오소스 등은 Awake함수로 이동.
    public void init()
    {
        uIManager.InnitMentalUI();
        _isDead = false;
        _usingStamina = 10f;
        _stamina = 100f;
        _mental = _maxMental;
        _recoverStamina = 30f;
        _speed = 5f;
        _isPlayerStop = false;
        _escapeClickCount = 0;
        _requiredEscapeClicks = 5;
        _rotateSpeed = 10f;
        _verticalRotation = 0f;
        _camera = GetComponentInChildren<CinemachineFollow>();
        _baseCamera = _cameraPos.transform.localPosition;
        _playerAudioSource = GetComponent<AudioSource>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        inputEventSO.SubscribeJump(SetJump);
        inputEventSO.SubscribeLook(SetLook);
        inputEventSO.SubscribeMove(SetMove);
        inputEventSO.SubscribePickup(ItemKeyClicked);
        inputEventSO.SubscribeInteract(InteractItem);
        inputEventSO.SubscribeEscape(TryEscape);
        inputEventSO.SubscribeSprint(Sprint);
        inputEventSO.SubscribeMenu(OpenUI);
        inputEventSO.SubscribeInven(OpenInventory);
        GameManager.OnGameOver += StopPlayerAudio;
        StartCoroutine(PlayerSprint());
    }


    private void OnDisable()
    {
        inputEventSO.UnSubscribeJump(SetJump);
        inputEventSO.UnSubscribeLook(SetLook);
        inputEventSO.UnSubscribeMove(SetMove);
        inputEventSO.UnSubscribePickup(ItemKeyClicked);
        inputEventSO.UnSubscribeInteract(InteractItem);
        inputEventSO.UnSubscribeEscape(TryEscape);
        inputEventSO.UnSubscribeSprint(Sprint);
        inputEventSO.UnSubscribeMenu(OpenUI);
        inputEventSO.UnSubscribeInven(OpenInventory);
        GameManager.OnGameOver -= StopPlayerAudio;
        StopCoroutine(PlayerSprint());
    }
    
    private void FixedUpdate()
    {
        /* //테스트용시간에따른 멘탈감소 코드 
         _timer += Time.fixedDeltaTime;
         if (_timer > 1f)
         {
             PlayerTakeDamage(5f);
             _timer = 0f;
         }*/


        if (_isPlayerStop)
        {
            OffSound();
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
        PlayerRoation();
        PlayerSetState();
        PlayerJump();
    }

    public void RestoreHealth(float value)
    {
        if (!_isDead)
        {
            _mental = Mathf.Clamp(_mental + value, 0f, _maxMental);
            OnHpchanged?.Invoke(_mental, _maxMental);
        }
        _healAudioSource.Play();
    }

    private void OpenUI(bool _isPlayerOpenUI)
    {
        if (_isPlayerOpenUI)
        {
            GameManager.Instance.PauseGame();
            _inGameUI.SetActive(true);
        }
        else
        {
            GameManager.Instance.ResumeGame();
            _inGameUI.SetActive(false);
        }
    }

    private void OpenInventory(bool _isPlayerOpenInven)
    {
        if (_isPlayerOpenInven)
        {
            _openInventoryAudioSource.PlayOneShot(_openInventoryAudioSource.clip);
            ShowInventory();
        }
        else
        {
            _openInventoryAudioSource.PlayOneShot(_openInventoryAudioSource.clip);
            HideInventory();
        }
    }

    private void HideInventory()
    {
        _inventoryGroup.alpha = 0f;
        _inventoryGroup.interactable = false;
        _inventoryGroup.blocksRaycasts = false;
    }

    private void ShowInventory()
    {
        _inventoryGroup.alpha = 1f;
        _inventoryGroup.interactable = true;
        _inventoryGroup.blocksRaycasts = true;
    }

    private void ChangeSound(AudioClip clip)
    {
        if (_playerAudioSource.clip != clip)
        {
            _playerAudioSource.clip = clip;
            _playerAudioSource.Play();
        }
    }
    private void OffSound()
    {
        _playerAudioSource.Stop();
    }

    private void OnSound()
    {
        if (_playerAudioSource.isPlaying) return;
        _playerAudioSource.Play();
    }

    private void PlayerSetState()
    {
        if (_isSprint)
        {
            _playerState = PlayerState.SPRINT;
        }
        else if (isMoving)
        {
            _playerState = PlayerState.WALK;
        }
        else if (!isMoving && !_isSprint)
        {
             _playerState = PlayerState.IDLE;
        }
    }

    #region PlayerSprint
    private IEnumerator PlayerSprint()
    {
        while (true)
        {
            switch (_playerState)
            {
                case PlayerState.IDLE:
                    _stamina += _recoverStamina * Time.deltaTime;
                    _stamina = Mathf.Min(_stamina, 100f);
                    OffSound();
                    break;
                case PlayerState.WALK:
                    _stamina += _recoverStamina * Time.deltaTime;
                    _stamina = Mathf.Min(_stamina, 100f);
                    _speed = 5f;
                    OnSound();
                    ChangeSound(_walkFootSound);
                    break;
                case PlayerState.SPRINT:
                    if (_stamina > 0f)
                    {
                        OnSound();
                        _stamina -= _usingStamina * Time.deltaTime;
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
                    break;
                case PlayerState.DAMAGED:
                case PlayerState.DEAD:
                    break;

            }
            /*
            if (_isSprint && _stamina > 0f)
            {
                _stamina -= _usingStamina * Time.deltaTime;
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
                _stamina += _recoverStamina * Time.deltaTime;
                _stamina = Mathf.Min(_stamina, 100f);
                _speed = 5f;
                ChangeSound(_walkFootSound);
            }
            */
            yield return null;
        }
    }
    
    private void Sprint(bool _isRun)
    {
        _isSprint = _isRun;
    }

    #endregion





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_monsterHandTag) == true)
        {
            _isPlayerStop = true;
            Vector3 forward = other.transform.forward;
            forward.y = 0;
            forward.Normalize();
            _playerEscapeDir = forward * 3f;
        }

        if (other.CompareTag(_monster) == true)
        {
            _keyMonster = other.GetComponent<KeyMonster>();
            _currMonsterTr = other.transform;
            Debug.Log("monster is exist.");
        }
    }
    public void PlayerTakeDamage(float damage)
    {
        _mental = Mathf.Clamp(_mental - damage, 0f, _maxMental);
        OnHpchanged(_mental, _maxMental);
        _hitAudioSource.PlayOneShot(_hitAudioSource.clip);
        
        if (_mental <= 0)
        {
            _isDead = true;
            GameManager.Instance.GameOver();
            _inGameUI.SetActive(false);
            _deadUI.SetActive(true);
            StartCoroutine(PlayerDeadUIWait());
        }
    }
    
    private void StopPlayerAudio()
    {
        _playerAudioSource.Stop();
        _hitAudioSource.Stop();
    }

    private IEnumerator PlayerDeadUIWait()
    {
        yield return new WaitForSecondsRealtime(5f);
        _deadUI.SetActive(false);
        PlayerDead?.Invoke();
    }

    #region PlayerEscape
    private void TryEscape()
    {
        // 잡혀있는 상태가 아니면 무시
        if (!_isPlayerStop) return;

        _escapeClickCount++;
        _impulseSource.GenerateImpulse();
        // 요구 횟수를 채우면 탈출 성공
        if (_escapeClickCount >= _requiredEscapeClicks)
        {
            OnSound();
            _keyMonster.IsPlayerEscape(true);
            _rb.AddForce(_playerEscapeDir , ForceMode.Impulse);
            _camera.FollowOffset = _baseCamera;
            _isPlayerStop = false;
            timeElapsed = 0f;
            _escapeClickCount = 0; // 카운트 초기화
            _timer = 0f;           // 데미지 타이머 초기화
            _currMonsterTr = null;
        }

    }

    private void PlayerView()
    {
        float t = Mathf.Clamp01(timeElapsed / 0.5f);
        _camera.FollowOffset = Vector3.Lerp(_baseCamera, new Vector3(0f, 2.5f, 0f), t);

        if (_currMonsterTr != null)
        {
            Vector3 direction = (_currMonsterTr.position - transform.position).normalized;
            direction.y = 0f;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // Slerp를 사용해 부드럽게 몬스터 쪽으로 몸을 돌림
                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, Time.deltaTime * 5f));
            }

        }
        timeElapsed += Time.deltaTime;
    }


    #endregion

    #region Player move & rotaion & Jump
    private void Movement()
    {
        isMoving = _verticalInput != 0 || _horizontalInput != 0;
        Vector3 moveForward = transform.forward * _verticalInput;
        Vector3 moveRight = transform.right * _horizontalInput;
        Vector3 moveDirection = (moveForward + moveRight).normalized;
        Vector3 movement = moveDirection * _speed * Time.deltaTime;
        _rb.MovePosition(transform.position + movement);
    }
    
    private void PlayerRoation()
    {
        float mouseX = _rotationInput.x;
        float mouseY = _rotationInput.y;
        _horizontalRotation += mouseX * _rotateSpeed;
        _horizontalRotation %= 360f;
        _verticalRotation -= mouseY * _rotateSpeed * 0.05f;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -10f, 15f);
        Quaternion targetRotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0);
        _rb.MoveRotation(targetRotation);
    }

    private void PlayerJump()
    {
        if (_isJump)
        {
            if (IsGrounded())
            {
                _rb.AddForce(Vector3.up * 5f,ForceMode.Impulse);
                _isJump = false;
            }
        }
        else
        {
            return;
        }
    }
    bool IsGrounded()
    {
        // 캐릭터의 반지름만큼 구체를 아래로 쏴서 체크
        return Physics.CheckSphere(transform.position + Vector3.down * 0.9f, 0.2f, groundLayer);
    }



    private void SetJump(bool jump)
    {
        if (_isJump == jump)
        {
            return;
        }
        else
        {
            _isJump = jump;
        }
    }

    private void SetLook(Vector2 input)
    {
        _rotationInput.x = input.x * 0.1f;
        _rotationInput.y = input.y * 0.1f;
    }

    private void SetMove(Vector2 input)
    {
        _horizontalInput = input.x;
        _verticalInput = input.y;
        //   Debug.Log("Move Event Received: " + input);
    }
    #endregion

    #region PlayerItem
    public bool HasItem(string itemName)
    {
        return inventory.HasItem(itemName);
    }


    private void GenerateAndInitializeActiveItem(int activeslotIndex)
    {

        Debug.Log("generated");
        //이전에생성한 아이템이있으면 오브젝트풀에반환하고 이미반환해서반환할게없으면 스킵
        if (ActiveItem != null)
        {
            ItemPoolManager.Instance.Release(ActiveItem);
            ActiveItem = null;
        }
        //인덱스번호에해당하는아이템data로 아이템생성및 이전아이템있으면 없애는코드 

        if (inventory.items == null) return;

        if (inventory.items[activeslotIndex] == null) return;


        var item = inventory.items[activeslotIndex].ItemData;
        if (item == null) return;



        ActiveItem = ItemPoolManager.Instance.Get(item);

        ActiveItem.TurnOffLightEffect();



        ActiveItem.transform.SetPositionAndRotation(
         HandTransform.position,
         HandTransform.rotation * item.ItemPrefab.transform.rotation
         );

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
        Debug.Log("Interacted with item");
    }


    private void ItemKeyClicked()
    {
        Debug.Log("clicked");
        if (exisitingItem != null)
        {
            ItemPickup itemPickupData = exisitingItem;
            inventory.AddItem(new ItemInstance(itemPickupData.itemData));

            Debug.Log("Picked up: " + itemPickupData.itemData.ItemName);
            ItemPoolManager.Instance.Release(itemPickupData);
        }
    }
    private void Update()
    {
        exisitingItem = Existitem();
        ShowPickupUI();
    }

    private ItemPickup Existitem()
    {
        RaycastHit hit;
        Vector3 startposition = transform.position - transform.forward * 1f;
        Vector3 halfextension = new Vector3(1f, 1f, 1f);
        Vector3 direction = transform.forward;
        Quaternion rotation = transform.rotation;
        float distance = 2f;

        if (Physics.BoxCast(startposition, halfextension, direction, out hit, rotation, distance))
        {




            if (hit.collider.CompareTag("Item"))
            {
                ItemPickup itemPickupData = hit.collider.GetComponent<ItemPickup>();
                return itemPickupData;



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


    #endregion

    private void ShowPickupUI()
    {
        if (exisitingItem != null)
        {
            uIManager.OnPickUpUI(true);
        }
        else
        {
            uIManager.OnPickUpUI(false);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 center = transform.position - transform.forward * 1f;
        Vector3 halfExtents = new Vector3(1f, 1f, 1f);
        Quaternion rot = transform.rotation;
        Vector3 dir = transform.forward;
        float dist = 2f;


        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, rot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);


        Vector3 endCenter = center + dir * dist;
        Gizmos.color = Color.blue;
        Gizmos.matrix = Matrix4x4.TRS(endCenter, rot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2);


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(center, endCenter);


        // 바닥충돌 확인용
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down * 0.9f, 0.2f);
    }

}