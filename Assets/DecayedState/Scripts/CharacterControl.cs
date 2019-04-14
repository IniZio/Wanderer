using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControl : MonoBehaviour {
	public Animator _animator;
	private CharacterController _charCtrl;
	private bool _canSlideToCover;
	private bool _crouch;
	public bool _run;
	private bool _kickDoor;
	private bool _jump;
	private bool _jumpOverObstcle;
	private bool _jumpUpHigh;
	private bool _stop;
	private bool _stopBack;
	private bool _coverRolling;
	private bool _getInCar;
	private bool _getInLowCar;
	private bool _glider;
	private bool canRotate = true;
	private int doorSide=1;

	public bool chopTree;
	public bool driving;
	public bool gliding;
	public bool usingRifle;
	public bool usingAxe;
	public bool atWall;
	public bool rifleAiming;
	public bool atTheLeftCorner;
	public bool atTheRightCorner;

	private Vector3 moveDirection = Vector3.zero;//player's move direction

	public float _speed;
	public float currentWallAngleY;
	public float gravity = 3f;//gravity force 
	private float rotY;
	private float AimYSpeed =4;
	private float t = 0f;
	private float x = 0f;
	private float _strafe;
	private float _distToGround;

	public GameObject currDoor;
	public GameObject currCar;
	public GameObject currGlide;
	public GameObject carCamPoint; 
	public GameObject currentWall;//THE wall we'll hide at

	private Transform axeHandPosistion;
	private Transform shotgunHandPosition;
	private Transform axeHolster;
	private Transform shotgunHolster;

	//WEAPON SECTION

	/// assign all the values in the inspector

	public WeaponInfo currentWeapon;
	public List<WeaponInfo> WeaponList = new List<WeaponInfo>();
	[System.Serializable]
	public class WeaponInfo{
		public string weaponName = "veapon name";
		public float fireRate = 0.1f;
		public Transform weaponTransform;
		public Transform weaponMuzzlePoint;
		public float weaponKickBack;
		public GameObject bulletPrefab;
		public int totalAmmo;
		public int magazine;
	}

	static int SlideToCoverState = Animator.StringToHash("Base Layer.SprintSlideToCower");
	// Use this for initialization
	void Start () {
		_distToGround = GetComponent<Collider>().bounds.extents.y;
		_animator = GetComponent<Animator>();
		_charCtrl = GetComponent<CharacterController>();
		axeHandPosistion = GameObject.Find("axeMountPoint").transform;
		shotgunHandPosition = GameObject.Find("shotgunMountPoint").transform;
		axeHolster = GameObject.Find("axeHolster").transform;
		shotgunHolster = GameObject.Find("shotgunHolster").transform;

		WeaponList[1].weaponTransform.position = shotgunHolster.transform.position;
		WeaponList[1].weaponTransform.rotation = shotgunHolster.transform.rotation;
		WeaponList[1].weaponTransform.parent = shotgunHolster;

		WeaponList[0].weaponTransform.position = axeHolster.transform.position;
		WeaponList[0].weaponTransform.rotation = axeHolster.transform.rotation;
		WeaponList[0].weaponTransform.parent = axeHolster;
	}
	
	// Update is called once per frame
	void Update () {
		_speed = Input.GetAxis("Vertical");//reading vertical axis input
		_strafe = Input.GetAxis("Horizontal");//reading horizontal axis input
		_run = Input.GetKey(KeyCode.LeftShift) ? true : false;//check if run button was pressed
		_crouch = Input.GetKey(KeyCode.LeftControl) ? true : false;//check if run button was pressed
		_jump = Input.GetButton("Jump") ? true : false;//check if jump button was pressed
//check if we're running;
		if (_run &&(Input.GetAxis("Horizontal"))==0){  
			t += Time.deltaTime;
		}
		else {     
			t -= Time.deltaTime;
		}
		t = Mathf.Clamp(t, 0, 1);
		if(Input.GetAxis("Vertical")>=0){
			_speed += t;
		}
		if(Input.GetAxis("Vertical")<0){
			_speed -= t;
		}
		if (_run &&(Input.GetAxis("Vertical"))==0) {
			x += Time.deltaTime;		
		} else {
			x-=Time.deltaTime;		
		}
		x = Mathf.Clamp (x, 0, 1);
		if(Input.GetAxis("Horizontal")>=0){
			_strafe += x;
		}
		if (Input.GetAxis ("Horizontal") < 0) {
			_strafe -= x;
		}
//check if we've stopped
		if (_speed > 1.5f && !_run && (Input.GetAxis("Horizontal")==0)) {
			_stop = true;		
				} 
		else {
			_stop = false;
				}
		if (_speed < -1.5f && !_run && (Input.GetAxis("Horizontal")==0)) {
			_stopBack = true;		
		} 
		else {
			_stopBack = false;
		}
//PROCESSING ROTATION
		Vector3 aimPoint =  Camera.main.transform.forward*10f;
		if(!atWall && canRotate){
			Quaternion targetRotation = Quaternion.LookRotation(aimPoint);
			this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10* Time.deltaTime);
			this.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
		}
		if (!rifleAiming) {//
			rotY += Input.GetAxis ("Mouse Y") * Time.deltaTime * AimYSpeed;
			rotY = Mathf.Clamp (rotY, -1, 1);
			} else {
				rotY = 0;		
		}
//APPLYING GRAVITY TO CHARACTER WHEN IN THE AIR
		if(isGrounded() && _charCtrl !=null){ //player is in the		
				moveDirection.y -= gravity * Time.deltaTime;
				_charCtrl.Move(moveDirection * Time.deltaTime);//moving player's character controller down on axis Y over time	
		}
//SLIDING TO THE CLOSEST COVER LOGIC
		Vector3 ahead = transform.forward;
		Vector3 rayStart = new Vector3(this.transform.position.x, this.transform.position.y+1f, this.transform.position.z);
		Ray	ray = new Ray(rayStart, ahead);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 10f)){
			if(hit.transform.gameObject.name == ("wall")){
				float distToCover = Vector3.Distance(hit.transform.position, transform.position);
				if(distToCover > 5f && Input.GetKeyDown(KeyCode.E)){
					_canSlideToCover = true;						
				}
				else{
					_canSlideToCover = false;
				}
				Debug.DrawLine (ray.origin, hit.point, Color.blue);
			}
			else{
				_canSlideToCover = false;
			}
		}
