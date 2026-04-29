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
        _traceDist = 10f;
        _attackDist = 2f;
        _posX = Random.Range(-5f, 5f);
        _posY = Random.Range(-5f, 5f);
        _monsterMove = new Vector3(_posX,0f,_posY);
        _monsterWalkPos = this.transform.position + _monsterMove;
        walkstopDistance = 2f * 2f;

        _monsterState = MonsterState.IDLE;

        StartCoroutine(CheckMonsterStateCo());
        StartCoroutine(MonsterSetStateCo());
    }
    

    private IEnumerator MonsterSetStateCo()
    {
        while (true)
        {
            switch (_monsterState)
            {
                case MonsterState.IDLE:
                    _timer += 0.1f;
                    _animator.SetBool(hashIsWalking, false);
                    _animator.SetBool(hashIsRun, false);
                    _animator.SetBool(hashIsAttack, false);
                    
                    _navMeshAgent.isStopped = true;
                    _navMeshAgent.ResetPath();

                    if (_timer >= _idleWaitTime)
                    {
                        SetMonsterDestination();
                        _monsterState = MonsterState.WALKING;
                        Debug.Log("_monsterState is walking");
                        _timer = 0f;
                    }
                    break;
                case MonsterState.WALKING:
                    _animator.SetBool(hashIsWalking, true);
                    _animator.SetBool(hashIsRun, false);
                    _animator.SetBool(hashIsAttack, false);
                    _navMeshAgent.isStopped = false;
                    
                    float dist = (_monsterWalkPos - this.transform.position).sqrMagnitude;
                    
                    if (_navMeshAgent.remainingDistance <= walkstopDistance)
                    {
                        _monsterState = MonsterState.IDLE;
                        Debug.Log("_monsterState is idle");
                    }
                    break;
                case MonsterState.TRACERUN:
                    _animator.SetBool(hashIsWalking, false);
                    _animator.SetBool(hashIsAttack, false);
                    _animator.SetBool(hashIsRun, true);
                    _navMeshAgent.SetDestination(_playerTransform.position);
                    _navMeshAgent.speed = _runSpeed;
                    _navMeshAgent.isStopped = false;
                    break;
                case MonsterState.ATTACK:
                    _animator.SetBool(hashIsWalking, false);
                    _animator.SetBool(hashIsRun, false);
                    _animator.SetBool(hashIsAttack, true);
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.isStopped = true;
                //TODO: 잡기모션 추가 
                //TODO: 
                    break;
            }
            yield return _ws;
        }
        yield return null;
    }
    
    private IEnumerator CheckMonsterStateCo()
    {
        while (true)
        {
            float dist = (_playerTransform.position - this.transform.position).sqrMagnitude;
            if (dist < _attackDist * _attackDist)
            {
                _monsterState = MonsterState.ATTACK;
            }
            else if (dist < _traceDist * _traceDist)
            {
                _monsterState = MonsterState.TRACERUN;
            }
            else if(_monsterState != MonsterState.IDLE && _monsterState != MonsterState.WALKING && dist > _attackDist * _attackDist)
            {
                _monsterState = MonsterState.IDLE;
            }
            yield return _ws;
        }

        yield return null;
    }



    private void SetMonsterDestination()
    {
        _posX = Random.Range(-10f, 10f);
        _posY = Random.Range(-10f, 10f);
        _monsterMove = new Vector3(_posX,0f,_posY);
        _monsterWalkPos =  this.transform.position + _monsterMove;
        _navMeshAgent.SetDestination(_monsterWalkPos);
        _navMeshAgent.speed = _walkSpeed;
    }


    
}
