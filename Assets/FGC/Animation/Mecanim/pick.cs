using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick : MonoBehaviour {


	void Start () {
		
	}

	void Update () {

	}
	void OnCollisionEnter(Collision collision){
		// var tag=collision.collider.tag;
		// if(tag=="Player"){
		// 	Destroy(this);
		// }
	}

	void OnCollisionExit(Collision collision){}

	void OnCollisionStay(Collision collision){}
}