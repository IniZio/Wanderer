using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Fyp.Game.Save {
	public class GameSave : ScriptableObject  {
		#region Singleton Code
		protected static GameSave sInstance = null;

		public static GameSave Instance {
			get {
				if(sInstance == null) {
					sInstance = ScriptableObject.CreateInstance<GameSave>();
					sInstance.hideFlags = HideFlags.HideAndDontSave;
				}
				return sInstance;
			}
		}
		#endregion

		// void SaveGame() {
		// 	FileIO.WriteSave();
		// }
	}
}
