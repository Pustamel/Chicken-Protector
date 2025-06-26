using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChickenCount : MonoBehaviour
{
    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private Transform panel;
    private List<GameObject> chickens = new List<GameObject>();
    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject chicken = Instantiate(imagePrefab, panel);
            chicken.GetComponent<RectTransform>().sizeDelta = new Vector2(32, 32);
            chickens.Add(chicken);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AllChickensDead()
    {
        return chickens.Count == 0;
    }

    public void RemoveChicken()
    {
        if(chickens.Count > 0)
        {
            Destroy(chickens[0]);
            chickens.RemoveAt(0);
        }
    }
}
