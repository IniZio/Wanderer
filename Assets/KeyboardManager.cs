using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

public class KeyboardManager : MonoBehaviour {
    
    string word = null;
    int wordIndex = 0;
    string alpha;
    public Text myName = null;
    public TextMesh Name = null;

    public void alphabetFunction(string alphabet){

        wordIndex++;
        word = word + alphabet;
        myName.text = word;
        Name.text = word;
        
    }


}
