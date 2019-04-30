using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class DebugPhotonGUI : MonoBehaviour {
	public void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "  " + PhotonNetwork.GetRoomList().Length);
	}
}