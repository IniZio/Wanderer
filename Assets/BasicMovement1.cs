using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMovement1 : MonoBehaviour
{
    private CharacterController _controller;

    public int Speed;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        move.y -= Physics.gravity.magnitude * Time.deltaTime;
        _controller.Move(move * Time.deltaTime * Speed);
    }

}