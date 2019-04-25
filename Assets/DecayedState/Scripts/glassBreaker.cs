using UnityEngine;
using System.Collections;

public class glassBreaker : MonoBehaviour {
	public GameObject brokenGlass;
	public AudioClip[] breakSounds;
	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter(Collision coll){
		this.GetComponent<MeshRenderer> ().enabled = false;
		this.GetComponent<BoxCollider> ().enabled = false;
		GameObject brokenGlasPrefab = Instantiate(brokenGlass, transform.position, transform.rotation)as GameObject;
		brokenGlasPrefab.transform.localScale = this.transform.localScale;
		AudioSource source = gameObject.AddComponent<AudioSource>();
		source.clip = breakSounds[Random.Range(0,breakSounds.Length)];
		source.Play();
		Invoke("Destroy", 10); 
		brokenGlasPrefab.transform.parent = this.transform;
	}
	void Destroy(){
		Destroy(gameObject);
	}
}
