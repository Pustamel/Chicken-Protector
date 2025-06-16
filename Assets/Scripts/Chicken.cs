using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] private GameObject house;
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

    // Update is called once per frame
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
            //navMesh.SetDestination(house.transform.position);
        }
    }

    public void Take()
    {
        state = ChickenState.BeingStolen;
    }

    public void Drop()
    {
        state = ChickenState.OnGround;
    }
}
