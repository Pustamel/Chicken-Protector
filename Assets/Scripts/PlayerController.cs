using System;
using System.Net.NetworkInformation;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float speed = 12f;

    private InputAction movingAction;
    private int health = 100;
    private float lastTimeAttack = 0f;
    private float simpleDamage = 25.0f;
    private AudioSource AudioSource;
    private Rigidbody rb;
    private CharacterController controller;
    private Vector3 playerVelocity;

    void Start()
    {
        movingAction = InputSystem.actions.FindAction("Move");
        AudioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
    }

   
    void Update()
    {
        if(lastTimeAttack > 0 && lastTimeAttack != 0)
        {
            lastTimeAttack -= (Time.deltaTime);
        }

        Vector2 moving = movingAction.ReadValue<Vector2>();

        float forwardInput = moving.y;
        float horizontalInput = moving.x;

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
        //bool groundedPlayer;

        //groundedPlayer = controller.isGrounded;
        //if (groundedPlayer && playerVelocity.y < 0)
        //{
        //    playerVelocity.y = 0f;
        //}

        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //move = Vector3.ClampMagnitude(move, 1f);

        //if (move != Vector3.zero)
        //{
        //    transform.forward = move;
        //}

        //playerVelocity.y += -9.81f * Time.deltaTime;

        //Vector3 finalMove = (move * speed) + (playerVelocity.y * Vector3.up);
        //controller.Move(finalMove * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(lastTimeAttack == 0 || lastTimeAttack < 0)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
            AudioSource.PlayOneShot(attackSound, 0.3f);
            lastTimeAttack = 3.0f;
            Instantiate(attackEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            DeteckAndDamageEnemies();
    }

    public void Damage(int damage)
    {
        health -= damage;
    }

    private void DeteckAndDamageEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f, enemyLayer);

        foreach (Collider hit in hits) { 

        GameObject gameObject = hit.gameObject;
        EnemyController enemy = gameObject.GetComponent<EnemyController>();

            if (enemy != null) { 
                enemy.Damage(simpleDamage);
            }
        }
    }
}
