using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fyp.Game.UI {
	public class ChangeSence : MonoBehaviour {
		public static void MenuToWaitingRoom() {
			Debug.Log("MenuToWaitingRoom");
			SceneManager.LoadScene("WaitingRoom");
		}

		public static void WaitingRoomToMenu() {
			Debug.Log("WaitingRoomToMenu");
			SceneManager.LoadScene("MainMenu");
		}

		public static void WaitingRoomToBase() {
			Debug.Log("WaitingRoomToBase");
			SceneManager.LoadScene("BaseNew");
		}

		public void MenuToTut() {
			Debug.Log("MenuToTut");
			SceneManager.LoadScene("Tutorial");
		}
	}
}

