using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.PlayerCharacter{
	public class PlayerController : MonoBehaviour {
		public float walkSpeed = 100;

		Rigidbody rb;
		Vector3 moveDirection;

		void Awake() {
			this.rb = GetComponent<Rigidbody>();
		}

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
			float horizontalMovement = Input.GetAxis("Horizontal") * Time.deltaTime;
			float verticalMovement = Input.GetAxis("Vertical") * Time.deltaTime;
			transform.Translate(0, 0, verticalMovement);
			transform.Translate(0, horizontalMovement, 0);
		}
	}
}
