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
        setAnimationState(AnimationState.ATTACKING);
    }
    public void setToJump()
    {
        setAnimationState(AnimationState.START_JUMP);
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
    
    enum AnimationState { IDLE_FLOAT = 0, START_JUMP = 1, JUMPING = 2, ATTACKING = 3, FALLING = 4}

    void setAnimationState(AnimationState animationState)
    {
        Animator.SetInteger("MainStage", (int)animationState);
    }

    void Update()
    {
        if (rigidbody2D.velocity.x < -0.00001)
            spriteRenderer.flipX = false;
        if (rigidbody2D.velocity.x > 0.000001)
            spriteRenderer.flipX = true;

        switch((AnimationState)Animator.GetInteger("MainStage"))
        {
            case AnimationState.IDLE_FLOAT:
                if (rigidbody2D.velocity.y < 0)
                {
                    setAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.START_JUMP:
                if (movement.isGrounded)
                {
                    setAnimationState(AnimationState.IDLE_FLOAT);
                }
                else if (rigidbody2D.velocity.y < 0)
                {
                    setAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.JUMPING:
                if (movement.isGrounded)
                {
                    setAnimationState(AnimationState.IDLE_FLOAT);
                }
                else if (rigidbody2D.velocity.y < 0)
                {
                    setAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.ATTACKING:
                if (movement.isGrounded)
                {
                    setAnimationState(AnimationState.IDLE_FLOAT);
                }
                else
                {
                    if (rigidbody2D.velocity.y > 0)
                    {
                        setAnimationState(AnimationState.JUMPING);
                    }
                    else
                    {
                        setAnimationState(AnimationState.FALLING);
                    }
                }
                break;

            case AnimationState.FALLING:
                if (movement.isGrounded)
                {
                    setAnimationState(AnimationState.IDLE_FLOAT);
                }
                break;

        }
        
        if (Input.GetMouseButton(1))
        {
            entityLogic.doAttack();
        }
    }
}
