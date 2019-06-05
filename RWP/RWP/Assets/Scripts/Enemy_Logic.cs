using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity_Logic))]
public class Enemy_Logic : MonoBehaviour
{
    Entity_Logic entityLogic;
    Transform target;
    public float range = 4;
    RaycastHit2D result;
    public PlayerRefSO playerRefSO;
    public EnemyListSO enemyListSO;

    private void OnEnable()
    {
        if (enemyListSO == null)
            Debug.Log(gameObject.ToString() +" "+ this + gameObject.name);
        enemyListSO.enemies.Add(this);
        enemyListSO.update.Invoke();
        target = playerRefSO.player;
    }

    private void OnDisable()
    {
        enemyListSO.enemies.Remove(this);
        enemyListSO.update.Invoke();
    }

    private void Awake()
    {
        if (entityLogic == null)
        {
            entityLogic = GetComponent<Entity_Logic>();
        }
    }

    void Update()
    {
        if (inRange() && hasLineOfSight())
        {
            entityLogic.doAttack();
        }
    }

    private bool hasLineOfSight()
    {
        LayerMask layerMask= 1 << 11;
        layerMask = ~layerMask;
        result = Physics2D.Raycast(transform.position, target.position - transform.position, Mathf.Infinity, layerMask);
        return result.collider.gameObject.layer == 10;
    }

    private bool inRange()
    {
        if (target == null)
            return false;
        return Vector2.Distance(transform.position, target.position) < range;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 10)
        {
            Entity_Logic temp;
            temp = collision.gameObject.GetComponent<Entity_Logic>();
            if (temp != null)
            {                
                temp.TakeDamage(temp.damage);
            }
        }
    }
}
