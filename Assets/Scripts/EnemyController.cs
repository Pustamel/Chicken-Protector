using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    //[SerializeField] private GameObject house;
    //[SerializeField] private GameObject enemyBase;
    [SerializeField] private Image healthFilling;
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField] private AudioClip soundAttack;
    //[SerializeField] private SpawnManager spawnManager;

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
    }

    void Update()
    {
        if (lastTimeAttack > 0 && lastTimeAttack != 0)
        {
            lastTimeAttack -= (Time.deltaTime);
        }

        if (carriesChicken)
        {
            transform.LookAt(enemyBase.transform.position);
            GoToEnemyBase();
        }
        else if (playerTarget != null)
        {
            transform.LookAt(new Vector3(0, playerTarget.position.y, 0));
            GoToPlayer();
        }
        else
        {
            transform.LookAt(house.transform.position);
            navMesh.SetDestination(house.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyBase"))
        {
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (chicken && distance < 3)
            {
                chicken.Dead();
                carriesChicken = false;
                chicken = null;
            }
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

    private void ThrowChicken()
    {
        if(chicken != null & carriesChicken)
        {
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
            lastTimeAttack = 3.0f;
            audioSource.PlayOneShot(soundAttack, 0.1f);
            ParticleSystem effect = Instantiate(attackEffect, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            Destroy(effect.gameObject, 2f);
            player.GetComponent<PlayerController>().Damage(15.0f);
        }
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
            chicken.Take();
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
}
