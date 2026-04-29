using System;
using UnityEngine;
using UnityEngine.AI;


public class BaseMonster : MonoBehaviour
{
    [SerializeField] protected float _damage = 0;
    [SerializeField] protected float _walkSpeed = 0;
    [SerializeField] protected float _runSpeed = 0;
    [SerializeField] protected Transform _playerTransform;
    
    [SerializeField] protected float _traceDist = 0;
    [SerializeField] protected float _attackDist = 0;
    
    protected WaitForSeconds _ws;

    
    protected MonsterState _monsterState;
    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    
    private void Awake()
    {
        Init();
    }

    protected virtual void TakeDamage(float damage)
    {
        
    }
    
    protected virtual void Init()
    {
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        if (playerobj != null)
        {
            _playerTransform = playerobj.transform;
        }
        else 
        {
            Debug.LogError("No player found");
        }
        _animator = GetComponent<Animator>();
        _navMeshAgent  = GetComponent<NavMeshAgent>();
        _ws = new WaitForSeconds(0.016f);
    }
}
