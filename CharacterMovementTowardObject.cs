using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementTowardObject : MonoBehaviour
{
    public float Speed = 5.0f;
    public float Gravity = 9.81f;
    private CharacterController _characterController;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();

            direction.y = 0f;

            Vector3 movement = direction * Speed * Time.deltaTime;

            movement.y -= Gravity * Time.deltaTime;

            _characterController.Move(movement);
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
            Debug.Log("Collided with Player!");
        }
    }
}


