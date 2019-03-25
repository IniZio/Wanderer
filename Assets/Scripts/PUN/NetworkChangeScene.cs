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
			PhotonNetwork.automaticallySyncScene = true;

			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.LoadLevelAsync(sceneName);
			}
		}
	}
}