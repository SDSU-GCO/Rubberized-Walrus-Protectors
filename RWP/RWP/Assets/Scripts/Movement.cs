using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float runSpeed = 3;
    public float jumpVelocity = 3;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2;
    public float allowedAirbornTime = .5f;
    public float airbornTime = 0;
    [SerializeField] public UnityEvent upBoy;

    new Rigidbody2D rigidbody2D;
    bool isGrounded = true;
    SpriteRenderer spriteRenderer;

    private Vector2 totalForce = Vector2.zero;

    private void Awake()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    Vector2 velocity = new Vector2();



    //update loop
    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();

        //if (isGrounded)
        //{
        //    spriteRenderer.color = Color.green;
        //}
        //else
        //{
        //    spriteRenderer.color = Color.white;
        //}

        if (isGrounded && Input.GetAxis("Vertical") == 0)
        {
            airbornTime = 0;
            //Debug.Log("reset");
        }
        if (!isGrounded && Input.GetAxis("Vertical") == 0)
        {
            airbornTime = allowedAirbornTime;
        }

        //Debug.Log(Input.GetAxis("Vertical"));
        //run
        velocity = rigidbody2D.velocity;
        velocity.x = Input.GetAxis("Horizontal") * runSpeed;
        rigidbody2D.velocity = velocity;

        
        //jump
        if (Input.GetAxis("Vertical") > 0 && airbornTime <= allowedAirbornTime)
        {
            velocity = rigidbody2D.velocity;
            velocity.y = jumpVelocity;
            rigidbody2D.velocity = velocity;
            upBoy.Invoke();
        }

        //smart gravity
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * (-9.8f) * (fallMultiplier) * 0.02f;

        }
        else if (rigidbody2D.velocity.y > 0 && Input.GetAxis("Vertical") <= 0)
        {
            rigidbody2D.velocity += Vector2.up * (-9.8f) * (lowJumpMultiplier) * 0.02f;
        }

        airbornTime = Mathf.Min(airbornTime + Time.deltaTime, allowedAirbornTime+1);
    }

    private bool CheckGrounded()
    {
        bool result = false;
        LayerMask layerMask = (1 << 9);
        //layerMask = ~layerMask;
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);

        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.CapsuleCast(transform.position, new Vector2(1, 1.9f), CapsuleDirection2D.Vertical, 0, Vector2.down, 1, (1 << 9));
        if (raycastHit2D.distance > 0 && raycastHit2D.distance < 0.05)
        {
            result = true;
        }

        return result;
    }
    
}

