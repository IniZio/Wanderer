using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hp : MonoBehaviour {
    public SimpleHealthBar healthBar;

    private float health = 100;

    // Use this for initialization
    void Start () {
		
	}
	
	public void Increase (float amount = 10) {
        healthBar.UpdateBar(health += amount, 100);
    }

    public void Decrease(float amount = 10) {
        healthBar.UpdateBar(health -= amount, 100);
    }
}
