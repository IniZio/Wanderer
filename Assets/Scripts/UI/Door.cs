using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI {
	public class Door : MonoBehaviour {
		Animator ani;
		public bool isOpen = false;
		void Start () {
			this.ani = GetComponent<Animator>();
			this.ani.SetBool ("OpenDoor", false);
		}
		[PunRPC]
		public void OpenDoor() {
			this.ani.SetBool ("OpenDoor", true);
			this.isOpen = true;
		}
		[PunRPC]
		public void CloseDoor() {
			this.ani.SetBool ("OpenDoor", false);
			this.isOpen = false;
		}
	}
}
