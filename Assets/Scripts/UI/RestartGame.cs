using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
    }

    void Update()
    {
        
    }

    private void Restart ()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        gameManager.StartGame();
    }
}
