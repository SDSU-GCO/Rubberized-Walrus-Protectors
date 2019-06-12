using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;
using NaughtyAttributes;

[RequireComponent(typeof(Entity_Logic))]
public class Enemy_Logic : MonoBehaviour
{
    [SerializeField]
    Attack_Controller rangedAttack;
    void InitializeFromRangedAttack()
    {
        if (rangedAttack != null)
        {
            rangedCoolDownInSecondsDefault = rangedCoolDownInSeconds = rangedAttack.AttackDelay;
            damage = rangedAttack.damage;
        }
    }

    [ShowIf("CheckRangedAttackNotNull")]
    public float range = 4;
    [ShowIf("CheckRangedAttackNotNull")]
    public float offset = 1.5f;

    bool CheckRangedAttackNotNull()
    {
        return rangedAttack != null;
    }

    RaycastHit2D result;


    [SerializeField, HideInInspector]
    EnemyListMBDO enemyListMBDO = null;
    [SerializeField, HideInInspector]
    PlayerRefMBDO playerRefMBDO = null;
    
    [SerializeField, HideInInspector]
    Transform target;

    [SerializeField, HideInInspector]
    Entity_Logic entityLogic;

    [SerializeField]
    int damage = 1;
    [SerializeField, HideInInspector]
    float rangedCoolDownInSeconds;
    [SerializeField, HideInInspector]
    float rangedCoolDownInSecondsDefault;


    private void OnValidate()
    {
        InitializeFromRangedAttack();

        if(enemyListMBDO == null || playerRefMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref playerRefMBDO);
            mBDOInitializationHelper.SetupMBDO(ref enemyListMBDO);
        }


        if(entityLogic==null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    private void Start()
    {
        target = playerRefMBDO.player;
    }

    private void OnEnable()
    {
        if (enemyListMBDO == null)
            Debug.Log(gameObject.ToString() +" "+ this + gameObject.name);
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
        OnValidate();
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    void Update()
    {
        rangedCoolDownInSeconds = Mathf.Max(0, rangedCoolDownInSeconds - Time.deltaTime);

        if (InRange())
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
        LayerMask layerMask= 1 << 11;
        layerMask = ~layerMask;
        result = Physics2D.Raycast(transform.position, target.position - transform.position, Mathf.Infinity, layerMask);
        return result.collider.gameObject.layer == 10;
    }

    private bool InRange()
    {
        if (target == null)
            return false;
        return Vector2.Distance(transform.position, target.position) < range;
    }


    //shoot enemy projectile
    void EnemyRangedAttack()
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
                temp.TakeDamage(damage);
            }
        }
    }
}
