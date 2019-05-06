using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick : MonoBehaviour
{

    private bool picking = false;

    void Start()
    {

    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        var tag = col.gameObject.tag;
        Debug.Log("col");
        if (tag == "Player")
        {
            PlayerControl3 script = col.gameObject.GetComponent("PlayerControl3") as PlayerControl3;
            Debug.Log("player");
            if (!picking)
            {
                picking = true;
                script.pick(this.gameObject);
            }
        }
    }

    void OnCollisionExit(Collision collision) { }

    void OnCollisionStay(Collision collision) { }
}