//JUMP OVER OBSTACLE
		if(Physics.Raycast(ray, out hit, 1f)){
			if(hit.transform.gameObject.tag == ("wall")){
				if(Input.GetButton("Jump")){
					_jumpOverObstcle = true;
				}
				else{
					_jumpOverObstcle = false;
				}
			}
//JUMP UP HIGH
			if(hit.transform.gameObject.tag == ("jumpUpZone")){
				if(Input.GetButton("Jump")){
					_jumpUpHigh = true;
				}
				else{
					_jumpUpHigh = false;
				}
			}

//GET IN THE CAR
			if(hit.transform.gameObject.tag == ("carDoor")){
				currCar = hit.transform.gameObject;
				if((currCar!=null && !driving)){
					if(Input.GetKeyDown(KeyCode.E)){
						if(currCar.GetComponent<CarControl>().lowDoors){
							_getInLowCar = true;
						}
						else{
							_getInCar = true;							
						}
					}
				}
			}
//OPEN THE DOOR
			if(hit.transform.gameObject.tag == ("doorFront")){
				GameObject doorCollisionDetector = hit.transform.parent.gameObject;
				currDoor = doorCollisionDetector.transform.parent.gameObject;
					if(Input.GetKeyDown(KeyCode.E)&&(currDoor.GetComponent<DoorController>().closedDoor)){
						_kickDoor = true;
						StartCoroutine(OpenDoor(doorSide = 1));
					}
				}
			if(hit.transform.gameObject.tag == ("doorBack")){
				GameObject doorCollisionDetector = hit.transform.parent.gameObject;
				currDoor = doorCollisionDetector.transform.parent.gameObject;
					if(Input.GetKeyDown(KeyCode.E)&&(currDoor.GetComponent<DoorController>().closedDoor)){
						_kickDoor = true;
						StartCoroutine(OpenDoor(doorSide = 2));
					}
				}
//GET IN TO THE GLIDER
			if(hit.transform.gameObject.tag == ("glider")){
				currGlide = hit.transform.gameObject;
				if(Input.GetKeyDown(KeyCode.E) && currGlide!=null){
					if(currGlide.GetComponent<FlightScript>().gliderBroken!=true){
						TurnOffCollider ();						
						GameObject sitPoint = currGlide.transform.Find("sitPoint").gameObject;
						this.transform.position = sitPoint.transform.position;
						this.transform.rotation = sitPoint.transform.rotation;
						this.transform.parent = sitPoint.transform;
						_glider  = true;
						gliding = true;
					}
				}
			}				
			else{
				currGlide = null;			
			}
		}
		if(driving){
			if(Input.GetKeyDown(KeyCode.E)){
				GameObject sitPoint = currCar.transform.Find("sitPoint").gameObject;
				this.transform.position = sitPoint.transform.position;
				this.transform.rotation = sitPoint.transform.rotation;
				this.transform.parent = null;
				driving = false;
				_getInCar = false;
				_getInLowCar = false;
			}
		}
