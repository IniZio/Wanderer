using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer2 : MonoBehaviour
{

    //First, we will create a reference called myAnimator so we can talk to the Animator component on the game object.
    //The Animator is what listens to our instructions and tells the mesh which animation to use.
    private Animator myAnimator;

    private int count = 0;

    public GameObject Tool, myTool;

    private Transform toolHandPosistion, gunHandPosistion;
    private Transform Holster1, gunhold;
    private Transform Holster2;

    SimpleShoot SimpleShoot;


    // The start method is called when the script is initalized, before other stuff in the scripts start happening.
    void Start()
    {
        //We have a reference called myAnimator but we need to fill that reference with an Animator component.
        //We can do that by 'getting' the animator that is on the same game object this script is appleid to.
        myAnimator = GetComponent<Animator>();
        toolHandPosistion = GameObject.Find("toolMountPoint").transform;
        //	toolHandPosistion = GameObject.Find ("toolMountPoint").transform;
        gunhold = GameObject.Find("Holster1").transform;
        //	Holster1 = GameObject.Find ("Holster1").transform;
        //	Holster2 = GameObject.Find ("Holster2").transform;
        // WeaponList[1].weaponTransform.position = Holster2.transform.position;
        // WeaponList[1].weaponTransform.rotation = Holster2.transform.rotation;
        // WeaponList[1].weaponTransform.parent = Holster2;
        myTool = Instantiate(Tool, gunhold.transform.position, gunhold.transform.rotation) as GameObject;
        // Tool.SetActive(false);
        //Tool.transform.position = gunhold.transform.position;
        //Tool.transform.rotation = gunhold.transform.rotation;
        myTool.transform.parent = gunhold;
        myTool.SetActive(true);
        //SimpleShoot = myTool.GetComponent("SimpleShoot") as SimpleShoot;
    }

    // Update is called once per frame so this is a great place to listen for input from the player to see if they have
    //interacted with the game since the LAST frame (such as a button press or mouse click).
    void Update()
    {

        // myAnimator.SetFloat ("HSpeed", Input.GetAxis ("Horizontal"));
        // //!!!switch run mode
        // if (Input.GetKey (KeyCode.LeftShift)) {
        // 	_VSpeed = 2 * (Input.GetAxis ("Vertical"));
        // } else {
        // 	_VSpeed = Input.GetAxis ("Vertical");
        // }
        // myAnimator.SetFloat ("VSpeed", _VSpeed);

        // if (Input.GetKey ("a")) {

        // 	//Rotate the character procedurally based on Time.deltaTime.  This will give the illusion of moving
        // 	//Even though the animations don't have root motion
        // 	transform.Rotate (Vector3.down * Time.deltaTime * 100.0f);

        // 	//Also, IF we're currently standing still (both vertically and horizontally)
        // 	if ((Input.GetAxis ("Vertical") == 0f) && (Input.GetAxis ("Horizontal") == 0)) {
        // 		//change the animation to the 'inplace' animation
        // 		myAnimator.SetBool ("TurnLeft", true);
        // 	}

        // } else {
        // 	//Else here means if the Q key is not being held down
        // 	//Then we make sure that we are not playing the turning animation
        // 	myAnimator.SetBool ("TurnLeft", false);
        // }

        // //Same thing for E key, just rotating the other way!
        // if (Input.GetKey ("d")) {
        // 	transform.Rotate (Vector3.up * Time.deltaTime * 100.0f);
        // 	if ((Input.GetAxis ("Vertical") == 0f) && (Input.GetAxis ("Horizontal") == 0)) {
        // 		myAnimator.SetBool ("TurnRight", true);
        // 	}

        // } else {
        // 	myAnimator.SetBool ("TurnRight", false);
        // }

        if (Input.GetKey("q"))
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
        // 	if (Input.GetKey (KeyCode.Alpha3)) {
        // 	myAnimator.SetTrigger("Pick");
        // }     
        // RaycastHit hit = new RaycastHit ();         // This ray will see where we clicked er chopped
        // Vector3 ahead = RayOrigin.forward;        
        // Vector3 rayStart = new Vector3 (RayOrigin.position.x, RayOrigin.position.y + 1f, RayOrigin.position.z);        
        // Ray ray = new Ray (rayStart, ahead);         // Did we hit anything at that point, out as far as 10 units?
        //         
        // if (Physics.Raycast (ray, out hit, 1.5f)) {   
        // 	if (hit.collider.gameObject.tag == "Tree") { 
        // 		closestTreePosition = hit.transform.position;            
        // 	}            
        // 	if (usingAxe && Input.GetKeyUp (KeyCode.E)) {
        // 		choppingPoint = hit.point;                
        // 		chopTree = true;
        // 		StartCoroutine (ChopItDown (hit, closestTreePosition));            
        // 	}        
        // }
        if (Input.GetKeyUp("e"))
        {
            StartCoroutine(shooting1());
        }
    }
    IEnumerator shooting1()
    {
        if (count == 0)
        {
            myAnimator.SetBool("ToTwoHandedAttack", true);
            yield return new WaitForSeconds(1f);
            Debug.Log("g");
            myAnimator.SetBool("ToTwoHandedAttack", false);
            count++;

        }
        else
        {
            myAnimator.SetBool("TwoHandedAttack", true);
            yield return new WaitForSeconds(0.5f);
            myAnimator.SetBool("TwoHandedAttack", false);
        }


    }

    IEnumerator Arming1()
    {
        myAnimator.SetBool("SwitchTool", true);
        //currentWeapon = WeaponList[0];
        yield return new WaitForSeconds(0.5f);
        myTool.transform.position = toolHandPosistion.transform.position;
        myTool.transform.rotation = toolHandPosistion.transform.rotation;
        myTool.transform.parent = toolHandPosistion.transform;
        myAnimator.SetBool("SwitchTool", false);
        myAnimator.SetInteger("CurrentAction", 1);

    }
    IEnumerator Disarming1()
    {
        myAnimator.SetBool("SwitchTool", false);

        //	myAnimator.SetBool ("SwitchTool", true);
        //currentWeapon = WeaponList[0];
        yield return new WaitForSeconds(0.5f);
        myTool.transform.position = gunhold.transform.position;
        myTool.transform.rotation = gunhold.transform.rotation;
        myTool.transform.parent = gunhold;
        //	myAnimator.SetBool ("SwitchTool", false);
        myAnimator.SetBool("SwitchTool", true);

        myAnimator.SetInteger("CurrentAction", 0);

    }



}