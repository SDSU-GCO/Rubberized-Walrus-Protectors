using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Enemy_Logic))]
public class Frog : MonoBehaviour
{
    public float timeToLoad = 1;

    [SerializeField, HideInInspector]
    private PlayerRefMBDO playerRefMBDO;

    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Enemy_Logic enemy_Logic;
    private Transform target;
    private float timer = 1;

    private void OnValidate()
    {
        if (playerRefMBDO == null)
        {
            //idk if this creates a lot of garbage for the gc...
            MBDOInitializationHelper mBDOInitializationHelper = new MBDOInitializationHelper(this);

            mBDOInitializationHelper.SetupMBDO(ref playerRefMBDO);
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
    private void Update()
    {
        timer = Mathf.Max(0, timer - Time.deltaTime);
        if (timer <= 0)
        {
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (target != null && Vector2.Distance(target.position, transform.position) <= enemy_Logic.range)
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