using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Restart);
    }

    private void Restart ()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        gameManager.StartGame();
    }
}
