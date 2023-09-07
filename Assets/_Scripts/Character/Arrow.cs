using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Arrow : MonoBehaviour
{
    [FormerlySerializedAs("arrowBodyRigidbody2D")]
    public Rigidbody2D rigidbody2D;

    public bool isShooting;

    private float _angle;
    // public Rigidbody2D headRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isShooting)
        {
            RotateArrow();
        }
    }

    private void RotateArrow()
    {
        _angle = Mathf.Atan2(-rigidbody2D.velocity.y, -rigidbody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit Target");

        if (other.gameObject.tag == "Target")
        {
            Debug.Log("Hit Target");
            transform.SetParent(null);
            rigidbody2D.simulated = false;
        }
    }
}