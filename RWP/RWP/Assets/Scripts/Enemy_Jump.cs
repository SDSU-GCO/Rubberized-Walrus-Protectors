using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump : MonoBehaviour
{
    public float fallMultiplier=2.5f;
    public float lowJumpMultiplier=2f;
    public float jumpDelay = 3f;
    public float jumpVelocity = 30f;
    private Vector2 velocity;
    public float timePassed = 0f;

    new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

       
        //smart gravity
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * 0.02f;
        }
        else if (rigidbody2D.velocity.y > 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * 0.02f;
        }
   
        //jump
        if (timePassed >= jumpDelay && rigidbody2D.velocity.y <= 0.0000001)
        {
            rigidbody2D.velocity += Vector2.up * jumpVelocity;
            timePassed = 0; 
        }
            
        timePassed += Time.deltaTime;
        
    }
    
}
