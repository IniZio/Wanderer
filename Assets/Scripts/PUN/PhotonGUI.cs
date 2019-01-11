using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PhotonGUI : MonoBehaviour {
	public void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}
}