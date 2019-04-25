using UnityEngine;
using System.Collections;

public class lookAt : MonoBehaviour {
	public Transform boneToRotate;
	public float OffsetX;
	public float OffsetY;
	public float OffsetZ;
	private GameObject objPlayer;
	private CharacterControl ptrCharContrlScript;
	public bool thisIsSpine;
	// Use this for initialization
	void Start () {	
		objPlayer = (GameObject) GameObject.FindWithTag ("Player");//caching player's GO
		ptrCharContrlScript = (CharacterControl) objPlayer.GetComponent( typeof(CharacterControl) );//caching player control script
		//ptrCharContrlScript = objPlayer.GetComponent<PlayerController>();

	}	
	void LateUpdate(){//bone rotation
		if(ptrCharContrlScript.rifleAiming){			
		    Vector3 aimPoint = Camera.main.transform.position + Camera.main.transform.forward*10f;
		   	boneToRotate.LookAt(aimPoint);//rotate bone towards aimpoint
			boneToRotate.transform.Rotate(boneToRotate.transform.rotation.x+OffsetX, boneToRotate.transform.rotation.y+OffsetY, boneToRotate.transform.rotation.z+OffsetZ);
			//as bone's axis are not the same as aimPoint's axis lets reorient bone properly
		}
//PROCESS WEAPON RECOIL
		//if(thisIsSpine){
		//if(ptrCharContrlScript.firing){	
		//	Vector3 rotation = ptrCharContrlScript.mySpine.localEulerAngles;			
		//	rotation.z += Mathf.Sin(Time.time * ptrCharContrlScript.currentWeapon.weaponKickBack);		
		//	ptrCharContrlScript.mySpine.localEulerAngles = rotation;
		//	Debug.Log ("SIN");
		//	}
		//}
	}
}
