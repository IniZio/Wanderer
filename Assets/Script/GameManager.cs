using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerCharacter;

namespace Fyp.Game.Manager {
	public class GameManager : MonoBehaviour {

		public static GameManager Instance = null;                         

		void Awake() {

			if (Instance == null) {
				Instance = this;
			}

			else if (Instance != this) {
				Destroy(gameObject);
			}

			DontDestroyOnLoad(gameObject);
		}
		public PlayerMainObj Player;
	}
}
