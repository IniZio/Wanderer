using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp : MonoBehaviour {
    public SimpleHealthBar healthBar;

    private int health = 100;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void Increase () {
        health += 10;
        healthBar.UpdateBar(health, 100);
    }

    public void Decrease() {
        health -= 10;
        healthBar.UpdateBar(health, 100);
    }
}
