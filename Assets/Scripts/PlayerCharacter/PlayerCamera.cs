using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.Carmera {
	public class PlayerCamera : MonoBehaviour {
		// public int speed = 10;

		// public Camera firstPersonViewCamera;
		// public Camera menuViewCamera;

		// public bool onMenuView;

		// // Use this for initialization
		// void Start () {
		// 	firstPersonViewCamera.enabled = false;
		// 	menuViewCamera.enabled = true;
		// 	onMenuView = true;
		// }

		// // Update is called once per frame
		// void Update()
		// {
		// 	float horizontal = Input.GetAxis("Mouse X") * speed;
		// 	float vertical = Input.GetAxis("Mouse Y") * speed;

		// 	transform.Rotate(0f, horizontal, 0f, Space.World);
		// 	transform.Rotate(-vertical, 0f, 0f, Space.Self);
		// }

		// void switchCamera() {
		// 	firstPersonViewCamera.enabled = !firstPersonViewCamera.enabled;
		// 	menuViewCamera.enabled = !menuViewCamera.enabled;
		// 	onMenuView = !onMenuView;
		// }

		public GameObject player;
		private Vector3 offset;
		private Vector3 playerHeight = new Vector3((float) 0.2, (float) 0, (float) 0);

		void start() {
			offset = transform.position;
		}

		void LateUpdate() {
			if (this.player != null) {
				transform.position = player.transform.position + offset + playerHeight;
            	transform.rotation = player.transform.rotation;
			}
		}

		public void setCamera(GameObject player) {
			Debug.Log("set Camera");
			this.player = player;
		}
	}
}

