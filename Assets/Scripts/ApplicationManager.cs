using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;

public class ApplicationManager : MonoBehaviour {
	

	public void Quit () 
	{
        Debug.Log("Quit");
#if UNITY_EDITOR
        Debug.Log("Quit");
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