//WALKING ALONG THE WALL LOGIC
		if((Input.GetKeyDown(KeyCode.E))&&(currentWall)){
			transform.rotation = currentWall.transform.rotation;//face player to the wall
			//transform.position = new Vector3(transform.position.x, transform.position.y, currentWall.transform.position.z);//position player to the wall
			atWall = true;
			currentWallAngleY = currentWall.transform.rotation.eulerAngles.y;
		}
		if(atWall){
			transform.rotation = currentWall.transform.rotation;//face player to the wall
			if(Input.GetKeyDown(KeyCode.H)){//
				atWall = false;
			}
		}
//AT WALL LOGIC
		if(atWall && usingRifle){
			if(Input.GetMouseButtonDown(1)){
				rifleAiming= true;				
			}
			if(Input.GetMouseButtonUp(1)){
				rifleAiming= false;
			}
		}
		if(!atWall && rifleAiming){
			rifleAiming = false;
		}
//WEAPON PROCESSING
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			if(!usingAxe){				
				StartCoroutine(SwapAxe());
			}
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			if(!usingRifle){
				StartCoroutine(SwapRifle());
			}
		}
		if (usingRifle && !atWall && canRotate && !_run) {
						if (Input.GetMouseButton (1)) {
								rifleAiming = true;
						}
						if (Input.GetMouseButtonUp (1)) {
								rifleAiming = false;
						}
				}
	}
//TRIGGER DETECTION
	void OnTriggerEnter(Collider trigg){
		if(trigg.gameObject.name == "wall"){//if we've triggered the wall behind which we can hind
			currentWall = trigg.gameObject;//make this wall the ONE we'll hide at
			if((_animator.GetCurrentAnimatorStateInfo(0).nameHash == SlideToCoverState)||(_animator.GetNextAnimatorStateInfo(0).nameHash == SlideToCoverState)){
				transform.rotation = currentWall.transform.rotation;
				atWall = true;
				currentWallAngleY = currentWall.transform.rotation.eulerAngles.y;
			}
		}
	}
	void OnTriggerExit(Collider trigg){
		if(trigg.gameObject.name == "wall"){
			atWall = false;
			currentWall = null;
		}
	}
//FIXED UPDATE METHOD
	void FixedUpdate(){
		///setting mecanim parameters
		_animator.SetFloat("Speed", _speed);
		_animator.SetFloat("Strafe", _strafe);
		_animator.SetBool("Stop", _stop);
		_animator.SetBool("Jump", _jump);
		_animator.SetBool("KickDoor", _kickDoor);
		_animator.SetBool("JumpOverObstcle", _jumpOverObstcle);
		_animator.SetBool("JumpUpHigh",_jumpUpHigh);
		_animator.SetFloat("AimHeight",rotY);
		_animator.SetBool("StopBack", _stopBack);
		_animator.SetBool("Aiming", rifleAiming);
		_animator.SetBool("Run", _run);
		_animator.SetBool("ChoppingTree", chopTree);
		_animator.SetBool("GetInCar", _getInCar);
		_animator.SetBool("GetInLowCar", _getInLowCar);
		_animator.SetBool("Crouch", _crouch);
		_animator.SetBool("UsingRifle", usingRifle);
		_animator.SetBool("AtThewall", atWall);
		_animator.SetBool("SlideToCover", _canSlideToCover);
		_animator.SetBool("Glider", _glider);
		_animator.SetBool("Grounded", isGrounded());
	}
	IEnumerator GetInCar(){
		hideAllweapon ();
		_charCtrl.enabled = false;
		canRotate = false;
		GameObject sitPoint = currCar.transform.Find("sitPoint").gameObject;
		carCamPoint = currCar.transform.Find ("carCamPoint").gameObject;
		currCar.GetComponent<CarControl>().openDoor = true;
		this.transform.parent = sitPoint.transform;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		yield return new WaitForSeconds(4.5f);
		driving = true;
		currCar.GetComponent<CarControl>().openDoor = false;
		Transform Seat = currCar.transform.Find("seat");
		this.transform.parent = Seat.transform;
		this.transform.position = Seat.transform.position;
		this.transform.rotation = Seat.transform.rotation;
	}
	IEnumerator GetOutCar(){
		currCar.GetComponent<CarControl>().engineStopped = true;
		Transform Seat = currCar.transform.Find("seat");
		this.transform.parent = Seat.transform;
		this.transform.position = Seat.transform.position;
		this.transform.rotation = Seat.transform.rotation;
		GameObject sitPoint = currCar.transform.Find("sitPoint").gameObject;
		carCamPoint = null;
		currCar.GetComponent<CarControl>().openDoor = true;
		yield return new WaitForSeconds(4.5f);
		_charCtrl.enabled = true;
		canRotate = true;
		driving = false;
		currCar.GetComponent<CarControl>().openDoor = false;
		this.transform.position = sitPoint.transform.position;
		this.transform.rotation = sitPoint.transform.rotation;
		sitPoint = null;
		Seat = null;
		if(usingRifle){
			_animator.SetLayerWeight(1, 1f);
		}

		this.transform.parent = null;
	}
	IEnumerator OpenDoor(int doorSide){
		canRotate = false;
		if(doorSide ==1){
			currDoor.GetComponent<DoorController>().breakDoorForward = true;
			currDoor.GetComponent<DoorController>().closedDoor = false;
		}
		if(doorSide ==2){
			currDoor.GetComponent<DoorController>().breakDoorBackward = true;
			currDoor.GetComponent<DoorController>().closedDoor = false;
		}
		currDoor.GetComponent<DoorController>().DoorSound();
		yield return new WaitForSeconds(0.5f);
		_kickDoor = false;
		canRotate = true;
	}
