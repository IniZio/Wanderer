using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Fyp.Constant;

namespace Fyp.Game.Network {
	public class NetworkChangeScene : Photon.MonoBehaviour {
		public static void ChangeToScene(string sceneName) {
			SceneManager.LoadScene(sceneName);
		}

		public static void AllPlayerChangeScene(string sceneName) {
			if (PhotonNetwork.isMasterClient) {
				if (PhotonNetwork.isMasterClient) {
					SceneManager.LoadScene(sceneName);
				}
			}
		}
	}
}