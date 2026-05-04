using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class KeyMonster : BaseMonster
{
    private readonly int hashIsWalking = Animator.StringToHash("Walking");
    private readonly int hashIsRun = Animator.StringToHash("Run");
    private readonly int hashIsAttack = Animator.StringToHash("Attack");
    [SerializeField] private AudioSource _mainMonsterSFX;
    [SerializeField] private AudioClip _chaseClip;
    [SerializeField] private AudioClip _attackClip;
    [SerializeField] private AudioClip _mainClip;
    private Vector3 _monsterMove;
    private Vector3 _monsterWalkPos;
    // test 
    private float _posX;
    private float _posY;
    [SerializeField] private float walkstopDistance;

    // timer
    private float _waitTimer;
    private float _idleWaitTime;
    private float _attackTimer;
    private float _attackTime;
    
    private bool _isPlayerEscape;
    private Collider _monsterHandCollider;
    
    protected override void Init()
    {
        base.Init();

        _isPlayerEscape = false;
        
        //timer
        _attackTimer = 0f;
        _waitTimer = 0f;
        _idleWaitTime = 5f;
        _attackTime = 3f;
        //
        
        _runSpeed = 4f;
        _walkSpeed = 2f;
        _damage = 20f;
        _traceDist = 25f;
        _attackDist = 3f;
        _posX = Random.Range(-5f, 5f);
        _posY = Random.Range(-5f, 5f);
        _monsterMove = new Vector3(_posX,0f,_posY);
        _monsterWalkPos = this.transform.position + _monsterMove;
        walkstopDistance = 3f * 3f;
        _navMeshAgent.stoppingDistance = 0.5f;

        _monsterHandCollider = GetComponentInChildren<Collider>();
        ChangeState(MonsterState.IDLE);
        StartCoroutine(MonsterSetStateCo());
    }

    public bool IsPlayerEscape(bool escape)
    {
        _isPlayerEscape = escape;
        return _isPlayerEscape;
    }
    
    private IEnumerator MonsterSetStateCo()
         {
             while (true)
             {
                 CheckMonsterState();
                 switch (_monsterState)
                 {
                     case MonsterState.IDLE:
                         _waitTimer += Time.deltaTime;
                         if (_waitTimer >= _idleWaitTime && !_isPlayerEscape )
                         {
                             _navMeshAgent.SetDestination(SetMonsterDestination()); // 몬스터의 목적지 갱신 로직
                             ChangeState(MonsterState.WALKING);
                             _waitTimer = 0f;
                         }
                         break;
                     case MonsterState.WALKING:
                         float dist = (_monsterWalkPos - this.transform.position).sqrMagnitude;
                         //_navMeshAgent.remainingDistance 네브메시 남은거리 
                         if (dist <= walkstopDistance)  // 기본 idle 상태의 이동목표 지점 체크
                         {
                             ChangeState(MonsterState.IDLE);
                         }
                         break;
                     case MonsterState.TRACERUN:
                         /*NavMeshHit hit;
                         
                         if (NavMesh.SamplePosition(_playerTransform.position, out hit, 4f, NavMesh.AllAreas))
                         {
                             _navMeshAgent.SetDestination(hit.position);Debug.Log($"[성공] 네비메시 좌표 찾음: {hit.position}");
    
                             // 2단계 통과 확인: 해당 위치로 가는 경로가 실제로 이어져 있는지 확인
                             bool isPathValid = _navMeshAgent.SetDestination(hit.position);
                             if (!isPathValid)
                             {
                                 Debug.LogWarning("[실패] 좌표는 찾았지만 몬스터가 그곳으로 갈 수 없는 상태입니다 (길이 끊김 등).");
                             }
                         }
                         else
                         {
                            
                         }*/
         
                         _navMeshAgent.SetDestination(_playerTransform.position);
                         break;
                     case MonsterState.ATTACK:
                         

                         break;
                 }
                 yield return _ws;
             }
             yield return null;
         }
    
    private void CheckMonsterState()
    {
            float dist = (_playerTransform.position - this.transform.position).sqrMagnitude;
            if (dist < _attackDist * _attackDist && !_isPlayerEscape)
            {
                //this.transform.LookAt(_playerTransform);
                ChangeState(MonsterState.ATTACK);
            }
            else if (dist < _traceDist * _traceDist && !_isPlayerEscape)
            {
                //_monsterState = MonsterState.TRACERUN;
                ChangeState(MonsterState.TRACERUN);
            }
            else if( _isPlayerEscape ||  _monsterState != MonsterState.IDLE && _monsterState != MonsterState.WALKING && dist > _attackDist * _attackDist)
            {
                //_monsterState = MonsterState.IDLE;
                if (_isPlayerEscape)
                {
                    _attackTimer  += Time.deltaTime;
                    if (_attackTimer > _attackTime)
                    {
                        _isPlayerEscape = false;
                        _attackTimer = 0f;
                    }
                }
                ChangeState(MonsterState.IDLE);
            }
    }

    private void ChangeState(MonsterState newState)
    {
        if(_monsterState == newState) return;
        _monsterState = newState;
        switch (_monsterState)
        {
            case MonsterState.IDLE:
                _animator.SetBool(hashIsWalking, false);
                _animator.SetBool(hashIsRun, false);
                _animator.SetBool(hashIsAttack, false);
                _navMeshAgent.isStopped = true;
                _navMeshAgent.ResetPath();
                ChangedSFX(_mainClip);
                break;
            case MonsterState.WALKING:
                _animator.SetBool(hashIsRun, false);
                _animator.SetBool(hashIsAttack, false);
                _animator.SetBool(hashIsWalking, true);
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _walkSpeed;
                break;
            case MonsterState.TRACERUN:
                _animator.SetBool(hashIsWalking, false);
                _animator.SetBool(hashIsAttack, false);
                _animator.SetBool(hashIsRun, true);
                _navMeshAgent.isStopped = false;
                _navMeshAgent.speed = _runSpeed;
                ChangedSFX(_chaseClip);
                break;
            case MonsterState.ATTACK:
                _animator.SetBool(hashIsWalking, false);
                _animator.SetBool(hashIsRun, false);
                _animator.SetBool(hashIsAttack, true);
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
                ChangedSFX(_attackClip);
                break;
        }
    }
    
    private Vector3 SetMonsterDestination()
    {
        _posX = Random.Range(-10f, 10f);
        _posY = Random.Range(-10f, 10f);
        _monsterMove = new Vector3(_posX,0f,_posY);
        _monsterWalkPos =  this.transform.position + _monsterMove;
        return  _monsterWalkPos;
    }
    
    private void ChangedSFX(AudioClip clip)
    {
        if (_mainMonsterSFX.clip != clip)
        {
            _mainMonsterSFX.clip = clip;
            _mainMonsterSFX.Play();
        }
    }
    
}
