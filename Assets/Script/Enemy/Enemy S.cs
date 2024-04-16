using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Gravity = 9.81f;
    private CharacterController _characterController;
    private Animator _animator; // Animator component

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f; // Ignore vertical difference

            if (direction.magnitude > 0.1f)
            {
                // Calculate the rotation needed to look at the player
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                // Smoothly rotate towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            direction.Normalize();

            Vector3 movement = direction * Speed * Time.deltaTime;

            movement.y -= Gravity * Time.deltaTime;

            _characterController.Move(movement);

            // Set animator parameters based on movement
            if (movement.magnitude > 0)
            {
                _animator.SetBool("IsMoving", true); // Set IsMoving parameter to true
            }
            else
            {
                _animator.SetBool("IsMoving", false); // Set IsMoving parameter to false
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
            // You can add code here for when the enemy collides with the player
        }
    }
}
