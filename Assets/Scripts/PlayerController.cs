using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float speed = 12f;
    [SerializeField] private Image healthFilling;
    [SerializeField] private GameManager gameManager;

    private InputAction movingAction;
    private float health = 100f;
    private float lastTimeAttack = 0f;
    private float simpleDamage = 25.0f;
    private AudioSource AudioSource;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Animator animator;
    private float timeReloadAttack = 1.5f;

    void Start()
    {
        movingAction = InputSystem.actions.FindAction("Move");
        AudioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(lastTimeAttack > 0 && lastTimeAttack != 0)
        {
            lastTimeAttack -= (Time.deltaTime);
        }

        MovePlayer();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(lastTimeAttack == 0 || lastTimeAttack < 0)
            {
                Attack();
            }
        }
    }
    public void Damage(float damage)
    {
        health -= damage;
        healthFilling.fillAmount = (health / 100);

        if (health <= 0)
        {
            Dead();
        }
    }

    private void MovePlayer()
    {
        Vector2 moving = movingAction.ReadValue<Vector2>();
        bool groundedPlayer;

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 move = Camera.main.transform.TransformDirection(input);

        input = Vector3.ClampMagnitude(input, 1f);

        move.y = 0f;
        move.Normalize();

        animator.SetBool("Run", move != Vector3.zero);

        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        playerVelocity.y += -9.81f * Time.deltaTime;

        Vector3 finalMove = (move * speed) + (playerVelocity.y * Vector3.up);
        controller.Move(finalMove * Time.deltaTime);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        AudioSource.PlayOneShot(attackSound, 0.3f);
        lastTimeAttack = timeReloadAttack;
        Instantiate(attackEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        DeteckAndDamageEnemies();
    }

    private void Dead()
    {
        gameManager.GameOver(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HealingPlace"))
        {
            health = 100;
            healthFilling.fillAmount = 1; //update ui
        }
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
