using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.Carmera {
	public class FollowCamera : Photon.PunBehaviour {
		public GameObject camera;

		void setCamera(GameObject camera) {
			this.camera = camera;
		}
	}
}

