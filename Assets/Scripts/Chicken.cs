using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private GameObject house;
    [SerializeField] private GameManager gameManager;
    private NavMeshAgent navMesh;
    public enum ChickenState
    {
        OnGround,
        BeingStolen
    }
    private ChickenState state;
    private int offsetY = 2;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
    }

    public void UpdatePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y + 5, position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(state != ChickenState.BeingStolen)
        {
            navMesh.SetDestination(house.transform.position);
        }
    }

    public void Dead()
    {
        gameManager.Stolen();
        Destroy(gameObject);
    }

    public void Take()
    {
        state = ChickenState.BeingStolen;
        navMesh.enabled = false;
    }

    public void Drop()
    {
        state = ChickenState.OnGround;
        navMesh.enabled = true;
    }
}
