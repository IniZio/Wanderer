using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;

public class pick : MonoBehaviour {

	public TutManager manager;
	public Collider col;
	public string type;
	public bool isTut = false;
	void Start() {
		if (isTut) {
			GameObject temp = GameObject.FindGameObjectWithTag("TutManager");
			manager = temp.GetComponent("TutManager") as TutManager;
		}
	}
	void OnTriggerEnter(Collider player){
		var tag=player.gameObject.tag;
		Debug.Log("col");
		 if (tag == "Player"){
				ControlScript script=player.gameObject.GetComponent("ControlScript") as ControlScript;
			switch(type){
				case "axe":
					Debug.Log("player");
					manager.axe.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);
					// manager.axe.transform.rotation = Quaternion.Euler(0, 100, 0);

					script.pick(manager.axe,col);
					script.equip(manager.axe);
					manager.setState(2);
					break;
				case "gun":
					Debug.Log("gunnnnnnnnnnn");
					manager.axe.SetActive(false);
					manager.gun.transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);
					script.pick(manager.gun, col);
					script.equipGun(manager.gun);
					manager.setState(5);
					break;
				default:
					script.pickwood();
					this.gameObject.SetActive(false);
					manager.setState(3);
					break;
			 }

		 }
	}
}