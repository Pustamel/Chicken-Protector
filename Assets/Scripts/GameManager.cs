using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeGame = 60.0f;
    private ChickenCount chickenCount;
    public bool isGaming = true;
    public bool gameOver = false;
    void Start()
    {
        chickenCount = GetComponent<ChickenCount>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            timeGame -= Time.deltaTime;

            if (timeGame <= 0.0f)
            {
                TimeEnded();
            }
        }
    }

    void TimeEnded()
    {

    }

    public void Stolen()
    {
        if(!gameOver)
        {
            chickenCount.RemoveChicken();
            gameOver = chickenCount.AllChickensDead();

        }
    }
}