//SWITCHING TO AXE
	IEnumerator SwapAxe(){
		WeaponList[1].weaponTransform.position = shotgunHolster.transform.position;
		WeaponList[1].weaponTransform.rotation = shotgunHolster.transform.rotation;
		WeaponList[1].weaponTransform.parent = shotgunHolster;
		_animator.SetBool("SwapAxe", true);
		usingAxe = true;
		usingRifle = false;
		currentWeapon = WeaponList[0];	
		yield return new WaitForSeconds(0.5f);			
		WeaponList[0].weaponTransform.position = axeHandPosistion.transform.position;
		WeaponList[0].weaponTransform.rotation = axeHandPosistion.transform.rotation;
		WeaponList[0].weaponTransform.parent = axeHandPosistion.transform;	
		_animator.SetBool("SwapAxe", false);
	}
//SWITCHING TO SHOTGUN
	IEnumerator SwapRifle(){
		WeaponList[0].weaponTransform.position = axeHolster.transform.position;
		WeaponList[0].weaponTransform.rotation = axeHolster.transform.rotation;
		WeaponList[0].weaponTransform.parent = axeHolster;
		_animator.SetBool("SwapShotGun", true);
		currentWeapon = WeaponList[1];
		yield return new WaitForSeconds(0.5f);	
		usingAxe = false;
		usingRifle = true;
		WeaponList[1].weaponTransform.position = shotgunHandPosition.transform.position;
		WeaponList[1].weaponTransform.rotation = shotgunHandPosition.transform.rotation;
		WeaponList[1].weaponTransform.parent = shotgunHandPosition.transform;	
		_animator.SetBool("SwapShotGun", false);
	}
//HOLSTER ALL WEAPONS
	void hideAllweapon(){
		usingAxe = false;
		usingRifle = false;
		WeaponList[0].weaponTransform.position = axeHolster.transform.position;
		WeaponList[0].weaponTransform.rotation = axeHolster.transform.rotation;
		WeaponList[0].weaponTransform.parent = axeHolster;
		WeaponList[1].weaponTransform.position = shotgunHolster.transform.position;
		WeaponList[1].weaponTransform.rotation = shotgunHolster.transform.rotation;
		WeaponList[1].weaponTransform.parent = shotgunHolster;

	}
	void TurnOffCollider (){
		_charCtrl.enabled = false;
		atWall = false;
		currentWall = null;
		canRotate = false;
		if(usingRifle){
			_animator.SetLayerWeight(1, 0f);
		}
	}
	void TurnOnCollider (){
		_charCtrl.enabled = true;
		_jumpOverObstcle = false;
		_jumpUpHigh = false;
		canRotate = true;
		chopTree = false;
		if(usingRifle){
			_animator.SetLayerWeight(1, 1f);
		}
	}
	public void GetOfGlider (){
		_glider = false;
		gliding = false;
		currGlide = null;
		this.transform.parent = null;
		_charCtrl.enabled = true;
		canRotate = true;
		if(usingRifle){
			_animator.SetLayerWeight(1, 1f);
		}
	}
	bool isGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.05f);
	}
}
