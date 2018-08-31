using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationControll : MonoBehaviour {
    public Rigidbody rb;
    public float RotateSpeed = 10f;

    // Use this for initialization
    void Start () {
		
	}
	
	void FixedUpdate() {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(-Vector3.up * RotateSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }
}
