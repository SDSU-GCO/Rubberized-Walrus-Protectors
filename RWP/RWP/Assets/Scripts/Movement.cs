using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    bool tstay = true;
    [SerializeField] public UnityEvent upBoy;

    private Vector2 totalForce=Vector2.zero;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    Vector2 velocity = new Vector2();
    
    //update loop
    private void FixedUpdate()
    {

        if(cstay&&tstay&&(Mathf.Abs( rigidbody2D.velocity.y))<0.00001)
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
            //cstay = false;
            //tstay = false;
            upBoy.Invoke();
        }

        //smart gravity
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            
        }
        else if (rigidbody2D.velocity.y > 0 && Input.GetAxis("Vertical") <= 0)
        {
            airbornTime = allowedAirbornTime;
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        airbornTime += Time.deltaTime;
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==8)
            tstay = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
            tstay = false;
    }
}
