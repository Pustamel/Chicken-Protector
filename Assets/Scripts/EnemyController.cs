using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject house;
    [SerializeField] private GameObject enemyBase;
    private bool carriesChicken;
    private NavMeshAgent navMesh;
    private Transform playerTarget;
    private Chicken chicken = null;
    //private int health = 60;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.speed = 5.5f;
        navMesh.angularSpeed = 120f;
        navMesh.stoppingDistance = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTarget && !carriesChicken)
        {
            navMesh.SetDestination(playerTarget.position);
        } else if(carriesChicken)
        {
            GoToEnemyBase();
        }
        else
        {
            navMesh.SetDestination(house.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !carriesChicken)
        {
            playerTarget = other.transform;
        } else if (other.CompareTag("Chicken"))
        {   
            chicken = other.GetComponent<Chicken>();
            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (chicken != null && distance < 3)
            {
                carriesChicken = true;
                chicken.Take();
                chicken.transform.SetParent(transform, false);
                chicken.transform.localPosition = new Vector3(transform.position.x, 5, transform.position.z);
                //chicken.UpdatePosition(transform.position);
            }
        }
    }

    private void GoToEnemyBase()
    {
        navMesh.SetDestination(enemyBase.transform.position);
        if (chicken != null)
        {
            //chicken.UpdatePosition(transform.position);
            //chicken.SetParent
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTarget = null;
        }
    }
}
