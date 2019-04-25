using UnityEngine;
using System.Collections;

public class FlightScript : MonoBehaviour 
{
	public float moveSpeed;
	public float rotSpeed;
	public bool gliderBroken;

	private float deadZone = .1f;

	private GameObject objPlayer;//Player
	private CharacterControl CharCtrlScript;
	
	// Use this for initialization
	void Start () 	{
		objPlayer = (GameObject) GameObject.FindWithTag ("Player");
		CharCtrlScript = (CharacterControl) objPlayer.GetComponent( typeof(CharacterControl) );
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void FixedUpdate()	{
		if (CharCtrlScript.gliding && CharCtrlScript.currGlide == this.gameObject && !gliderBroken) {
						GetLocomotionInput ();	
						if (!GetComponent<AudioSource>().isPlaying) {
								GetComponent<AudioSource>().Play ();
						}
				} else {
				GetComponent<AudioSource>().Stop();
				}
	}
	
	void GetLocomotionInput(){
		float gravityMultplr = (100 / moveSpeed)*2;
		this.GetComponent<Rigidbody>().AddForce(Vector3.up * -moveSpeed*gravityMultplr);
		if (Input.GetAxisRaw("Vertical")>0){
			moveSpeed = Mathf.Lerp(moveSpeed, 500, Time.deltaTime*0.5f);
			GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, 2f, Time.deltaTime * 0.5f);
		}
		else if (Input.GetAxisRaw("Vertical")<0){
			moveSpeed = Mathf.Lerp(moveSpeed, 1, Time.deltaTime);
			GetComponent<AudioSource>().pitch = Mathf.Lerp(GetComponent<AudioSource>().pitch, 0.5f, Time.deltaTime * 0.5f);
		}
		else{
			moveSpeed = Mathf.Lerp(moveSpeed, 10, Time.deltaTime*0.1f);
		}
		
		Vector3 ahead = this.transform.forward;
		this.GetComponent<Rigidbody>().AddForce(ahead * moveSpeed);

		if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone){ 
			this.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.forward * -rotSpeed*2 * Input.GetAxis("Horizontal"));
			GetComponent<Rigidbody>().AddRelativeTorque(Vector3.up * rotSpeed * Input.GetAxis("Horizontal"));

		}
		if (Input.GetAxisRaw("Vertical") > deadZone || Input.GetAxis("Vertical") < -deadZone){
			this.GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right * rotSpeed/4 * Input.GetAxis("Vertical"));
		}
		Quaternion _lookRotation = Quaternion.LookRotation(ahead);
		_lookRotation.z = 0;
		_lookRotation.x = 0;		
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 2f);
		GetComponent<Rigidbody>().velocity = ahead*10;

	}
	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name != "Player" && moveSpeed!=10){
			CharCtrlScript.GetOfGlider ();
			gliderBroken = true;
		}
	}
}