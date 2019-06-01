using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    public float acceleration=0;
    public float jumpForce=0;

    private Vector2 totalForce=Vector2.zero;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigidbody2D.AddForce(totalForce);

    }

    // Update is called once per frame
    void Update()
    {

        totalForce.x = Input.GetAxis("Horizontal")* acceleration;
        if (Input.GetAxis("Vertical") > 0)
            totalForce.y = jumpForce;
        else
            totalForce.y = 0;
    }
}
