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
    public enum ChickenState
    {
        OnGround,
        BeingStolen
    }
    private ChickenState state;
    private int offsetY = 2;
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
            //Debug.Log($"DISTANCE {distance}");

            if (distance > 3)
            {
                navMesh.SetDestination(house.transform.position);
                animator.Play("walking");
                animator.speed = 3f;
            }
            else
            {
                navMesh.ResetPath();
                animator.Play("Idle");
                animator.speed = 1f;
            }
        }
        else
        {
            animator.Play("Idle");
            animator.speed = 1f;
        }
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y + 5, position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
      
    }

    public void Dead()
    {
        audioSource.PlayOneShot(deadSound, 0.2f);
        gameManager.Stolen();
        Destroy(gameObject, 0.5f);
    }

    public void Take()
    {
        state = ChickenState.BeingStolen;
        navMesh.enabled = false;
    }

    public bool isCaptured ()
    {
        return state == ChickenState.BeingStolen;
    }


    public void Drop()
    {
        state = ChickenState.OnGround;
        navMesh.enabled = true;
    }
}
