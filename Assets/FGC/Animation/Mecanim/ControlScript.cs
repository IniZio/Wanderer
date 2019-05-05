using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.ResourcesGenerator;
using Leap.Unity.Interaction;
//using System.Collections;

namespace Fyp.Game.PlayerControl
{
    public class ControlScript : Photon.PunBehaviour, IPunObservable
    {

        public string state = "";
        public int[] loadouts = { 0, 3, 6 };
        public int loadoutIndex = 0;
        PlayerStatus playerStatus;
        public Hud hud;
        public bool isMaster;
        public bool chopping=false;
        public bool isReady = false;
        public bool isStandingWaitingRmDoor = false;
        public bool isStandingBaseGate = false;
        public bool isStandingRandomMapGate = false;
        public bool isAttacking = false;
        public bool isMoaning = false;
        bool isMe = false;
        public bool isTutMode = false;
        public AudioSource footstep;
        private int count = 0;

        public bool leapchop= false;

        private GameObject target;
        public float attackRange = 2;
        private float attackTimer = 0;
        public float attackInterval = 1;
        public int health = 60;

		//First, we will create a reference called myAnimator so we can talk to the Animator component on the game object.
		//The Animator is what listens to our instructions and tells the mesh which animation to use.
		private Animator myAnimator;
		private float _VSpeed;
		public bool unarmed;
		public bool twoHanded;
        public bool usingAxe, usingGun;
		public bool arming1;
		public bool chopTree;

		public GameObject FallingTreePrefab;    
		public Transform RayOrigin;    
		private Vector3 choppingPoint;    
		public AudioClip[] chopSounds;    
		Vector3 closestTreePosition;

        private Transform toolHandPosistion, gunHandPosistion, gunHold;
        private Transform Holster1;
		private Transform Holster2;

		public List<WeaponInfo> WeaponList = new List<WeaponInfo> ();
		[System.Serializable]
		public class WeaponInfo {
			public string weaponName = "veapon name";
			public float fireRate = 0.1f;
			public Transform weaponTransform;
			public Transform weaponMuzzlePoint;
			public float weaponKickBack;
			public GameObject bulletPrefab;
			public int totalAmmo;
			public int magazine;
		}
		public float colSize = 0.9F;
		public CapsuleCollider cc;
		public GameObject colObj;

		public int randomSeed = -1;

        public RaycastHit temp;
        public bool isTemp = false;

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
			if (stream.isWriting) {
				stream.SendNext(isMaster);
				stream.SendNext(isReady);
				stream.SendNext(isStandingWaitingRmDoor);
				stream.SendNext(isStandingBaseGate);
                stream.SendNext(health);
                stream.SendNext(loadouts);
                stream.SendNext(loadoutIndex);
				stream.SendNext(randomSeed);
                stream.SendNext(isStandingRandomMapGate);
			}
			else {
				isMaster = (bool) stream.ReceiveNext();
				isReady = (bool) stream.ReceiveNext();
				isStandingWaitingRmDoor = (bool) stream.ReceiveNext();
				isStandingBaseGate = (bool) stream.ReceiveNext();
                this.health = (int)stream.ReceiveNext();
                loadouts = (int[])stream.ReceiveNext();
                loadoutIndex = (int)stream.ReceiveNext();
                randomSeed = (int) stream.ReceiveNext();
                isStandingRandomMapGate = (bool) stream.ReceiveNext();
            }
		}

		void Start () {
			//We have a reference called myAnimator but we need to fill that reference with an Animator component.
			//We can do that by 'getting' the animator that is on the same game object this script is appleid to.
            if (!isTutMode) {
			    DontDestroyOnLoad(this);
            }
			myAnimator = GetComponent<Animator> ();
			toolHandPosistion = GameObject.Find ("toolMountPoint").transform;
			Holster1 = GameObject.Find ("Holster1").transform;
			Holster2 = GameObject.Find ("Holster2").transform;
            gunHandPosistion = GameObject.Find("gunMountPoint").transform;
            gunHold = GameObject.Find("gunhold").transform;
            // WeaponList[1].weaponTransform.position = Holster2.transform.position;
            // WeaponList[1].weaponTransform.rotation = Holster2.transform.rotation;
            // WeaponList[1].weaponTransform.parent = Holster2;

            WeaponList[0].weaponTransform.position = Holster1.transform.position;
			WeaponList[0].weaponTransform.rotation = Holster1.transform.rotation;
			WeaponList[0].weaponTransform.parent = Holster1;
		}

