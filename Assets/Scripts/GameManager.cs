using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeGame = 60.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeGame -= Time.deltaTime;

        if (timeGame <= 0.0f)
        {
            TimeEnded();
        }
    }

    void TimeEnded()
    {

    }
}
