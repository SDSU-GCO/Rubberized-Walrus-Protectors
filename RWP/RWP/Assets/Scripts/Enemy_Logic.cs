using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Entity_Logic))]
public class Enemy_Logic : MonoBehaviour
{
    [SerializeField]
#pragma warning disable IDE0044 // Add readonly modifier
    private int damageOnCollision = 1;
#pragma warning restore IDE0044 // Add readonly modifier

    [SerializeField]
    private Attack_Controller rangedAttack = null;

    private void InitializeFromRangedAttack()
    {
        if (rangedAttack != null)
        {
            rangedCoolDownInSeconds = 0;
            rangedCoolDownInSecondsDefault = rangedAttack.AttackDelay;
        }
    }

    [ShowIf("CheckRangedAttackNotNull")]
    public float range = 4;

    [ShowIf("CheckRangedAttackNotNull")]
    public float offset = 1.5f;

    private bool CheckRangedAttackNotNull() => rangedAttack != null;

    private RaycastHit2D result;

    [SerializeField, HideInInspector]
    private EnemyListMBDO enemyListMBDO = null;
    [SerializeField, HideInInspector]
    private PlayerRefMBDO playerRefMBDO = null;

    [SerializeField, HideInInspector]
    private Transform target;
    [SerializeField, HideInInspector]
    private Entity_Logic entityLogic;

    [SerializeField, HideInInspector]
    private float rangedCoolDownInSeconds;
    [SerializeField, HideInInspector]
    private float rangedCoolDownInSecondsDefault;

    private void OnValidate()
    {
        InitializeFromRangedAttack();

        if (enemyListMBDO == null || playerRefMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref playerRefMBDO);
            mBDOInitializationHelper.SetupMBDO(ref enemyListMBDO);
        }

        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

#pragma warning disable IDE0022 // Use expression body for methods
    private void Start()
    {
        target = playerRefMBDO.player;
    }
#pragma warning restore IDE0022 // Use expression body for methods

    private void OnEnable()
    {
        if (enemyListMBDO == null)
        {
            Debug.Log(gameObject.ToString() + " " + this + gameObject.name);
        }

        enemyListMBDO.enemies.Add(this);
        enemyListMBDO.update.Invoke();
        target = playerRefMBDO.player;
    }

    private void OnDisable()
    {
        enemyListMBDO.enemies.Remove(this);
        enemyListMBDO.update.Invoke();
    }

    private void Awake()
    {
        //OnValidate();
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    private void Update()
    {
        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);

        if (InRange() && rangedAttack != null)
        {
            DoAttack();
        }
    }

    public void DoAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            EnemyRangedAttack();
            rangedCoolDownInSeconds = rangedCoolDownInSecondsDefault;
        }
    }

    private bool HasLineOfSight()
    {
        LayerMask layerMask = 1 << 11;
        layerMask = ~layerMask;
        result = Physics2D.Raycast(transform.position, target.position - transform.position, Mathf.Infinity, layerMask);
        return result.collider.gameObject.layer == 10;
    }

    private bool InRange() => (target == null) ? false : (Vector2.Distance(transform.position, target.position) < range);

    //shoot enemy projectile
    private void EnemyRangedAttack()
    {
        if (rangedCoolDownInSeconds == 0)
        {
            Vector2 direction = Vector2.zero;
            direction = ((Vector2)target.position - (Vector2)transform.position).normalized * offset;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 10 && gameObject.layer == 11)
        {
            Entity_Logic temp;
            temp = collision.gameObject.GetComponent<Entity_Logic>();
            if (temp != null)
            {
                temp.TakeDamage(damageOnCollision);
            }
        }
    }
}