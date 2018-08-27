using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("onStart");
        Instantiate(PlayerUnitPrefab);
	}

    public GameObject PlayerUnitPrefab;
	
	// Update is called once per frame
	void Update () {
		
	}
}
