using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Movement))]
public class Ally_Logic : MonoBehaviour
{
    [SerializeField]
    private Attack_Controller rangedAttack = null;

    [SerializeField, HideInInspector]
    private new Rigidbody2D rigidbody2D;

    [SerializeField, HideInInspector]
    public SpriteRenderer spriteRenderer;

    [SerializeField, HideInInspector]
    public Animator animator;

    [SerializeField, HideInInspector]
    public Movement movement;

    [SerializeField, HideInInspector]
    private PlayerRefMBDO playerRefMBDO;

    private bool CheckRangedAttackNotNull() => rangedAttack != null;

    [ShowIf("CheckRangedAttackNotNull")]
    public float offset = 1.5f;

    [SerializeField, HideInInspector]
    private int damage;

    [SerializeField, HideInInspector]
    private float rangedCoolDownInSeconds = 0;

    [SerializeField, HideInInspector]
    private float rangedCoolDownInSecondsDefault;

    private void OnValidate()
    {
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (movement == null)
        {
            movement = GetComponent<Movement>();
        }

        InitializeFromRangedAttack();
        if (playerRefMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);
            mBDOInitializationHelper.SetupMBDO(ref playerRefMBDO);
        }
    }

    private void InitializeFromRangedAttack()
    {
        if (rangedAttack != null)
        {
            rangedCoolDownInSecondsDefault = rangedAttack.AttackDelay;
            damage = rangedAttack.damage;
        }
    }

    public void SetToAttack() => SetAnimationState(AnimationState.ATTACKING);

    public void SetToJump() => SetAnimationState(AnimationState.START_JUMP);

    private void Awake()
    {
        playerRefMBDO.player = transform;
        playerRefMBDO.update.Invoke();
    }

    private enum AnimationState
    {
        IDLE_FLOAT = 0, START_JUMP = 1, JUMPING = 2, ATTACKING = 3, FALLING = 4
    }

#pragma warning disable IDE0022 // Use expression body for methods

    private void SetAnimationState(AnimationState animationState)
    {
        animator.SetInteger("MainStage", (int)animationState);
    }

#pragma warning restore IDE0022 // Use expression body for methods

    private bool wasAttacking = false;

    private void FlipWithVelocity()
    {
        if (rigidbody2D.velocity.x < -0.00001)
        {
            spriteRenderer.flipX = false;
        }
        else if (rigidbody2D.velocity.x > 0.000001)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void Update()
    {
        switch ((AnimationState)animator.GetInteger("MainStage"))
        {
            case AnimationState.IDLE_FLOAT:
                FlipWithVelocity();
                if (rigidbody2D.velocity.y < 0 && movement.isGrounded != true)
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

                if (wasAttacking)//ensure we attack for at least one frame to enter state.
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
    private void PlayerRangedAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

            Vector2 mouseposition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            mouseposition = (mouseposition - (Vector2)transform.position).normalized * offset;

            SetToAttack();

            spriteRenderer.flipX = mouseposition.x > 0;

            GameObject childInstance = Instantiate(rangedAttack.gameObject, mouseposition + (Vector2)transform.position, transform.rotation);

            Vector3 temp = childInstance.transform.position;
            temp.z = transform.position.z;
            childInstance.transform.position = temp;
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * mouseposition.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
        }
    }
}