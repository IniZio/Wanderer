using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.Carmera {
	public class PlayerCamera : MonoBehaviour {
		public GameObject player;
		private Vector3 playerHeight = new Vector3((float) 0, (float) 0, (float) 0);

		void LateUpdate() {
			if (this.player != null) {
				transform.rotation = player.transform.rotation;
				transform.position = player.transform.position + playerHeight;
				transform.position = transform.position + transform.forward * (float)0.15;
			}
		}
		public void setCamera(GameObject player) {
			this.player = player;
		}
	}
}

