using UnityEngine;
using UnityEngine.InputSystem;

public class RotationCamera : MonoBehaviour
{
    [SerializeField] private float sensitivity = 100f;
    private float yaw = 0f;
    private PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        yaw += mouseX * sensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(32f, yaw, 0f);
        player.transform.rotation = Quaternion.Euler(transform.rotation.x, yaw, 0f);
    }
}
