using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Fyp.Game.PlayerCharacter {
	public class PlayerData : ScriptableObject {
		private string roomName;
		private int dayNumber;
		private bool isMaster;

		public PlayerData(string roomName, int dayNumber) {
			this.roomName = roomName;
			this.dayNumber = dayNumber;
		}

		public void nextDay() {
			this.dayNumber += 1;
		}

		public string getRoomName() {
			return this.roomName;
		}
	}
}