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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Pause()
    {
        gameManager.PouseGame();
    }
}
