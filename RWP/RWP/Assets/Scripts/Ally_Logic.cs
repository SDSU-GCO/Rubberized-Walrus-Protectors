using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally_Logic : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    Entity_Logic entityLogic;
    public SpriteRenderer spriteRenderer;
    public Animator Animator;
    public Movement movement;
    public PlayerRefSO playerRefSO;

    public void setToAttack()
    {
        Animator.SetInteger("MainStage", 3);
    }
    public void setToJump()
    {
        Animator.SetInteger("MainStage", 1);
    }

    private void Awake()
    {
        playerRefSO.player = transform;
        
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }
    
    void Update()
    {
        if (rigidbody2D.velocity.x < -0.00001)
            spriteRenderer.flipX = false;
        if (rigidbody2D.velocity.x > 0.000001)
            spriteRenderer.flipX = true;

        if (Animator.GetInteger("MainStage")==3 && movement.isGrounded)
            Animator.SetInteger("MainStage", 0);

        if (Animator.GetInteger("MainStage") == 2 && rigidbody2D.velocity.y<0)
            Animator.SetInteger("MainStage", 4);

        if (Animator.GetInteger("MainStage") == 0 && rigidbody2D.velocity.y < 0)
            Animator.SetInteger("MainStage", 4);

        if (Animator.GetInteger("MainStage") == 2 && movement.isGrounded)
            Animator.SetInteger("MainStage", 0);


        if (Animator.GetInteger("MainStage") == 4 && movement.isGrounded)
            Animator.SetInteger("MainStage", 0);

        if (Animator.GetInteger("MainStage") == 3 && !movement.isGrounded)
        {
            if(rigidbody2D.velocity.y>0)
            {
                Animator.SetInteger("MainStage", 2);
            }
            else
            {
                Animator.SetInteger("MainStage", 4);
            }
        }



        if (rigidbody2D.velocity.y<0 && Animator.GetInteger("MainStage") == 1)
        {
            Animator.SetInteger("MainStage", 4);
        }

        if (movement.isGrounded && Animator.GetInteger("MainStage") == 1)
        {
            Animator.SetInteger("MainStage", 0);
        }


        if (Input.GetMouseButton(1))
        {
            entityLogic.doAttack();
        }

        
    }
}
