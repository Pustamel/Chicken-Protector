using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerElement;
    [SerializeField] private TextMeshProUGUI welcomeMessage;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Color eveningColor = new Color(1f, 0.5f, 0.2f);
    [SerializeField] private Color morningColor = new Color(1f, 1f, 0.8f);
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menuPaused;
    [SerializeField] private GameObject menuGameOver;
    [SerializeField] private GameObject menuWin;
    [SerializeField] private ParticleSystem winEffect;

    static private readonly float totalTimeGame = 60.0f;

    public bool isGaming = false;
    public bool gameOver = false;
    public bool paused = false;
    private float timeGame = totalTimeGame;
    private ChickenCount chickenCount;
    private float elapsedTime = 0f;

    void Start()
    {
        chickenCount = GetComponent<ChickenCount>();
    }

    void Awake()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
       if(isGaming)
        {
            Gaming();
        }
    }

    public void StartGame()
    {
        menu.SetActive(false);
        welcomeMessage.enabled = true;
        Time.timeScale = 1;
        isGaming = true;
    }



    public void GameOver()
    {
        gameOver = true;
        TimeEnded();
        menuGameOver.SetActive(true);
    }

    public void PouseGame()
    {
        if (isGaming == false)
        {
            Time.timeScale = 1;
            isGaming = true;
            menuPaused.SetActive(false);
        } else
        {
            Time.timeScale = 0;
            isGaming = false;
            menuPaused.SetActive(true);
        }

    }

    public void Stolen()
    {
        if(!gameOver)
        {
            chickenCount.RemoveChicken();
            gameOver = chickenCount.AllChickensDead();

            if(gameOver == true)
            {
                GameOver();
            }
        }
    }

    private void Win()
    {
        if (!gameOver)
        {
            ParticleSystem effect = Instantiate(winEffect, Vector3.zero, Quaternion.identity);
            Destroy(effect, 2f);
            Invoke("OpenWinMenu", 2f);
        }
    }

    private void OpenWinMenu()
    {
        TimeEnded();
        menuWin.SetActive(true);
    }
    private void TimeEnded()
    {
        isGaming = false;
        Time.timeScale = 0;
    }

    private void NightToMorning()
    {
        if (elapsedTime < totalTimeGame)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / totalTimeGame;
            float tIntensity = elapsedTime / 10f;

            directionalLight.intensity = Mathf.Lerp(1f, 2f, tIntensity);

            directionalLight.color = Color.Lerp(eveningColor, morningColor, t);
            directionalLight.intensity = Mathf.Lerp(0.3f, 1f, t);
            directionalLight.transform.rotation = Quaternion.Euler(Mathf.Lerp(120f, 30f, t), -30f, 0);
        }
    }

    private void Gaming()
    {
        if (timeGame < (totalTimeGame - 2f))
        {
            welcomeMessage.enabled = false;
        }

        if (!gameOver && timeGame > 0)
        {
            timeGame -= Time.deltaTime;
            timerElement.text = Mathf.CeilToInt(timeGame).ToString();

            if (timeGame <= 0.0f)
            {
                Win();
                Invoke("TimeEnded", 2f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PouseGame();
        }

        NightToMorning();
    }
}
