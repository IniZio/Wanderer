using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;

namespace Fyp.Game.UI {
	public class Door : MonoBehaviour {
		Animator ani;
		public bool isOpen = false;
		int NumOfPlayer = 0;
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

		void OnCollisionEnter(Collision col) {
			if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
				ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
				if (script.isReady) {
					this.NumOfPlayer += 1;
				}
			}
		}

		void OnCollisionExit(Collision col) {
			if (col.gameObject.CompareTag("Player1Character") || col.gameObject.CompareTag("Player2Character")) {
				ControlScript script = col.gameObject.GetComponent("ControlScript") as ControlScript;
				if (script.isReady) {
					this.NumOfPlayer -= 1;
				}
			}
		}
	}
}
