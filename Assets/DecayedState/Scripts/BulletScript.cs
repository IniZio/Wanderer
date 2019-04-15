using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float shootForce = 100f;
	
	void OnEnable(){
		Invoke ("Destroy", 3f);
		this.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
	}
	void Destroy(){
		gameObject.SetActive (false);
	}
	void OnDisable(){
		CancelInvoke ();
	}
	void OnCollision(Collision col){
		gameObject.SetActive (false);
	}
}
