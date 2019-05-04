using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fyp.Game.PlayerControl;
using Leap.Unity.Interaction;
using UnityEngine.Events;

namespace Fyp.Game.Carmera {
	public class PlayerCamera : MonoBehaviour {
		public GameObject player;
		private Vector3 playerHeight = new Vector3((float) 0, (float) 1.7, (float) 0);
        private System.Action swapToWeapon1Action;
        private System.Action swapToWeapon2Action;

        private void Start()
        {
            swapToWeapon1Action += this.swapToWeapon1;
            swapToWeapon2Action += this.swapToWeapon2;

            this.transform.Find("ChooseMelee").GetComponent<InteractionButton>().OnPress = swapToWeapon1Action;
            this.transform.Find("ChooseGun").GetComponent<InteractionButton>().OnPress = swapToWeapon2Action;
        }

        void LateUpdate() {
			if (this.player != null) {
				transform.rotation = player.transform.rotation;
				transform.position = player.transform.position + playerHeight;
				transform.position = transform.position + transform.forward * (float) 0.2;
			}
		}
		public void setCamera(GameObject player) {
			this.player = player;
		}

        private void swapToWeapon1()
        {
            Debug.Log("Gonna switch weapon1");
            player.GetComponent<ControlScript>().SwitchWeapon(0);
        }

        private void swapToWeapon2()
        {
            Debug.Log("Gonna switch weapon2");
            player.GetComponent<ControlScript>().SwitchWeapon(1);
        }

        public void isReady() {
            ControlScript cs = player.GetComponent("ControlScript") as ControlScript;
			cs.ReadyToPlay();
		}
	}
}

