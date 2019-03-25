using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP.Game.Carmera {
	public class WaitingRoomCamera : MonoBehaviour {
		public int speed = 10;

		public Camera firstPersonViewCamera;
		public Camera menuViewCamera;

		public bool onMenuView;

		// Use this for initialization
		void Start () {
			firstPersonViewCamera.enabled = false;
			menuViewCamera.enabled = true;
			onMenuView = true;
		}

		// Update is called once per frame
		void Update()
		{
			float horizontal = Input.GetAxis("Mouse X") * speed;
			float vertical = Input.GetAxis("Mouse Y") * speed;

			transform.Rotate(0f, horizontal, 0f, Space.World);
			transform.Rotate(-vertical, 0f, 0f, Space.Self);
		}

		void switchCamera() {
			Debug.Log("switch camera");
			if(onMenuView) {
				firstPersonViewCamera = GameObject.Find("PlayerCharacter").GetComponent<Camera>();
			}
			firstPersonViewCamera.enabled = !firstPersonViewCamera.enabled;
			menuViewCamera.enabled = !menuViewCamera.enabled;
			onMenuView = !onMenuView;
		}
	}
}

