using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;
using Fyp.Game.Network;

namespace Fyp.Game.UI {
	public class Door : MonoBehaviour {
		Animator ani;
		public bool isOpen = false;
		public int NumOfPlayer = 0;

		void Start () {
			this.ani = GetComponent<Animator>();
			this.ani.SetBool ("OpenDoor", false);
		}

		public void OpenDoor() {
			this.ani.SetBool("OpenDoor", true);
			this.isOpen = true;
		}
		public void CloseDoor() {
			this.ani.SetBool("OpenDoor", false);
			this.isOpen = false;
		}

		void OnTriggerEnter (Collider col) {
			Debug.Log("hihi i am enter");
			Debug.Log(col.gameObject.CompareTag("Player1Character"));
			if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
				ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
				if (script.isReady) {
					this.NumOfPlayer += 1;
					script.enterWaitingRmDoor();
				}
			}
		}

		void OnTriggerExit(Collider col) {
			Debug.Log("hihi i am exit");
			if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
				ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
				if (script.isReady) {
					this.NumOfPlayer -= 1;
					script.exitWaitingRmDoor();
				}
			}
		}
	}
}
