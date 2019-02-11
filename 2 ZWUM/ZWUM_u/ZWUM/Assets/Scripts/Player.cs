using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Input")]
    public KeyCode leftKey;
    public KeyCode rightKey;

    [Header("Audio")]
    public string honk;

    [Header("Forces")]
    public float skatingForce;
    public float rotationForce;
    public float maxSpeed;
    public float maxRadiansDelta;
    public float stickForce;

    private AudioManager _audioMan;
    private Rigidbody2D _rbBody;
    private Rigidbody2D _rbStick;


    private void Start()
    {
        _rbBody = this.transform.GetChild(0).GetComponent<Rigidbody2D>();
        _rbStick = this.transform.GetChild(1).GetComponent<Rigidbody2D>();
        _audioMan = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        Vector3 desiredForce = _rbBody.transform.up * skatingForce;

        _rbBody.velocity = Vector2.ClampMagnitude(_rbBody.velocity, maxSpeed);

        if (Input.GetKey(leftKey) && Input.GetKey(rightKey))
            return;

        if (Input.GetKey(leftKey))
        {
            //Add turning force and velocity force to body
            _rbBody.AddForce(Vector3.RotateTowards(desiredForce, -_rbBody.transform.right, maxRadiansDelta, 0f));
            _rbBody.AddTorque(rotationForce);

            //Add force to stick
            _rbStick.AddForceAtPosition(_rbStick.transform.up * -stickForce, _rbStick.transform.position);
        }

        if (Input.GetKey(rightKey))
        {
            //Add turning force and velocity force to body
            _rbBody.AddForce(Vector3.RotateTowards(desiredForce, _rbBody.transform.right, maxRadiansDelta, 0f));
            _rbBody.AddTorque(-rotationForce);

            //Add force to stick
            _rbStick.AddForceAtPosition(_rbStick.transform.up * stickForce, _rbStick.transform.position);
        }
    }

    public void Honk()
    {
        _audioMan.Play(honk);
    }
}
