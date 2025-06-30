using UnityEngine;
using UnityEngine.UI;

public class StartGameClick : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private Button buttonStart;
    void Start()
    {
        buttonStart = GetComponent<Button>();
        buttonStart.onClick.AddListener(StartGame);
    }

    void Update()
    {
        
    }

    private void StartGame()
    {
        gameManager.StartGame();
    }
}
