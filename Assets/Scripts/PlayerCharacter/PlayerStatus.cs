using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace Fyp.Game.PlayerControl {
	public class PlayerStatus {
		public bool readyForPlay = false;
		public bool isMaster;

		public PlayerStatus(bool isMaster) {
			this.isMaster = isMaster;
		}

		public PlayerStatus() {
			this.isMaster = false;
		}

		public void setIsMaster(bool isMaster) {
			this.isMaster = isMaster;
		}

		public void ReadyToPlay() {
			this.readyForPlay = !this.readyForPlay;
		}
		public void setIsReady(bool isReady) {
			this.readyForPlay = isReady;
		}

		public bool isReady() {
			return this.readyForPlay;
		}

		public bool isMasterPlayer() {
			return this.isMaster;
		}
	}
}
