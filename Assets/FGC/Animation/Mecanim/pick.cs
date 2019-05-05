using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;

public class pick : MonoBehaviour {

	public TutManager manager;
	public Collider col;
	public string type;
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
			default:
			script.pickwood();
			this.gameObject.SetActive(false);
			
			break;
			 }
			
		 }
	}
}