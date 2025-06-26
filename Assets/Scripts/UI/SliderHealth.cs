using UnityEngine;
using UnityEngine.UI;

public class SliderHealth : MonoBehaviour
{
    [SerializeField] private Slider slider;
    void Start()
    {
        slider.value = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
