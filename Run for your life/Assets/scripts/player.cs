using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 1000000000;
    [SerializeField] private float _turnSpeed = 360;

    private Vector3 _input;

    private void Update()
    {
        GatherInput();
        Look();
    }

    void Look()
    {
        if (_input != Vector3.zero)
        {


            var relative = (transform.position + _input) - transform.position;
            var rot = Quaternion.LookRotation(relative, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        Move();
    }
    void GatherInput()
    {
        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }


    void Move()
    {
        _rb.MovePosition(transform.position + (transform.forward * _input.magnitude) * Time.deltaTime);
    }
}

