using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyS : MonoBehaviour
{
    public float Gravity = 9.81f;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent; // NavMeshAgent component

    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
        _navMeshAgent.stoppingDistance = 1.0f; // Set stopping distance
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _navMeshAgent.SetDestination(player.transform.position);

            // Set animator parameters based on NavMeshAgent velocity
            if (_navMeshAgent.velocity.magnitude > 0.1f)
            {
                _animator.SetBool("IsMoving", true);
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }
        }
        else
        {
            Debug.LogWarning("Player object is not found!");
        }
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {

            // Handle collision with player
        }
    }

    void OnCollisionEnter(Collision S)
    {
        if(S.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("S Att");
        }
    }
    
    // Additional code for handling rotation
    void LateUpdate()
    {
        if (_navMeshAgent.velocity.magnitude > 0.1f)
        {
            // Rotate towards the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(_navMeshAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _navMeshAgent.angularSpeed);
        }
    }
}
