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
    CircleCollider2D circleCollider2D;

    new Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
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

    private bool CheckGrounded()
    {
        bool result = false;
        LayerMask layerMask = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 11) | (1 << 12);
        //layerMask = ~layerMask;
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);

        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.CircleCast(transform.position, circleCollider2D.radius*0.9f, Vector2.down, Mathf.Infinity, layerMask);
        if (raycastHit2D.distance > 0 && raycastHit2D.distance < 0.05)
        {
            result = true;
        }

        return result;
    }

}
