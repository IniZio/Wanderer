using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fyp.Game.Network {
	public class PunNetworkManager : MonoBehaviour {
		#region Singleton Code
		protected static PunNetworkManager Instance = null;

		void Awake() {
			if (Instance == null) {
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else {
				Destroy(gameObject);
			}
		}
		#endregion

	}
}
