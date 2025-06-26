using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Transform playerTransform;
    private Vector3 offsetCamera = new Vector3(-1f, 3f, 1f);
    private Vector3 rotateCamera = new Vector3(24f, 124f, 0f);
    void Start()
    {
        playerTransform = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        //transform.position = playerTransform.position + offsetCamera;
        //transform.rotation = playerTransform.rotation * Quaternion.Euler(rotateCamera);
    }
}
