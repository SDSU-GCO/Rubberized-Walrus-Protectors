using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;

[RequireComponent(typeof(Entity_Logic))]
public class Enemy_Logic : MonoBehaviour
{
    Entity_Logic entityLogic;
    Transform target;
    public float range = 4;
    RaycastHit2D result;
    public PlayerRefMBDO playerRefMBDO = null;
    public EnemyListMBDO enemyListMBDO = null;

    private void OnValidate()
    {
         
        GameObject cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
        MBDatabaseObjectReferences mbDatabaseObjectReferences = null;
        if (cardinalSubsystem != null)
            mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDatabaseObjectReferences>();

        if (cardinalSubsystem != null && cardinalSubsystem.scene != gameObject.scene)
        {
            playerRefMBDO = null;
            enemyListMBDO = null;
        }
        if (playerRefMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            if(mbDatabaseObjectReferences!=null)
                mbDatabaseObjectReferences.tryPopulate(out playerRefMBDO);
            if (playerRefMBDO == null)
                Debug.LogWarning("ScriptableObject Object playerRefSO: " + playerRefMBDO + "is null in: " + this);
        }
        if (enemyListMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            if (mbDatabaseObjectReferences != null)
                mbDatabaseObjectReferences.tryPopulate(out enemyListMBDO);
            if (enemyListMBDO == null)
                Debug.LogWarning("ScriptableObject Object enemyListSO: " + enemyListMBDO + "is null in: " + this);
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
        if (inRange())
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
        if (collision.collider.gameObject.layer == 10 && gameObject.layer == 11)
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
