using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Transform head;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = head.position;
        transform.Rotate(Input.GetAxis("Mouse Y") * 10 * Time.deltaTime,
                        Input.GetAxis("Mouse X") * 10 * Time.deltaTime, 0);
    }
}
