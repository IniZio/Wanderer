using UnityEngine;
using System.Collections;

public class LODcontrol : MonoBehaviour {
	public float[] distRanges;
	public GameObject[] lodModels;
	public Camera playerCam;

	private int current = -2;

	// Use this for initialization
	void Start () {
		for (var i = 0; i< lodModels.Length; i++) {
			lodModels[i].SetActive(false);		
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance (playerCam.transform.position, transform.position);
		int level = -1;
		for (var i = 0; i< distRanges.Length; i++) {
			if(dist < distRanges[i]){
				level = i;
				i = distRanges.Length;
			}		
		}
		if (level == -1) {
			level = distRanges.Length;		
		}
		if (current != level) {
			ChangeLod(level);		
		}
	}
	void ChangeLod(int level){
		lodModels[level].SetActive(true);
		if(current>=0){
			lodModels[current].SetActive(false);
		}
		current = level;
	}
}
