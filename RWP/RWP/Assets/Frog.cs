using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Enemy_Logic))]
public class Frog : MonoBehaviour
{
    public PlayerRefSO PlayerRefSO;
    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Enemy_Logic enemy_Logic;
    Transform target;
    private void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemy_Logic = GetComponent<Enemy_Logic>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = PlayerRefSO.player;
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > 1.5)
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        if(target!=null && Vector2.Distance(target.position, transform.position)<=enemy_Logic.range)
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
