using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fyp.Game.Save {
	public class FileIO {
		public static void WriteSave(string path, string content) {
			StreamWriter writer = new StreamWriter(path);
			writer.Write(content);

			writer.Close();
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
