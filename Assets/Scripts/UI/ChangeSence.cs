using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fyp.Game.UI {
	public class ChangeSence : MonoBehaviour {
		public void MenuToWaitingRoom() {
			Debug.Log("MenuToWaitingRoom");
			SceneManager.LoadScene("WaitingRoom");
		}

		public void WaitingRoomToMenu() {
			Debug.Log("WaitingRoomToMenu");
			SceneManager.LoadScene("MainMenu");
		}
	}
}

