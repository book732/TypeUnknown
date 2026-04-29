using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeyMonster : BaseMonster
{
    private readonly int hashIsWalking = Animator.StringToHash("Walking");
    private readonly int hashIsRun = Animator.StringToHash("Run");
    private readonly int hashIsAttack = Animator.StringToHash("Attack");
    private Vector3 _monsterMove;
    private Vector3 _monsterWalkPos;

    private float _posX;
    private float _posY;
    [SerializeField] private float walkstopDistance;

    private float _timer;
    private float _idleWaitTime;
    protected override void Init()
    {
        base.Init();
        _timer = 0f;
        _idleWaitTime = 5f;
        _runSpeed = 2f;
        _walkSpeed = 1f;
        _damage = 20f;
        _traceDist = 20f;
        _attackDist = 2f;
        _posX = Random.Range(-5f, 5f);
        _posY = Random.Range(-5f, 5f);
        _monsterMove = new Vector3(_posX,0f,_posY);
        _monsterWalkPos = this.transform.position + _monsterMove;
        walkstopDistance = 3f * 3f;
        _navMeshAgent.stoppingDistance = 0.5f;

        ChangeState(MonsterState.IDLE);
        StartCoroutine(MonsterSetStateCo());
    }
    

    private IEnumerator MonsterSetStateCo()
    {
        while (true)
        {
            CheckMonsterState();
            switch (_monsterState)
            {
                case MonsterState.IDLE:
                    _timer += 0.1f;
                    if (_timer >= _idleWaitTime)
                    {
                        _navMeshAgent.SetDestination(SetMonsterDestination()); // 몬스터의 목적지 갱신 로직
                        ChangeState(MonsterState.WALKING);
                        _timer = 0f;
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
            if (dist < _attackDist * _attackDist)
            {
                ChangeState(MonsterState.ATTACK);
            }
            else if (dist < _traceDist * _traceDist)
            {
                //_monsterState = MonsterState.TRACERUN;
                ChangeState(MonsterState.TRACERUN);
            }
            else if(_monsterState != MonsterState.IDLE && _monsterState != MonsterState.WALKING && dist > _attackDist * _attackDist)
            {
                //_monsterState = MonsterState.IDLE;
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
                break;
            case MonsterState.ATTACK:
                _animator.SetBool(hashIsWalking, false);
                _animator.SetBool(hashIsRun, false);
                _animator.SetBool(hashIsAttack, true);
                _navMeshAgent.ResetPath();
                _navMeshAgent.isStopped = true;
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
}
