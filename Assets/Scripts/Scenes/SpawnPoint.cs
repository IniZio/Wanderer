using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.UI{
	public class SpawnPoint : MonoBehaviour {
		public GameObject effect;
		public Light light;

		// Use this for initialization
		void Start () {
			this.light.color = Color.gray;
			this.effect.SetActive(false);
		}

		public void onSpawnPlayer() {
			this.light.color = Color.cyan;
			// this.effect.SetActive(true);
		}
	}
}