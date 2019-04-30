using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Fyp.Game.PlayerControl;
using Fyp.Game.Network;

namespace Fyp.Game.UI {
	public class WaitingRoomSceneManager : UnityEngine.MonoBehaviour {
		GameObject Player1, Player2;
		Door door;
		public PanelManager savePanel;

		void Start () {
            this.door = GameObject.FindWithTag("WaitingRmDoor").GetComponent("Door") as Door;
			savePanel.CloseCurrent();
        }

		void Update() {
			if (Player1 != null && Player2 !=  null) {
				ControlScript script1 = Player1.GetComponent("ControlScript") as ControlScript;
				ControlScript script2 = Player2.GetComponent("ControlScript") as ControlScript;
				if (script1.isReady && script2.isReady) {
					if (!door.isOpen) {
						door.OpenDoor();
					}
				}
				else {
					if (door.isOpen) {
						door.CloseDoor();
					}
				}
				if (script1.getStandingWaitingRmDoor() && script2.getStandingWaitingRmDoor()) {
					script1.exitWaitingRmDoor();
					script2.exitWaitingRmDoor();
					NetworkChangeScene.AllPlayerChangeScene("BaseNew");
				}
			}
			if (Player1 == null) {
				GameObject temp = GameObject.FindWithTag("Player1Character");
				if (temp != null) {
					Player1 = temp;
				}
			}
			if (Player2 == null) {
				GameObject temp = GameObject.FindWithTag("Player2Character");
				if (temp != null) {
					Player2 = temp;
				}
			}
		}

		public void setPlayer(bool isMaster, ref GameObject player) {
			if (isMaster) {
				this.Player1 = player;
			}
			else {
				this.Player2 = player;
			}
		}
 	}
}
