using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FYP.Game.Carmera {
	public class Charactor : MonoBehaviour {
		public int speed = 10;
		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update()
		{
			float horizontal = Input.GetAxis("Mouse X") * speed;
			float vertical = Input.GetAxis("Mouse Y") * speed;

			transform.Rotate(0f, horizontal, 0f, Space.World);
			transform.Rotate(-vertical, 0f, 0f, Space.Self);
		}
	}
}

