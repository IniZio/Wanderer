using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
	public class DoorLight : MonoBehaviour {
		Animator ani;
		void Start () {
			this.ani = GetComponent<Animator>();
			this.ani.SetBool ("OpenDoorLight", false);
		}
		public void OpenDoor() {
			this.ani.SetBool ("OpenDoorLight", true);
		}
		public void CloseDoor() {
			this.ani.SetBool ("OpenDoorLight", false);
		}
	}
}
