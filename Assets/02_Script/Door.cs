using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _isOpen;
    private void Awake()
    {
        _rb =GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //if key를 가지고있음이 확인 되어 true
        if (_isOpen)
        {
            MoveDoor();
        }
        else
        {
            
        }
        return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if() player key를 가지고있는지
            Player player = other.GetComponent<Player>();
            if (player != null && player.HasItem("Key"))
            {
                _isOpen = true;
            }
        }
    }

    
    
    
    private void MoveDoor()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
        // 현재 회전에서 목표 회전까지 0.1의 속도로 부드럽게 보간
        Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, 0.01f);
        _rb.MoveRotation(smoothRotation);
    }
    
    
}
