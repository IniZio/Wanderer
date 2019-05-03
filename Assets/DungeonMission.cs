using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMission : MonoBehaviour
{
       IEnumerator ExecuteAfterTime(float time)
            {
                print("start wait");
                yield return new WaitForSeconds(time);
                print("start wait");

             }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(2));
        print("wait 2s");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
