﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    public float runSpeed = 3;
    public float jumpVelocity=3;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2;
    public float allowedAirbornTime = .5f;
    float airbornTime = 0;
    bool cstay = true;
    public CircleCollider2D circleCollider2D;
    float oldDistanceToGround;

    private Vector2 totalForce=Vector2.zero;

    private void Awake()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        if (circleCollider2D == null)
            circleCollider2D = GetComponent<CircleCollider2D>();
    }

    Vector2 velocity = new Vector2();
    
    //update loop
    private void FixedUpdate()
    {

        if(cstay&&IsGrounded())
        {
            airbornTime = 0;
        }
        //run
        velocity = rigidbody2D.velocity;
        velocity.x = Input.GetAxis("Horizontal") * runSpeed;
        rigidbody2D.velocity = velocity;
        
        //jump
        if(Input.GetAxis("Vertical") > 0 && airbornTime<=allowedAirbornTime)
        {
            rigidbody2D.velocity += Vector2.up * jumpVelocity;
        }

        //smart gravity
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * 0.02f;
        }
        else if (rigidbody2D.velocity.y > 0 && Input.GetAxis("Vertical") <= 0)
        {
            airbornTime = allowedAirbornTime;
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * 0.02f;
        }

        airbornTime += Time.deltaTime;
       
    }

    private bool IsGrounded()
    {
        bool result = false;
        LayerMask layerMask = (1 << 9);
        //layerMask = ~layerMask;
        RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position, circleCollider2D.radius, Vector2.down, Mathf.Infinity, layerMask);
        if(Mathf.Abs(raycastHit2D.distance - oldDistanceToGround)<0.00001)
        {
            result = true;
        }
        oldDistanceToGround = raycastHit2D.distance;


        LayerMask layerMaskCircle = (1 << 9);
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMaskCircle);
        List<Collider2D> col=new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, 0.480f, contactFilter2D, col);
        if (col.Count>0)
        {
            result = false;
        }
        oldDistanceToGround = raycastHit2D.distance;
        
        return result;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            cstay = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            cstay = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            cstay = false;
        }
    }


}
