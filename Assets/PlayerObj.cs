using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerObj : NetworkBehaviour {
    public bool alive = true;
    public string name;
    public int life = 10;

	// Use this for initialization
	void Start () {
        if(isLocalPlayer) {
            Debug.Log("local Player, return");
            return;
        }

        Debug.Log("hihi");

        //  Instantiate(PlayerUnitPrefab);
    }

    public GameObject PlayerUnitPrefab;
	
	// Update is called once per frame
	void Update () {
		
	}

    bool IsAlive() {
        return this.alive;
    }

    void UpdateLife(int change) {
        this.life += change;
    }
}
