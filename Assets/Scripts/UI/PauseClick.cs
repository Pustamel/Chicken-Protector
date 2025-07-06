using UnityEngine;
using UnityEngine.UI;

public class PauseClick : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Pause);
    }

    private void Pause()
    {
        gameManager.PouseGame();
    }
}
