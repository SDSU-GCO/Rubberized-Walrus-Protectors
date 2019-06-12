using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;
using System.Linq;
using NaughtyAttributes;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Movement))]
public class Ally_Logic : MonoBehaviour
{
    [SerializeField]
    Attack_Controller rangedAttack;

    [SerializeField, HideInInspector]
    new Rigidbody2D rigidbody2D;
    [SerializeField, HideInInspector]
    public SpriteRenderer spriteRenderer;
    [SerializeField, HideInInspector]
    public Animator animator;
    [SerializeField, HideInInspector]
    public Movement movement;

    [SerializeField, HideInInspector]
    public PlayerRefMBDO playerRefMBDO = null;
    

    bool CheckRangedAttackNotNull()
    {
        return rangedAttack != null;
    }


    [ShowIf("CheckRangedAttackNotNull")]
    public float offset = 1.5f;

    [SerializeField, HideInInspector]
#pragma warning disable IDE0044 // Add readonly modifier
    int damage;
#pragma warning restore IDE0044 // Add readonly modifier
    [SerializeField, HideInInspector]
    float rangedCoolDownInSeconds;
    [SerializeField, HideInInspector]
#pragma warning disable IDE0044 // Add readonly modifier
    float rangedCoolDownInSecondsDefault;
#pragma warning restore IDE0044 // Add readonly modifier

    private void OnValidate()
    {
        if (rigidbody2D == null)
            rigidbody2D = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (movement == null)
            movement = GetComponent<Movement>();


        InitializeFromRangedAttack();
        if(playerRefMBDO==null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref playerRefMBDO);
        }
        
    }

    void InitializeFromRangedAttack()
    {
        if (rangedAttack != null)
        {
            rangedCoolDownInSecondsDefault = rangedCoolDownInSeconds = rangedAttack.AttackDelay;
            damage = rangedAttack.damage;
        }
    }

    public void SetToAttack()
    {
        SetAnimationState(AnimationState.ATTACKING);
    }
    public void SetToJump()
    {
        SetAnimationState(AnimationState.START_JUMP);
    }

    private void Awake()
    {
        OnValidate();

        playerRefMBDO.player = transform;
        playerRefMBDO.update.Invoke();
    }
    
    enum AnimationState { IDLE_FLOAT = 0, START_JUMP = 1, JUMPING = 2, ATTACKING = 3, FALLING = 4}

    void SetAnimationState(AnimationState animationState)
    {
        animator.SetInteger("MainStage", (int)animationState);
    }

    bool wasAttacking=false;

    void FlipWithVelocity()
    {
        if (rigidbody2D.velocity.x < -0.00001)
            spriteRenderer.flipX = false;
        else if (rigidbody2D.velocity.x > 0.000001)
            spriteRenderer.flipX = true;
    }

    void Update()
    {
        switch ((AnimationState)animator.GetInteger("MainStage"))
        {
            case AnimationState.IDLE_FLOAT:
                FlipWithVelocity();
                if (rigidbody2D.velocity.y < 0 && movement.isGrounded!=true)
                {
                    SetAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.START_JUMP:
                FlipWithVelocity();
                if (movement.isGrounded)
                {
                    SetAnimationState(AnimationState.IDLE_FLOAT);
                }
                else if (rigidbody2D.velocity.y < 0)
                {
                    SetAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.JUMPING:
                FlipWithVelocity();
                if (movement.isGrounded)
                {
                    SetAnimationState(AnimationState.IDLE_FLOAT);
                }
                else if (rigidbody2D.velocity.y < 0)
                {
                    SetAnimationState(AnimationState.FALLING);
                }
                break;

            case AnimationState.ATTACKING:

                if(wasAttacking)//ensure we attack for at least one frame to enter state.
                {
                    if (movement.isGrounded)
                    {
                        SetAnimationState(AnimationState.IDLE_FLOAT);
                    }
                    else
                    {
                        if (rigidbody2D.velocity.y < 0)
                        {
                            SetAnimationState(AnimationState.FALLING);
                        }
                        else
                        {
                            SetAnimationState(AnimationState.JUMPING);
                        }
                    }
                }
                wasAttacking = true;
                
                break;

            case AnimationState.FALLING:
                FlipWithVelocity();
                if (movement.isGrounded)
                {
                    SetAnimationState(AnimationState.IDLE_FLOAT);
                }
                break;

        }

        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);
        if (Input.GetMouseButton(1))
        {
            PlayerRangedAttack();
        }
    }

    //shoot player projectile
    void PlayerRangedAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseposition = (mouseposition - (Vector2)transform.position).normalized * offset;

            SetToAttack();

            if (mouseposition.x > 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;

            GameObject childInstance = Instantiate(rangedAttack.gameObject, mouseposition + (Vector2)transform.position, transform.rotation);
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * mouseposition.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
        }
    }
}
