using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Enemy_Logic))]
public class Frog : MonoBehaviour
{
    public float timeToLoad = 1;
    public PlayerRefMBDO playerRefMBDO;
    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Enemy_Logic enemy_Logic;
    Transform target;
    float timer = 1;

    private void OnValidate()
    {

        GameObject cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
        MBDatabaseObjectReferences mbDatabaseObjectReferences = null;
        if (cardinalSubsystem != null)
            mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDatabaseObjectReferences>();

        if (cardinalSubsystem != null && cardinalSubsystem.scene != gameObject.scene)
        {
            playerRefMBDO = null;
        }
        if (playerRefMBDO == null && cardinalSubsystem != null && cardinalSubsystem.scene == gameObject.scene)
        {
            if (mbDatabaseObjectReferences != null)
                mbDatabaseObjectReferences.tryPopulate(out playerRefMBDO);
            if (playerRefMBDO == null)
                Debug.LogWarning("ScriptableObject Object playerRefSO: " + playerRefMBDO + "is null in: " + this);
        }
    }

    private void Start()
    {
        target = playerRefMBDO.player;
    }

    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemy_Logic = GetComponent<Enemy_Logic>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = timeToLoad;
        target = playerRefMBDO.player;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Max(0 , timer - Time.deltaTime);
        if (timer<=0)
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

        if (target!=null && Vector2.Distance(target.position, transform.position)<=enemy_Logic.range)
        {
            if (target.position.x <= transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }
}
