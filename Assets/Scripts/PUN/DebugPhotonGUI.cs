using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class DebugPhotonGUI : MonoBehaviour {
	public void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString() + "  " + Input.GetAxis("LeftVRTriggerAxis").ToString() + "  " + Input.GetAxis("RightVRTriggerAxis").ToString());
	}
}