using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class twst : MonoBehaviour {

    private InteractionBehaviour _intObj;

    public void Quit() {
        _intObj = GetComponent<InteractionBehaviour>();
        Debug.Log("ok");

    }

}
