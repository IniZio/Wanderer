using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
	public class Door : MonoBehaviour {
		Animator ani;
		void Start () {
			this.ani = GetComponent<Animator>();
			this.ani.SetBool ("OpenDoor", false);
		}
		public void OpenDoor() {
			this.ani.SetBool ("OpenDoor", true);
		}
		public void CloseDoor() {
			this.ani.SetBool ("OpenDoor", false);
		}
	}
}
