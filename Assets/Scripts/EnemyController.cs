using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject enemyBase;
    [SerializeField] private Image healthFilling;

    private bool carriesChicken;
    private NavMeshAgent navMesh;
    private Transform playerTarget;
    private Chicken chicken = null;
    private Rigidbody rb;
    private float health = 100f;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = 5.5f;
        navMesh.angularSpeed = 120f;
        navMesh.stoppingDistance = 1.5f;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (carriesChicken)
        {
            GoToEnemyBase();
        }
        else if (playerTarget != null)
        {
            GoToPlayer();
        }
        else
        {
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
    }

    private void GoToEnemyBase()
    {
        navMesh.SetDestination(enemyBase.transform.position);
    }

    private void StealChicken(Collider other)
    {
        chicken = other.GetComponent<Chicken>();
        float distance = Vector3.Distance(transform.position, other.transform.position);

        if (chicken != null && distance < 3)
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
