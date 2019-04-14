using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
	private Animator _animator;

	public bool breakDoorForward;
	public bool breakDoorBackward;
	public bool closedDoor;

	public AudioClip doorBreak;
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		closedDoor = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void FixedUpdate () {
		_animator.SetBool("breakDoorForward", breakDoorForward);
		_animator.SetBool("breakDoorBackward", breakDoorBackward);
		_animator.SetBool("closedDoor", closedDoor);
	}
	public void DoorSound()
	{
		if (!GetComponent<AudioSource>().isPlaying) {
						StartCoroutine (CoDoorSound ());
				}
	}
	private IEnumerator CoDoorSound(){
		GetComponent<AudioSource>().clip = doorBreak;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
		GetComponent<AudioSource>().Stop();
	}
}
