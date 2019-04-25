using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public float speed = 3.0f;
    public float jumpSpeed = 5.0f;
    public float gravity = 10.0f;
    public float rotateSpeed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate() {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded) {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        transform.Rotate(0, Input.GetAxis("Mouse X") * 10 * Time.deltaTime, 0);

    }
}