		// Update is called once per frame so this is a great place to listen for input from the player to see if they have
		//interacted with the game since the LAST frame (such as a button press or mouse click).
		 [PunRPC]
		void Update () {
			if (PhotonNetwork.isMasterClient && randomSeed == -1) {
				System.Random rd = new System.Random();
				// rd.Next(1, 999)
				randomSeed = rd.Next(1, 999);
			}
            switch (state)
            {
                case "attacking":
                    Weapon weapon = Constants.Weapons[loadouts[loadoutIndex]];
                    myAnimator.SetBool("Run", false);

                    if (photonView.isMine)
                    {

                        //if (Constants.Weapons[loadouts[loadoutIndex]].type == "melee")
                        //{
                        if (attackTimer < attackInterval)
                        {
                            attackTimer += Time.deltaTime;
                            target = null;
                            break;
                        }
                        attackTimer = 0;
                        //       }
                        if (/*!animator.GetBool("ToTwoHandAttack") && */isAttacking)
                        {
                            state = "";
                            isAttacking = false;
                            myAnimator.SetBool("ToTwoHandedAttack", false);
                            if (weapon.type == "melee")
                            {
                                RaycastHit hit;
                                Vector3 fwd = transform.TransformDirection(Vector3.forward);

                                if (Physics.Raycast(transform.position, fwd, out hit, attackRange))
                                {
                                    DealDamage(hit.collider.gameObject);
                                }
                            }
                            else
                            {
                                if (target != null)
                                {
                                    DealDamage(target);
                                    target = null;
                                }
                            }

                        }
                        else
                        {
                            isAttacking = true;
                            if (weapon.type == "melee")
                            {
                                myAnimator.SetBool("ToTwoHandAttack", true);
                            }
                        }
                    }
                    break;
                case "harmed":
                    if (!myAnimator.GetBool("Get_Hit") && isMoaning)
                    {
                        isMoaning = false;
                        if (health <= 0)
                        {
                            health = 0;
                            myAnimator.SetBool("Dead", true);
                            GameObject.Find("SceneManager").GetComponent<DungeonMission>().FailMission();
                            return;
                        }
                        state = "";
                    }
                    else
                    {
                        isMoaning = true;
                        myAnimator.SetBool("Get_Hit", true);
                    }
                    break;
                default:
                    break;
            }

            Debug.Log("ControlScript update isMINE");
            if (photonView.isMine || isTutMode)
            {
                // Update HUD
                try
                {
                    hud = GameObject.FindGameObjectWithTag("Hud").GetComponent<Hud>();
                }
                catch { }
                if (hud != null)
                {
                    hud.SetHealth(health);
                }

                this.cc.radius = this.colSize;

                //Set the VSpeed and HSpeed floats for our animator to control walking and strafing animations.
                myAnimator.SetFloat("HSpeed", Input.GetAxis("Horizontal"));

                //Set Jump Boolean to true to trigger jump animation, then wait a small time and set to false so we don't jump agani.
                if (Input.GetButtonDown("Jump"))
                {
                    myAnimator.SetBool("Jumping", true);
                    Invoke("StopJumping", 0.1f);
                    ReadyToPlay();
                }
                //!!!switch run mode
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _VSpeed = 2 * (Input.GetAxis("Vertical"));
                }
                else
                {
                    _VSpeed = Input.GetAxis("Vertical");
                }
                myAnimator.SetFloat("VSpeed", _VSpeed);

                if (Input.GetKey("a"))
                {

                    //Rotate the character procedurally based on Time.deltaTime.  This will give the illusion of moving
                    //Even though the animations don't have root motion
                    transform.Rotate(Vector3.down * Time.deltaTime * 100.0f);

                    //Also, IF we're currently standing still (both vertically and horizontally)
                    if ((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0))
                    {
                        //change the animation to the 'inplace' animation
                        myAnimator.SetBool("TurnLeft", true);
                    }

                }
                else
                {
                    //Else here means if the Q key is not being held down
                    //Then we make sure that we are not playing the turning animation
                    myAnimator.SetBool("TurnLeft", false);
                }

                //Same thing for E key, just rotating the other way!
                if (Input.GetKey("d"))
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * 100.0f);
                    if ((Input.GetAxis("Vertical") == 0f) && (Input.GetAxis("Horizontal") == 0))
                    {
                        myAnimator.SetBool("TurnRight", true);
                    }

                }
                else
                {
                    myAnimator.SetBool("TurnRight", false);
                }

                if (Input.GetKey(KeyCode.Alpha1))
                {
                    switch (myAnimator.GetInteger("CurrentAction"))
                    {
                        case 0:
                            StartCoroutine(Arming1());
                            break;

                        case 1:
                            StartCoroutine(Disarming1());
                            break;
                    }
                }
                // Shoot once
                if (Input.GetKey("t"))
                {
                    state = "attacking";
                }

                //Combining methods.
                //You can combine all of these methods (and much more advanced logic) in many ways!  Let's go over an example.  We want our character to kneel down
                //Stay in a kneeling loop idle, then stand up when we tell them to.  For this we'll need to combine a triggered 'transition' animation with looping
                //animations.  You can see in the Animator Controller that Action3 (kneeling) is actually comprised of 3 animations.  "kneeling down", "kneeling idle"
                //and "kneeling stand".

                //The first is "kneeling down".  The intro requirement is the same as before, CurrentAction = 3.  We can control this using the same "SetInteger" method
                //we used in our previous examples.  However, the exit transition is based on exit time.  So as soon as that intro "kneeling down" animation plays
                //The animation will transition into a kneeling idle.  Now we want the character to remain there until we tell it to get up, so the exit transition
                //From kneeling idle is "currentaction = 0".  This means we'll need to set up a toggle just like in Example #4.  When we toggle from 3 to 0, we'll transition
                //Into the kneeling stand animation, which will get our character back to their feet.  Finally the exit transition for kneeling stand is exit time, so
                //As soon as they are done standing up they will go the next state ("idle/walk").
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    switch (myAnimator.GetInteger("CurrentAction"))
                    {
                        case 0:
                            StartCoroutine(Draw2());
                            break;

                        case 2:
                            StartCoroutine(Hold2());
                            break;
                    }
                }
                // if (Input.GetKeyDown("2"))
                // {
                //     if (myAnimator.GetInteger("CurrentAction") == 0)
                //     {
                //         myAnimator.SetInteger("CurrentAction", 2);
                //     }
                //     else if (myAnimator.GetInteger("CurrentAction") == 2)
                //     {
                //         myAnimator.SetInteger("CurrentAction", 0);
                //     }
                // }
                //MauryEND
                // if (Input.GetKey ("r") && (myAnimator.GetInteger ("CurrentAction") == 0)) {
                // 	myAnimator.SetBool ("2HandIdle", true);
                // }
                // if (Input.GetKey ("t") && (myAnimator.GetInteger ("CurrentAction") == 0)) {
                // 	myAnimator.SetBool ("chopTree", true);
                // } else {
                // 	myAnimator.SetBool ("chopTree", false);
                // }

                RaycastHit hit2 = new RaycastHit();         // This ray will see where we clicked er chopped
                Vector3 ahead = RayOrigin.forward;
                Vector3 rayStart = new Vector3(RayOrigin.position.x, RayOrigin.position.y + 1f, RayOrigin.position.z);
                Ray ray = new Ray(rayStart, ahead);         // Did we hit anything at that point, out as far as 10 units?

                if (Physics.Raycast(ray, out hit2, 1.5f))
                {
                    if (hit2.collider.gameObject.tag == "Resources")
                    {   // Resources object has valueable type which is Tree, Rock and Metal
                        Debug.Log("hited");
                        ResourcesGenerator.Resources res = hit2.collider.gameObject.GetComponent("Resources") as ResourcesGenerator.Resources;
                        switch (res.type)
                        {
                            case "Tree":
                                Debug.Log("treeasdfasdfasdf");
                                closestTreePosition = hit2.transform.position;
                                break;
                            case "Rock":
                                Debug.Log("Rockasdfasdfasdf");
                                closestTreePosition = hit2.transform.position;
                                break;
                            case "Metal":
                                Debug.Log("Metalasdfasdfasdf");
                                closestTreePosition = hit2.transform.position;
                                break;
                        }
                    }
                                            this.temp = hit2;
                                            isTemp = true;

                    if (usingAxe && (Input.GetKey(KeyCode.E)||leapchop))
                    {
                        this.temp = hit2;
                        this.OnChopping();
                    }
                }
            }
            //MauryEND
        }

        //This method is called after jumping is started to stop the jumping!
        void StopJumping()
        {
            myAnimator.SetBool("Jumping", false);
        }

        public void ReadyToPlay()
        {
            isReady = !isReady;
        }

        //We've added some simple GUI labels for our controls to make it easier for you to test out.

        public PlayerStatus GetPlayerStatus()
        {
            return playerStatus;
        }

        public void setPlayerStatus(PlayerStatus ps)
        {
            Debug.Log("settttt");
            Debug.Log(ps);
            playerStatus = ps;
        }

        public bool getStandingWaitingRmDoor()
        {
            return this.isStandingWaitingRmDoor;
        }

        public void enterWaitingRmDoor()
        {
            this.isStandingWaitingRmDoor = true;
        }
        public void exitWaitingRmDoor()
        {
            this.isStandingWaitingRmDoor = false;
        }

        public bool getStandingBaseGate()
        {
            return this.isStandingBaseGate;
        }

        public void enterBaseGate()
        {
            this.isStandingBaseGate = true;
        }
        public void exitBaseGate()
        {
            this.isStandingBaseGate = false;
        }

        public bool getRandomMapGate()
        {
            return this.isStandingRandomMapGate;
        }

        public void enterRandomMapGate()
        {
            this.isStandingRandomMapGate = true;
        }
        public void exitRandomMapGate()
        {
            this.isStandingRandomMapGate = false;
        }
        public void SetIsMe()
        {
            this.isMe = true;
        }

        public void SwitchWeapon(int index)
        {
            loadoutIndex = index;
            StartCoroutine(Arming1());
        }

        public bool getIsMe()
        {
            return this.isMe;
        }

        public void DealDamage(GameObject target)
        {
            Weapon weapon = Constants.Weapons[loadouts[loadoutIndex]];
            if (target.GetComponent<NPCControl>() != null)
            {
                target.GetComponent<NPCControl>().Harmed(weapon.damage);
            }

            if (target.GetComponent<AnimalControl>() != null)
            {
                target.GetComponent<AnimalControl>().Harmed(weapon.damage);
            }
        }

        public void Chopping()
        {
            //print("Start chopping");
            //RaycastHit hit;
            //Vector3 fwd = transform.TransformDirection(Vector3.forward);
            //float melee_range = 1;

            //if (Physics.Raycast(transform.position, fwd, out hit, melee_range))
            //{
            //   Debug.ClearDeveloperConsole();
            // Debug.Log("Melee hit something! " + hit.collider.name);
            //}
            state = "attacking";
        }

        void OnTriggerEnter(Collider col)
        {
            if (photonView.isMine)
            {
                if (col.gameObject.CompareTag("Resources"))
                {
                    Debug.Log("------enter Resources");
                    this.colObj = col.gameObject;
                }
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.CompareTag("Resources"))
            {
                Debug.Log("------exit Resources");
                this.colObj = null;
            }
        }

        public void OnChopping()
        {   
            if (!usingAxe ||chopping ||closestTreePosition == null || !isTemp) {
                return;
            }
            choppingPoint = this.temp.point;
            chopTree = true;
            StartCoroutine(ChopItDown(this.temp, closestTreePosition));
            this.isTemp = false;
        }

        //We've added some simple GUI labels for our controls to make it easier for you to test out.

        void OnGUI()
        {

        }

        public void Harmed(int damage = 3)
        {
            Debug.Log("I did get harmed");
            state = "harmed";
            health -= damage;
        }

        public void Attack(GameObject target = null) {
            this.target = target;
            state = "attacking";
        }

        IEnumerator Arming1()
        {
            myAnimator.SetBool("SwitchTool", true);
          //  arming1 = true;
            usingAxe = true;
            //currentWeapon = WeaponList[0];
            yield return new WaitForSeconds(0.5f);
            WeaponList[0].weaponTransform.position = toolHandPosistion.transform.position;
            WeaponList[0].weaponTransform.rotation = toolHandPosistion.transform.rotation;
            WeaponList[0].weaponTransform.parent = toolHandPosistion.transform;
            myAnimator.SetBool("SwitchTool", false);
            myAnimator.SetInteger("CurrentAction", 1);
        }
        IEnumerator Disarming1()
        {
            myAnimator.SetBool("SwitchTool", true);
            //arming1 = false;
            usingAxe = false;
            //currentWeapon = WeaponList[0];
            yield return new WaitForSeconds(0.5f);
            WeaponList[0].weaponTransform.position = Holster1.transform.position;
            WeaponList[0].weaponTransform.rotation = Holster1.transform.rotation;
            WeaponList[0].weaponTransform.parent = Holster1;
            myAnimator.SetBool("SwitchTool", false);
            myAnimator.SetInteger("CurrentAction", 0);

        }
        IEnumerator Draw2()
        {
            myAnimator.SetBool("Draw", true);
            usingGun = true;
            //currentWeapon = WeaponList[0];
            yield return new WaitForSeconds(0.5f);
            WeaponList[0].weaponTransform.position = gunHandPosistion.transform.position;
            WeaponList[0].weaponTransform.rotation = gunHandPosistion.transform.rotation;
            WeaponList[0].weaponTransform.parent = gunHandPosistion.transform;
            myAnimator.SetBool("Draw", false);
            myAnimator.SetInteger("CurrentAction", 2);
        }
        IEnumerator Hold2()
        {
            myAnimator.SetBool("Hold", true);
            usingGun = false;
            //currentWeapon = WeaponList[0];
            yield return new WaitForSeconds(0.5f);
            WeaponList[0].weaponTransform.position = gunHold.transform.position;
            WeaponList[0].weaponTransform.rotation = gunHold.transform.rotation;
            WeaponList[0].weaponTransform.parent = gunHold;
            myAnimator.SetBool("Hold", false);
            myAnimator.SetInteger("CurrentAction", 0);
        }

        IEnumerator ChopItDown(RaycastHit hit, Vector3 closestTreePosition)
        {
   
                myAnimator.SetBool("ToTwoHandedAttack", true);

            yield return new WaitForSeconds(1f);
            // Remove the tree from the terrain tree list		        

                hit.collider.gameObject.SetActive(false);
                // Now refresh the terrain, getting rid of the darn collider
                // float[, ] heights = terrain.GetHeights (0, 0, 0, 0);
                // terrain.SetHeights (0, 0, heights);
                // Put a falling tree in its place	  
                closestTreePosition.y += 3.1f;
                Instantiate(FallingTreePrefab, closestTreePosition, Quaternion.Euler(0, 0, 80));
    
            myAnimator.SetBool("ToTwoHandedAttack", false);
                        chopTree = false;

        }
        public void treeChopSound()
        {
            GameObject go = new GameObject("Audio");
            go.transform.position = choppingPoint;         //Create the source

            AudioSource source = go.AddComponent<AudioSource>();
            source.clip = chopSounds[Random.Range(0, chopSounds.Length)];
            source.Play();
            Destroy(go, source.clip.length);
        }

        public void pick(GameObject go,Collider col)
        {
            col.enabled=false;
            StartCoroutine(Pick (go));
        }
        IEnumerator Pick(GameObject go) {
            myAnimator.SetTrigger("Pick");
            yield return new WaitForSeconds(1f);
        }

                public void equip(GameObject go)
        {
            usingAxe = true;
            go.transform.position = toolHandPosistion.transform.position;
            go.transform.rotation = toolHandPosistion.transform.rotation;
            go.transform.Rotate(0f,-100f,0f, Space.Self);
            usingAxe=true;
            go.transform.parent = toolHandPosistion.transform;
           
        }

         public void pickwood()
        {
            StartCoroutine(Pickwood());
        }
        IEnumerator Pickwood() {
            myAnimator.SetTrigger("Pick");
            yield return new WaitForSeconds(1f);
        }

    }
}
