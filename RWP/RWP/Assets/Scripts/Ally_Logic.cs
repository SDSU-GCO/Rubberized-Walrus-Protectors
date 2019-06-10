using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;

public class Ally_Logic : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    Entity_Logic entityLogic;
    public SpriteRenderer spriteRenderer;
    public Animator Animator;
    public Movement movement;
    public PlayerRefSO playerRefSO;

    private void OnValidate()
    {
        if (PrefabUtility.IsPartOfPrefabInstance(gameObject) == true)
        {
            if (playerRefSO == null)
            {
                GameObject CardinalSubsystem = GameObject.Find("Cardinal Subsystem");
                ScriptableObjectReferences scriptableObjectReferences = null;
                if (CardinalSubsystem != null)
                    scriptableObjectReferences = CardinalSubsystem.GetComponent<ScriptableObjectReferences>();
                if (playerRefSO == null)
                {
                    scriptableObjectReferences.tryPopulate(ref playerRefSO);
                    if (playerRefSO == null)
                        Debug.LogWarning("ScriptableObject Object playerRefSO: " + playerRefSO + "is null in: " + this);
                }
            }
        }
        else
        {
            playerRefSO = null;
        }
    }
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
        OnValidate();

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

    bool wasAttacking=false;

    void flipWithVelocity()
    {
        if (rigidbody2D.velocity.x < -0.00001)
            spriteRenderer.flipX = false;
        else if (rigidbody2D.velocity.x > 0.000001)
            spriteRenderer.flipX = true;
    }

    void Update()
    {
        switch ((AnimationState)Animator.GetInteger("MainStage"))
        {
            case AnimationState.IDLE_FLOAT:
                flipWithVelocity();
                if (rigidbody2D.velocity.y < 0 && movement.isGrounded!=true)
                {
                    setAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.START_JUMP:
                flipWithVelocity();
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
                flipWithVelocity();
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

                if(wasAttacking)//ensure we attack for at least one frame to enter state.
                {
                    if (movement.isGrounded)
                    {
                        setAnimationState(AnimationState.IDLE_FLOAT);
                    }
                    else
                    {
                        if (rigidbody2D.velocity.y < 0)
                        {
                            setAnimationState(AnimationState.FALLING);
                        }
                        else
                        {
                            setAnimationState(AnimationState.JUMPING);
                        }
                    }
                }
                wasAttacking = true;
                
                break;

            case AnimationState.FALLING:
                flipWithVelocity();
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
