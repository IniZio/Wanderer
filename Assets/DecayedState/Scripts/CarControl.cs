using UnityEngine;
using System.Collections;
//CAR CONTROLL SCRIPT BASED ON  Andrew Gotow's TUTORIAL (http://www.gotow.net/andrew/blog/?page_id=78) please support him
public class CarControl : MonoBehaviour {
	private Animator _animator;

	public WheelCollider FrontLeftWheel;//front left wheel collider
	public WheelCollider FrontRightWheel;//front right wheel collider

	public bool openDoor = false;
	public bool engineStopped;
	public bool lowDoors;
	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public float[] GearRatio ;//array of gear ratios
	public ParticleSystem[] fumes;
	public int CurrentGear = 0;//our current gear
	public int minEmission;
	public int maxEmission;
	public float EngineTorque = 600f;
	public float MaxEngineRPM =  1000f;//maximum rotation per minute
	public float MinEngineRPM = 500f;//minimum rotation per minute
	public float zRotation = 3f;//speed of steering whell rotation
	public float centerOfMassY;

	public AudioClip engineStart;
	public AudioClip engineIdle;
	public AudioClip engineStop;

	public float EngineRPM = 0f;//starting rotation per minute
	private GameObject objPlayer;//Player
	private CharacterControl CharCtrlScript;
	// Use this for initialization
	void Start () {
		FrontLeftWheel.brakeTorque = 300;
		FrontRightWheel.brakeTorque = 300;
///////CACHING VARIABLES
		foreach (ParticleSystem PS in fumes) {
			PS.enableEmission = false;
			PS.emissionRate = minEmission;
		} 
		_animator = GetComponent<Animator>();
		objPlayer = (GameObject) GameObject.FindWithTag ("Player");
		CharCtrlScript = (CharacterControl) objPlayer.GetComponent( typeof(CharacterControl) );
		GetComponent<Rigidbody>().centerOfMass = new Vector3(GetComponent<Rigidbody>().centerOfMass.x, centerOfMassY, GetComponent<Rigidbody>().centerOfMass.z);//changing car's center of mass
	//if U'll be using another car's model play with these values to fit Your needs
	}

	// Update is called once per frame
	void Update () {
		if ((CharCtrlScript.driving) && (CharCtrlScript.currCar == this.gameObject)) {
			if (!GetComponent<AudioSource>().isPlaying){
				StartCoroutine(EngineStart());
			}
				foreach (ParticleSystem PS in fumes) {
								PS.enableEmission = true;
					}
						if (Input.GetAxis ("Vertical") != 0) {
							foreach (ParticleSystem PS in fumes) {
									PS.emissionRate = Mathf.Lerp (PS.emissionRate, maxEmission, Time.deltaTime * 10);
								}

						} else {
								foreach (ParticleSystem PS in fumes) {
										PS.emissionRate = Mathf.Lerp (PS.emissionRate, minEmission, Time.deltaTime * 10);
								}
						}
						//check if we're riding a car, AND a car is actually THIS car
						GetComponent<Rigidbody>().drag = GetComponent<Rigidbody>().velocity.magnitude / 50;//lets decrease the drag (http://docs.unity3d.com/Documentation/ScriptReference/Rigidbody-drag.html)	
						EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm) / 2;//calculate engine rotation per minute
			//engine Sound control
						GetComponent<AudioSource>().pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1f ;//audio source pitch value is calculated basing on rounded division of current
						//rotation per minute and max rotation per minute +1, so this value will never be lesser than 1 (standart pitch value)
						//but will be increased if rotation per minute is increased
						if (GetComponent<AudioSource>().pitch > 2f ) {//don't make pitch value greater than 2
							GetComponent<AudioSource>().pitch = 2f;
						}
						FrontLeftWheel.motorTorque = EngineTorque / GearRatio [CurrentGear] * Input.GetAxis ("Vertical");// multiply the torque on the wheels by the input value
						FrontRightWheel.motorTorque = EngineTorque / GearRatio [CurrentGear] * Input.GetAxis ("Vertical");// multiply the torque on the wheels by the input value
						//wheels' steering angle based on player's input		
						FrontLeftWheel.steerAngle = 25 * Input.GetAxis ("Horizontal");
						FrontRightWheel.steerAngle = 25 * Input.GetAxis ("Horizontal");

						if (Input.GetButtonDown ("Jump")) {//if we press space bar
								//lets stop the car 
								FrontLeftWheel.brakeTorque = 300;
								FrontRightWheel.brakeTorque = 300;
						}
						if (Input.GetButtonUp ("Jump")) {//if we release space bar
								//lets zero brakeTorque values of the wheels so we can move
								FrontLeftWheel.brakeTorque = 0;
								FrontRightWheel.brakeTorque = 0;
						}
				} else {
			foreach (ParticleSystem PS in fumes) {
				PS.enableEmission = false;
			}
		}
		if(engineStopped){
				StartCoroutine(EngineStop());
		}
	}
	void FixedUpdate(){
		_animator.SetBool ("Open", openDoor);				
	}
	IEnumerator EngineStart(){
		engineStopped = false;
		GetComponent<AudioSource>().clip = engineStart;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length-0.3f);
		GetComponent<AudioSource>().clip = engineIdle;
		GetComponent<AudioSource>().Play();
	}
	IEnumerator EngineStop(){
		FrontLeftWheel.brakeTorque = 300;
		FrontRightWheel.brakeTorque = 300;
		engineStopped = false;
		GetComponent<AudioSource>().clip = engineStop;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length-0.3f);
		GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().clip = null;

	}
}

