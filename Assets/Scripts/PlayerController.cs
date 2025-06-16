using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction movingAction;
    private int health = 100;
    void Start()
    {
        movingAction = InputSystem.actions.FindAction("Move");  
    }

   
    void Update()
    {
        Vector2 moving = movingAction.ReadValue<Vector2>();
        float forwardInput = moving.y;
        float horizontalInput = moving.x;

        transform.Translate(Vector3.forward * Time.deltaTime * 12.0f * forwardInput);
        //transform.Rotate(Vector3.up, Time.deltaTime * 15.0f * horizontalInput);
        transform.Translate(Vector3.right * Time.deltaTime * 12.0f * horizontalInput);
    }
}
