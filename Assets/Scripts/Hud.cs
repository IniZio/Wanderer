using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hud : MonoBehaviour
{
    public GameObject HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(60);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int health)
    {
        int i = 0;
        foreach (GameObject block in GameObject.FindGameObjectsWithTag("HealthBlock"))
        {
            block.SetActive(health >= (i * 10));
            i++;
        }
    }
}
