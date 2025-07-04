using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private GameObject house;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ParticleSystem deadEffect;
    [SerializeField] private AudioClip deadSound;

    private NavMeshAgent navMesh;
    private AudioSource audioSource;
    private Animator animator;
  
    private ChickenState state;
    public bool isDead = false;
    public int num = 0;
    public EnemyController thief = null;
    public enum ChickenState
    {
        OnGround,
        BeingStolen
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        navMesh.updateRotation = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state != ChickenState.BeingStolen)
        {
            transform.LookAt(-house.transform.position);

            float distance = Vector3.Distance(transform.position, house.transform.position);

            if (distance > 3)
            {
                navMesh.SetDestination(house.transform.position);
                animator.SetBool("walking", true);
                animator.speed = 3f;
            }
            else
            {
                navMesh.ResetPath();
                animator.SetBool("walking", false);
                animator.speed = 1f;
            }
        }
        else
        {
            animator.SetBool("walking", false);
            animator.speed = 1f;
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y + 5, position.z);
    }

    public void Dead()
    {
        if(!isDead)
        {
        isDead = true;
        audioSource.PlayOneShot(deadSound, 0.2f);
        gameManager.Stolen();
        Destroy(gameObject, 0.5f);
        }
    }

    public void Take(EnemyController enemy)
    {
        state = ChickenState.BeingStolen;
        navMesh.enabled = false;
        thief = enemy;
    }

    public bool isCaptured ()
    {
        return state == ChickenState.BeingStolen || isDead;
    }


    public void Drop()
    {
        state = ChickenState.OnGround;
        navMesh.enabled = true;
        thief = null;
    }
}
