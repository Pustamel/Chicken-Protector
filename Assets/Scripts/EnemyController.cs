using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Image healthFilling;
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private AudioClip soundAttack;

    private bool carriesChicken;
    private NavMeshAgent navMesh;
    private Transform playerTarget;
    private GameObject player;
    private Chicken chicken = null;
    private Rigidbody rb;
    private float health = 100f;
    private float lastTimeAttack = 0f;
    private AudioSource audioSource;
    private GameManager gameManager;
    private SpawnManager spawnManager;
    private GameObject house;
    private GameObject enemyBase;
    private Animator animator;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = 5.5f;
        navMesh.angularSpeed = 120f;
        navMesh.stoppingDistance = 1.5f;
        navMesh.updateRotation = false;

        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        spawnManager = GameObject.FindWithTag("SpawnManager").GetComponent<SpawnManager>();
        house = GameObject.FindWithTag("PlayerBase");
        enemyBase = GameObject.FindWithTag("EnemyBase");
        animator = GetComponent<Animator>();

        gameObject.name = $"Enemy_{Random.Range(1000, 9999)}";
    }

    void Update()
    {
        if (lastTimeAttack > 0 && lastTimeAttack != 0)
        {
            lastTimeAttack -= (Time.deltaTime);
        }
    
        if (carriesChicken && chicken)
        {
            transform.LookAt(enemyBase.transform.position);
            GoToEnemyBase();
        }
        else if (playerTarget != null)
        {
            Vector3 targetPos = playerTarget.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
            GoToPlayer();
        }
        else
        {
            float distance = Vector3.Distance(transform.position, house.transform.position);

            if(distance > 3)
            {
                transform.LookAt(house.transform.position);
                navMesh.SetDestination(house.transform.position);
            }
        }
    }

    public void Damage(float damage)
    {

        health -= damage;
        healthFilling.fillAmount = (health / 100);
        ThrowChicken();

        if (health <= 0)
        {
            Dead();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyBase"))
        {
            KillChicken(other);
        }

        if (other.CompareTag("Player") && !carriesChicken)
        {
            playerTarget = other.transform;
            player = other.gameObject;
        }
        else if (other.CompareTag("Chicken") && !carriesChicken)
        {
            StealChicken(other);
        }
    }

    private void KillChicken(Collider other)
    {
        float distanceToEnemyBase = Vector3.Distance(transform.position, other.transform.position);
        bool sameEnemy = chicken?.thief == this;

        if (chicken && distanceToEnemyBase < 3 && !chicken.isDead && sameEnemy)
        {
            carriesChicken = false;
            chicken.Dead();
            chicken = null;

        }
    }

    private void ThrowChicken()
    {
        if(chicken != null && carriesChicken)
        {
            chicken.transform.SetParent(null, false);
            carriesChicken = false;
            chicken.Drop();
        }
    }

    private void GoToPlayer()
    {
        float distance = Vector3.Distance(transform.position, playerTarget.transform.position);

        if (distance > 2)
        {
            navMesh.SetDestination(playerTarget.position);

        }

        if(distance < 1.5 && (lastTimeAttack == 0 || lastTimeAttack < 0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        lastTimeAttack = 3.0f;
        audioSource.PlayOneShot(soundAttack, 0.1f);
        ParticleSystem effect = Instantiate(attackEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        Destroy(effect.gameObject, 2f);
        player.GetComponent<PlayerController>().Damage(15.0f);
    }

    private void GoToEnemyBase()
    {
        navMesh.SetDestination(enemyBase.transform.position);
    }

    private void StealChicken(Collider other)
    {
        chicken = other.GetComponent<Chicken>();
        float distance = Vector3.Distance(transform.position, other.transform.position);

        if (chicken != null && distance < 3 && !chicken.isCaptured())
        {
            carriesChicken = true;
            chicken.Take(this);
            chicken.transform.SetParent(transform, false);
            chicken.transform.localPosition = new Vector3(0, 2, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
        }
    }

    private void Dead()
    {
        spawnManager.DeleteSpawnedEnemy();
        Destroy(gameObject);
    }
}
