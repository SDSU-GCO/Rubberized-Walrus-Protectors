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
    public PlayerRefSO playerRefSO;
    public EnemyListSO enemyListSO;

    private void OnValidate()
    {
        //Debug.Log("\n");
        //Debug.Log(gameObject.name + "::: IsPartOfPrefabInstance: " + PrefabUtility.IsPartOfPrefabInstance(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfAnyPrefab: " + PrefabUtility.IsPartOfAnyPrefab(gameObject));
        //Debug.Log(gameObject.name + "::: IsAnyPrefabInstanceRoot: " + PrefabUtility.IsAnyPrefabInstanceRoot(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfImmutablePrefab: " + PrefabUtility.IsPartOfImmutablePrefab(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfModelPrefab: " + PrefabUtility.IsPartOfModelPrefab(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfNonAssetPrefabInstance: " + PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfPrefabAsset: " + PrefabUtility.IsPartOfPrefabAsset(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfRegularPrefab: " + PrefabUtility.IsPartOfRegularPrefab(gameObject));
        //Debug.Log(gameObject.name + "::: IsPartOfVariantPrefab: " + PrefabUtility.IsPartOfVariantPrefab(gameObject));
        //Debug.Log(gameObject.name + "::: IsPrefabAssetMissing: " + PrefabUtility.IsPrefabAssetMissing(gameObject));
        //Debug.Log("\n");

        GameObject outerGameObject = PrefabUtility.GetOutermostPrefabInstanceRoot(this);

        PrefabStage prefabStage = PrefabStageUtility.GetPrefabStage(gameObject);
        bool isPrefab = false;

        if (outerGameObject != null)
            isPrefab = (outerGameObject.scene == gameObject.scene);
        else
            isPrefab = false;

        Debug.Log(gameObject.name + "::: isPrefab: " + isPrefab);

        if (isPrefab!=true)
        {
            if (playerRefSO == null || enemyListSO == null)
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
                if (enemyListSO == null)
                {
                    scriptableObjectReferences.tryPopulate(ref enemyListSO);
                    if (enemyListSO == null)
                        Debug.LogWarning("ScriptableObject Object enemyListSO: " + enemyListSO + "is null in: " + this);
                }
            }
        }
    }

    private void Start()
    {
        target = playerRefSO.player;
    }

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
