using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Fyp.Game.PlayerCharacter;

namespace Fyp.Game.Save {
	public class FileIO {
		public static void WriteSave(PlayerData po) {
			string path = Path.Combine(Application.persistentDataPath, po.getRoomName() + ".txt");
			string jsonString = JsonUtility.ToJson (po);
			using (StreamWriter streamWriter = File.CreateText (path)) {
				streamWriter.Write (jsonString);
        	}
		}

		public static void ReadSave(string path) {
			try {
				StreamReader reader = new StreamReader(path);
				string content = reader.ReadToEnd();
				reader.Close();
				Debug.Log(content);
			}
			catch (System.Exception err) {
				Debug.Log("Read file error: " + err.ToString());
			}
		}
	}
}
