using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy_Sideways : MonoBehaviour
{
    public float moveDelay = 3f;
    new Rigidbody2D rigidbody2D;
    private Vector2 velocity;
    public float moveVelocity = 5f;
    public float timePassed;
    bool switchDirection=false;
    public float idle=6f;
    public SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        //move
        if (switchDirection == true && idle >= (moveDelay*2))
        {
            moveVelocity = -moveVelocity;
            idle = 0;
        }
        else
        {
            rigidbody2D.velocity += Vector2.left*0;
        }

        if (timePassed >= moveDelay && rigidbody2D.velocity.x <= 0.0000001)
        {
            rigidbody2D.velocity += Vector2.left* moveVelocity;
            timePassed = 0;
            
            switchDirection = true;
            
        }
        else
        {
            switchDirection = false;
        }

        timePassed += Time.deltaTime;
        idle += Time.deltaTime;

        if (rigidbody2D.velocity.x > 0)
            spriteRenderer.flipX = false;
        if (rigidbody2D.velocity.x < 0)
            spriteRenderer.flipX = true;
    }
    
        
}
