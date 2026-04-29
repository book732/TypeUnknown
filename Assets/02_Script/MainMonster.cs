using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MainMonster : BaseMonster
{
    private readonly int hashFindPlayer = Animator.StringToHash("FindPlayer");
    private readonly int hashIsAttack = Animator.StringToHash("IsAttack");
    [SerializeField] private Transform StartPoint;

    private void OnEnable()
    {
        StartCoroutine(MonsterState());
        StartCoroutine(MonsterAction());
    }
    
    protected override void Init()
    {
        base.Init();
        _damage = 10f;
        _walkSpeed = 6f;
        _traceDist = 55f;
        _attackDist = 2f;
    }

    private IEnumerator MonsterState()
    {
        while (true)
        {
            float distance = (transform.position - _playerTransform.position).sqrMagnitude;
            if (distance <= _attackDist * _attackDist)
            {
                _monsterState = global::MonsterState.ATTACK;
            }
            else if (distance <= _traceDist * _traceDist)
            {
                _monsterState = global::MonsterState.TRACERUN;
            }
            else
            {
                _monsterState = global::MonsterState.WALKING;
            }
            yield return _ws;
        }
    }

    private IEnumerator MonsterAction()
    {
        while (true)
        {
            switch (_monsterState)
            {
                case global::MonsterState.IDLE:
                    _navMeshAgent.isStopped = true;
                    break;
                case global::MonsterState.TRACERUN:
                    _navMeshAgent.isStopped = false;
                    _navMeshAgent.SetDestination(_playerTransform.position);
                    _animator.SetBool(hashIsAttack,false);
                    _animator.SetTrigger(hashFindPlayer);
                    break;
                case global::MonsterState.ATTACK:
                    _animator.SetBool(hashIsAttack,true);
                    _navMeshAgent.isStopped = true;
                    break;
                case global::MonsterState.WALKING:
                    _navMeshAgent.SetDestination(StartPoint.position);
                    _animator.ResetTrigger(hashFindPlayer);
                    _animator.SetBool(hashIsAttack,false);
                    break;
            }
            yield return _ws;
        }
    }
}
