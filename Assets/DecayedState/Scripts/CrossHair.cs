using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour {
	public GameObject Player;
	private CharacterControl ptrCharacterControl;//global variables script 
	public GameObject crossHairTexture;
	public bool crossHair =true;
	public GameObject bullet;
	public GameObject gunTip;
	private Rect crossHairPosition;
	public Camera GUICam;
	public float cursorZMAx;
	public float cursorZMin;

	private float shootTime = 0f;

	void Start (){
		Player = GameObject.FindGameObjectWithTag("Player");
		ptrCharacterControl = Player.GetComponent<CharacterControl> ();
	}

	// Update is called once per frame
	void Update () {
		crossHairTexture.transform.LookAt(GUICam.transform);
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
		RaycastHit hit;

		if (ptrCharacterControl.usingRifle) {
						crossHairTexture.GetComponent<Renderer>().enabled = true;		
				} else {
						crossHairTexture.GetComponent<Renderer>().enabled = false;	
				}

		if (Physics.Raycast (ray, out hit, 100)) {
			Debug.DrawLine (ray.origin, hit.point, Color.black);
			crossHairTexture.transform.position = hit.point;
			gunTip.transform.LookAt (hit.point);
			}
		else{
			crossHairTexture.transform.position = ray.origin + (ray.direction * 20);
			gunTip.transform.LookAt(ray.origin + (ray.direction * 20));
		}
		if (ptrCharacterControl.currentWeapon.magazine > 0 && ptrCharacterControl.rifleAiming) {
			if (Input.GetMouseButton (0)) {
				if (shootTime <= Time.time) {
					shootTime = Time.time + ptrCharacterControl.currentWeapon.fireRate;
					GameObject obj = PoolManager.current.GetPooledObject();
					if(obj == null) return;
					obj.transform.position = ptrCharacterControl.currentWeapon.weaponMuzzlePoint.position;
					obj.transform.rotation = ptrCharacterControl.currentWeapon.weaponMuzzlePoint.rotation;
					obj.SetActive(true);

					ptrCharacterControl.currentWeapon.magazine -= 1;
					if (ptrCharacterControl.currentWeapon.magazine < 0) {
						ptrCharacterControl.currentWeapon.magazine = 0;
					}
					if (ptrCharacterControl.currentWeapon.magazine == 0) {
						ptrCharacterControl._animator.SetBool ("ReloadPistol", true);	
					}
				}				
			}
		}
	}
}
