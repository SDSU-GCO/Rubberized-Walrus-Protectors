using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity_Logic : MonoBehaviour
{
    public Event_One_Float damaged = new Event_One_Float();
    //entity parameters
    public bool disableColliderOnDeath = true;
    public float health = 3f;
    public Attack_Controller rangedAttack;
    public float offset = 1.5f;
    public UnityEvent attacked;
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

    public GameObject onDeathReplaceWith;

    [HideInInspector]
    public int damage;

    SpriteRenderer spriteRenderer;
    float rangedCoolDownInSeconds;
    float rangedCoolDownInSecondsDefault;
    Rigidbody2D my2DRigidbody;
    Transform playerPosition;

    delegate void attackMethod();
    attackMethod attack_method;
    //initialize ambiguous parameters
    public void Awake()
    {
        OnValidate();
        Debug.Assert(rangedAttack != null, "Error: rangedAttack in \"" + this + "\"is null!");
        rangedCoolDownInSeconds = rangedAttack.AttackDelay;
        rangedCoolDownInSecondsDefault = rangedCoolDownInSeconds;
        my2DRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        damage = rangedAttack.damage;

        if (gameObject.layer == 10)
        {
            attack_method = PlayerRangedAttack;
        }
        else if (gameObject.layer == 11)
        {
            attack_method = EnemyRangedAttack;
        }
        
    }

    private void Start()
    {

        playerPosition = playerRefSO.player;
    }

    void OnEnable()
    {

        if (playerRefSO == null)
            Debug.Log(gameObject.ToString() + " " + this + gameObject.name);
        playerPosition = playerRefSO.player;
        damaged.Invoke(health);
    }

    
    void Update()
    {

        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);
        invincibility = Mathf.Min(invincibilityTime, invincibility + Time.deltaTime);

    }

    public void doAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            attack_method();
            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
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


            attacked.Invoke();

            if (mouseposition.x > 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;

            GameObject childInstance = Instantiate(rangedAttack.gameObject, mouseposition + (Vector2)transform.position, transform.rotation);
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * mouseposition.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;

        }

    }
    //shoot enemy projectile
    void EnemyRangedAttack()
    {

        if (rangedCoolDownInSeconds == 0)
        {

            Vector2 direction = new Vector2();

            direction = ((Vector2)playerPosition.position - (Vector2)transform.position).normalized * offset;

            float rotation = Mathf.Rad2Deg * (Mathf.Atan(direction.y / direction.x));
            rotation += -90;
            if (direction.x < 0)
            {
                rotation += 180;
            }

            

            GameObject childInstance = Instantiate(rangedAttack.gameObject, direction + (Vector2)transform.position, Quaternion.Euler(0, 0, rotation));
            childInstance.GetComponent<Rigidbody2D>().velocity = rangedAttack.speed * direction.normalized;

            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;

        }

    }
    float invincibility = 0;
    public float invincibilityTime = 0.5f;
    public float timeToFlashOnHit = 0.5f;

    //take damage function
    public void TakeDamage(float amount)
    {
        if (invincibility >= invincibilityTime)
        {
            health -= amount;
            invincibility = 0;

            damaged.Invoke(health);
            if (health <= 0)
            {
                Enemy_Logic tmp = GetComponent<Enemy_Logic>();
                if(tmp!=null)
                    tmp.enabled=false;

                if(gameObject.layer==11 && onDeathReplaceWith==null)
                {
                    gameObject.layer = 12;
                    Animator tempAnimator = GetComponent<Animator>();
                    if (tempAnimator != null)
                        tempAnimator.SetBool("isSaved", true);
                    if(disableColliderOnDeath)
                        GetComponent<Collider2D>().enabled = false;
                }
                else if(onDeathReplaceWith!=null)
                {
                    Instantiate(onDeathReplaceWith, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(ChangeColor());
            }
        }
    }

    enum GoToColor { red, white};
    public float flashSpeed = 20f;
    IEnumerator ChangeColor()
    {
        float flashingTime = timeToFlashOnHit;
        GoToColor goToColor = GoToColor.red;
        float amount = 0;
        while(flashingTime>0)
        {
            flashingTime -= Time.deltaTime;
            if (goToColor == GoToColor.red)
                amount += Time.deltaTime * flashSpeed;
            else
                amount -= Time.deltaTime * flashSpeed;

            if (amount >= 1)
                goToColor = GoToColor.white;
            else if (amount <= 0)
                goToColor = GoToColor.red;

            spriteRenderer.color = Color.Lerp(Color.white, Color.red, amount);

            yield return null;
        }
        spriteRenderer.color = Color.white;
    }
    
}
[System.Serializable]
public class Event_One_Float : UnityEvent<float>
{

}