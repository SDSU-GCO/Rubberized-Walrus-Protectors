using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Movement : MonoBehaviour
{
    public float runSpeed = 3;
    public float jumpVelocity = 3;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2;
    public float allowedAirbornTime = .5f;
    public float airbornTime = 0;
    public UnityEvent Jumped;

    [SerializeField, HideInInspector]
    new Rigidbody2D rigidbody2D = null;
    [SerializeField, HideInInspector]
    CapsuleCollider2D capsuleCollider2D = null;
    
    [ReadOnly]
    public bool isGrounded = true;

    private Vector2 totalForce = Vector2.zero;

    private void OnValidate()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        if (capsuleCollider2D == null)
            capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Awake()
    {
        OnValidate();
    }

    Vector2 velocity = new Vector2();
    
    //update loop
    private void Update()
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
            Jumped.Invoke();
        }

        //smart gravity
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * (-9.8f) * (fallMultiplier) * Time.deltaTime;
        }
        else if (rigidbody2D.velocity.y > 0 && Input.GetAxis("Vertical") <= 0)
        {
            rigidbody2D.velocity += Vector2.up * (-9.8f) * (lowJumpMultiplier) * Time.deltaTime;
        }

        airbornTime = Mathf.Min(airbornTime + Time.deltaTime, allowedAirbornTime+1);
    }

    private bool CheckGrounded()
    {
        bool result = false;
        LayerMask layerMask = (1 << 8) | (1 << 9) | (1 << 11); ;
        //layerMask = ~layerMask;
        List<Collider2D> results = new List<Collider2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();
        contactFilter2D.SetLayerMask(layerMask);
        

        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.CapsuleCast(transform.position, capsuleCollider2D.size, capsuleCollider2D.direction, 0, Vector2.down, 1,(int) layerMask);

        if (raycastHit2D.collider != null && raycastHit2D.distance > 0 && raycastHit2D.distance < 0.035)
        {
            result = true;
        }
        

        return result;
    }
}

