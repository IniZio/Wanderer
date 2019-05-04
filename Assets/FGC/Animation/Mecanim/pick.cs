using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick : MonoBehaviour {


	void Start () {
		
	}

	void Update () {

	}
	void OnTriggerEnter(Collider col){
		var tag=col.gameObject.tag;
		Debug.Log("col");
		 if(tag=="Player"){
			PlayerControl script=col.gameObject.GetComponent("PlayerControl") as PlayerControl;
					Debug.Log("player");
			script.pick(this.gameObject);
		 }
	}

	void OnCollisionExit(Collision collision){}

	void OnCollisionStay(Collision collision){}
}