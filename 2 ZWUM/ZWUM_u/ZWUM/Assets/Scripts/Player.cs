using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public KeyCode leftKey;
    public KeyCode rightKey;
    public float skatingForce;
    public float rotationForce;
    public float maxSpeed;
    public float maxRadiansDelta;


    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = this.transform.GetChild(0).GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 desiredForce = _rb.transform.up * skatingForce;

        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);

        if (Input.GetKey(leftKey) && Input.GetKey(rightKey))
            return;

        if (Input.GetKey(leftKey))
        {
            _rb.AddForce(Vector3.RotateTowards(desiredForce, -_rb.transform.right, maxRadiansDelta, 0f));
            _rb.AddTorque(rotationForce);
        }

        if (Input.GetKey(rightKey))
        {
            _rb.AddForce(Vector3.RotateTowards(desiredForce, _rb.transform.right, maxRadiansDelta, 0f));
            _rb.AddTorque(-rotationForce);
        }
    }
}
